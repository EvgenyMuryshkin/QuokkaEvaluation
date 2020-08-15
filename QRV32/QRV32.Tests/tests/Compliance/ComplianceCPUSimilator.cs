using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.Tests;
using System.Collections.Generic;

namespace QRV32.Compliance
{
    public class ComplianceCPUSimilator : CPUSimulator
    {
        int _dataMarkerAddress = 0;
        public int Asserts = 0;
        public bool HasNonZeroValues = false;

        public List<uint> AssertedValues = new List<uint>();

        public ComplianceCPUSimilator(int dataMarkerAddress)
        {
            _dataMarkerAddress = dataMarkerAddress;
        }

        protected override void ECall()
        {
            var actual = MemoryBlock[_dataMarkerAddress];
            var expected = MemoryBlock[_dataMarkerAddress + 1];

            if (actual == 0x87654321)
                Assert.Fail($"Asserting data marker value, should not be like that");

            Assert.AreEqual(expected, actual, "Value does not match");

            Asserts++;
            HasNonZeroValues |= (expected != 0);
            AssertedValues.Add(actual);
        }
    }
}
