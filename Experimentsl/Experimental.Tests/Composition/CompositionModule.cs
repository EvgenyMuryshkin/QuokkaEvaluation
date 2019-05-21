using System;

namespace QuokkaTests.Experimental
{
    public class CompositionModule : CombinationalRTLModule<CompositionInputs>
    {
        public EmitterModule Emitter = new EmitterModule();
        public TransmitterModule Transmitter = new TransmitterModule();
        public ReceiverModule Receiver = new ReceiverModule();

        public bool HasData => Receiver.HasData;
        public byte Data => Receiver.Data;

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
                Data = Emitter.Data,
                Trigger = Emitter.HasData,
                Ack = Receiver.HasData
            });

            Receiver.Schedule(() => new ReceiverInputs()
            {
                Bit = Transmitter.Bit,
                IsValid = Transmitter.IsTransmitting,
                Ack = true
            });
        }
    }
}
