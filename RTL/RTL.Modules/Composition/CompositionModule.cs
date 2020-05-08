using Quokka.RTL;
using System;

namespace RTL.Modules
{
    public class CompositionInputs
    {
        public bool IsEnabled = true;
    }

    public class CompositionModule : RTLCombinationalModule<CompositionInputs>
    {
        public EmitterModule Emitter = new EmitterModule();
        public TransmitterModule Transmitter = new TransmitterModule();
        public ReceiverModule Receiver = new ReceiverModule();

        public bool HasData => Receiver.HasData;
        public byte Data => Receiver.Data;

        protected override void OnSchedule(Func<CompositionInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            Emitter.Schedule(() => new EmitterInputs()
                {
                    IsEnabled = Inputs.IsEnabled,
                    Ack = Transmitter.IsReady
                });

            Transmitter.Schedule(() => new TransmitterInputs()
                {
                    Trigger = Emitter.HasData,
                    Data = Emitter.Data,
                    Ack = Receiver.HasData
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
