using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using Quokka.RTL.Simulatot;

namespace QRV32.Tests
{
    [TestClass]
    public class InstructionDecoderTests
    {
        [TestMethod]
        public void RTypeTest()
        {
            var sim = new CombinationalRTLSimulator<RTypeDecoderModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x8358D8C1 });

            Assert.AreEqual("1000001", tl.Funct7.AsBinaryString());
            Assert.AreEqual("10101", tl.RS2.AsBinaryString());
            Assert.AreEqual("10001", tl.RS1.AsBinaryString());
            Assert.AreEqual("101", tl.Funct3.AsBinaryString());
            Assert.AreEqual("10001", tl.RD.AsBinaryString());
            Assert.AreEqual("1000001", tl.OpCode.AsBinaryString());
        }

        [TestMethod]
        public void ITypeTest()
        {
            var sim = new CombinationalRTLSimulator<ITypeDecoderModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x8018D8C1 });

            Assert.AreEqual("100000000001".PadLeft(32, '1'), tl.Imm.AsBinaryString());
            Assert.AreEqual("10001", tl.RS1.AsBinaryString());
            Assert.AreEqual("101", tl.Funct3.AsBinaryString());
            Assert.AreEqual("10001", tl.RD.AsBinaryString());
            Assert.AreEqual("1000001", tl.OpCode.AsBinaryString());
        }

        [TestMethod]
        public void STypeTest()
        {
            var sim = new CombinationalRTLSimulator<STypeDecoderModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x8358D8C1 });

            Assert.AreEqual("100000110001".PadLeft(32, '1'), tl.Imm.AsBinaryString());
            Assert.AreEqual("10101", tl.RS2.AsBinaryString());
            Assert.AreEqual("10001", tl.RS1.AsBinaryString());
            Assert.AreEqual("101", tl.Funct3.AsBinaryString());
            Assert.AreEqual("1000001", tl.OpCode.AsBinaryString());
        }

        [TestMethod]
        public void BTypeTest()
        {
            var sim = new CombinationalRTLSimulator<BTypeDecoderModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0xC358D9C1 });
            Assert.AreEqual("1110000110010".PadLeft(32, '1'), tl.Imm.AsBinaryString());

            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0xC358D941 });
            Assert.AreEqual("1010000110010".PadLeft(32, '1'), tl.Imm.AsBinaryString());

            Assert.AreEqual("10101", tl.RS2.AsBinaryString());
            Assert.AreEqual("10001", tl.RS1.AsBinaryString());
            Assert.AreEqual("101", tl.Funct3.AsBinaryString());
            Assert.AreEqual("1000001", tl.OpCode.AsBinaryString());
        }

        [TestMethod]
        public void UTypeTest()
        {
            var sim = new CombinationalRTLSimulator<UTypeDecoderModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x800018C1 });

            Assert.AreEqual("10000000000000000001".PadRight(32, '0'), tl.Imm.AsBinaryString());
            Assert.AreEqual("10001", tl.RD.AsBinaryString());
            Assert.AreEqual("1000001", tl.OpCode.AsBinaryString());
        }

        [TestMethod]
        public void JTypeTest()
        {
            var sim = new CombinationalRTLSimulator<JTypeDecoderModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0xC03818C1 });

            Assert.AreEqual("110000001110000000010".PadLeft(32, '1'), tl.Imm.AsBinaryString());
            Assert.AreEqual("10001", tl.RD.AsBinaryString());
            Assert.AreEqual("1000001", tl.OpCode.AsBinaryString());
        }
    }
}
