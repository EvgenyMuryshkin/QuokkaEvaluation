using BHDto;
using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Threading.Tasks;

namespace BHDevice
{
    public struct State
    {
        public ushort Index;
        public ushort Current;
        public ushort Average;
        public ProximityState DistanceState;
        public ushort LastMaxDistance;
        public bool RaiseGoneNotification;
    }

    [BoardConfig(Name = "Quokka")]
    public static class Device
    {
        public static async Task Aggregator(
            FPGA.OutputSignal<bool> TXD,
            FPGA.OutputSignal<bool> WiFiRXD,

            // banks 
            FPGA.OutputSignal<bool> Bank1,
            FPGA.OutputSignal<bool> Bank2,

            FPGA.InputSignal<bool> Echo,
            FPGA.OutputSignal<bool> Trigger
            )
        {
            FPGA.Signal<bool> internalTXD = true;

            FPGA.Config.Link(internalTXD, TXD);
            FPGA.Config.Link(internalTXD, WiFiRXD);

            QuokkaBoard.OutputBank(Bank1);
            QuokkaBoard.InputBank(Bank2);

            State state = new State();
            object stateLock = new object();
            DistanceMeasuring(state, stateLock, Echo, Trigger);
            StateReporter(state, stateLock, internalTXD);
        }

        public static void DistanceMeasuring(
            State state,
            object stateLock,
            FPGA.InputSignal<bool> echo,
            FPGA.OutputSignal<bool> trigger
            )
        {
            ushort[] buff = new ushort[10];
            int addr = 0;
            bool filled = false;

            Sequential measureHandler = () =>
            {
                ushort distance = 0;
                HCSR04.Measure(echo, trigger, out distance);

                buff[addr] = distance;
                addr++;

                if (addr == buff.Length)
                {
                    filled = true;
                    addr = 0;
                }

                state.Current = distance;

                if (filled)
                {
                    // get averange of distances
                    Filters.Average(buff, out state.Average);

                    if (state.Average < 50)
                    {
                        state.DistanceState = ProximityState.Alert;
                    }
                    else if (state.Average < 150)
                    {
                        state.DistanceState = ProximityState.Warning;
                    }
                    else
                    {
                        state.DistanceState = ProximityState.Safe;
                    }

                    if (state.Average < 200)
                    {
                        if (state.Average > (state.LastMaxDistance + 10))
                        {
                            state.LastMaxDistance = state.Average;
                            lock (stateLock)
                            {
                                state.RaiseGoneNotification = true;
                            }
                        }
                        else if (state.Average < (state.LastMaxDistance - 5))
                        {
                            state.LastMaxDistance = state.Average;
                        }
                    }
                    else
                    {
                        state.LastMaxDistance = state.Average;
                    }
                }
                else
                {
                    state.DistanceState = ProximityState.Measuring;
                }
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(200), measureHandler);
        }

        public static void StateReporter(
            State state,
            object stateLock,
            FPGA.Signal<bool> TXD)
        {
            Sequential reportState = () =>
            {
                var dto = new ReportDTO();
                dto.Idx = state.Index;
                dto.Cur = state.Current;
                dto.Ave = state.Average;
                dto.Prx = state.DistanceState;
                dto.Max = state.LastMaxDistance;

                lock(stateLock)
                {
                    dto.Gone = (byte)(state.RaiseGoneNotification ? 1 : 0);
                    state.RaiseGoneNotification = false;
                }
                JSON.SerializeToUART(dto, TXD);
            };

            FPGA.Config.OnTimer(TimeSpan.FromMilliseconds(500), reportState);
        }
    }
}
