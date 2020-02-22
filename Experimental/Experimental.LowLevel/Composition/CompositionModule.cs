using Quokka.RTL;
using System;

namespace QuokkaTests.Experimental
{
    public class CompositionModule : RTLCombinationalModule<CompositionInputs>
    {
        public EmitterModule Emitter = new EmitterModule();
        public TransmitterModule Transmitter = new TransmitterModule();
        public ReceiverModule Receiver = new ReceiverModule();

        public bool HasData => Receiver.HasData;
        public byte Data => Receiver.Data;
        public byte Fixed1 => 10;
        public byte Fixed2 => 20;
        public byte Fixed3 = 20;

        public override void Schedule(Func<CompositionInputs> inputsFactory)
        {
            base.Schedule(inputsFactory);

            Emitter.Schedule(() => new EmitterInputs()
                {
                    IsEnabled = Inputs.IsEnabled,
                    Ack = Transmitter.IsReady
                });

            Transmitter.Schedule(() => new TransmitterInputs()
                {
                    Trigger = Emitter.HasData,
                    Ack = Receiver.HasData,
                    Data = Receiver.Data
               });

            Receiver.Schedule(() => new ReceiverInputs()
                {
                    IsValid = Transmitter.IsTransmitting,
                    Bit = Transmitter.Bit,
                    Ack = true
                });
        }
    }
}
