using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System.Collections.Generic;
using System.Linq;

namespace QuokkaTests.Experimental
{
    [TestClass]
    public class CompositionTests
    {
        [TestMethod]
        public void Combined()
        {
            var emitter = new EmitterModule();
            var transmitter = new TransmitterModule();
            var receiver = new ReceiverModule();

            emitter.Schedule(() => new EmitterInputs()
            {
                Ack = transmitter.IsReady
            });

            transmitter.Schedule(() => new TransmitterInputs()
            {
                Data = emitter.Data,
                Trigger = emitter.HasData,
                Ack = receiver.HasData
            });

            receiver.Schedule(() => new ReceiverInputs()
            {
                Bit = transmitter.Bit,
                IsValid = transmitter.IsTransmitting,
                Ack = true
            });

            var allModules = new List<IRTLModule>() { emitter, transmitter, receiver };

            var receivedData = new List<byte>();
            var clock = 0;

            var bytesToProcess = 256;
            var maxCycles = 1000000;

            while (receivedData.Count < bytesToProcess && clock < maxCycles)
            {
                allModules.ForEach(m => m.Stage());

                if (receiver.HasData)
                {
                    receivedData.Add(receiver.Data);
                }

                allModules.ForEach(m => m.Commit());
                clock++;
            }

            Assert.AreEqual(bytesToProcess, receivedData.Count);
            var missing = Enumerable
                .Range(0, bytesToProcess)
                .Zip(receivedData, (e, a) => new { e, a })
                .Where(p => p.e != p.a)
                .Select(p => $"E: {p.e}, A: {p.a}")
                .ToList();

            Assert.AreEqual(0, missing.Count, missing.ToCSV());
        }
    }
}
