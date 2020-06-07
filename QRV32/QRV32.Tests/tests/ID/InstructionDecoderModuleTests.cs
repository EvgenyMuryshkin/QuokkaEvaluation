using Microsoft.VisualStudio.TestTools.UnitTesting;
using QRV32.CPU;
using Quokka.RTL.Simulator;

namespace QRV32.Tests
{
    [TestClass]
    public class InstructionDecoderModuleTests
    {
        [TestMethod]
        public void Test()
        {
            var sim = new CombinationalRTLSimulator<InstructionDecoderModule>();
            var tl = sim.TopLevel;
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x8358D8C1 });

            // regs, opcodes
            Assert.AreEqual("1000001", tl.OpCode.AsBinaryString(), "OpCode");
            Assert.AreEqual("10001", tl.RD.AsBinaryString(), "RD");
            Assert.AreEqual("10001", tl.RS1.AsBinaryString(), "RS1");
            Assert.AreEqual("10101", tl.RS2.AsBinaryString(), "RS2");
            Assert.AreEqual("101", tl.Funct3.AsBinaryString(), "Funct2");
            Assert.AreEqual("1000001", tl.Funct7.AsBinaryString(), "Funct7");

            // immediates

            // IType
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x8018D8C1 });
            Assert.AreEqual("100000000001".PadLeft(32, '1'), tl.ITypeImm.AsBinaryString(), "IType");

            // SType
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x8358D8C1 });
            Assert.AreEqual("100000110001".PadLeft(32, '1'), tl.STypeImm.AsBinaryString(), "SType");

            // BType
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0xC358D9C1 });
            Assert.AreEqual("1110000110010".PadLeft(32, '1'), tl.BTypeImm.AsBinaryString(), "BType");

            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0xC358D941 });
            Assert.AreEqual("1010000110010".PadLeft(32, '1'), tl.BTypeImm.AsBinaryString(), "BType");

            // UType
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0x800018C1 });
            Assert.AreEqual("10000000000000000001".PadRight(32, '0'), tl.UTypeImm.AsBinaryString(), "UType");

            // JType
            tl.Cycle(new InstructionDecoderInputs() { Instruction = 0xC03818C1 });
            Assert.AreEqual("110000001110000000010".PadLeft(32, '1'), tl.JTypeImm.AsBinaryString(), "JType");
        }
    }
}
