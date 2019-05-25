using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Experimental.Tests
{
    enum EmptyEnum
    {

    }

    enum OneStateZero
    {
        Stage1
    }

    enum OneStateOne
    {
        Stage1 = 1
    }
    enum OneStateTwo
    {
        Stage1 = 2
    }
    enum OneStateThree
    {
        Stage1 = 3
    }
    enum OneStateFour
    {
        Stage1 = 4
    }
    enum BitMask
    {
        None,
        Bit1 = 1,
        Bit2 = 2,
        Bit3 = 4,
        Bit4 = 8,
        Bit5 = 16,
        Bit6 = 32,
        Bit8 = 128
    }

    [TestClass]
    public class RTLModuleHelperTests
    {
        [TestMethod]
        public void EnumSizeTest()
        {
            Assert.AreEqual(1, RTLModuleHelper.SizeOfEnum(typeof(EmptyEnum)));
            Assert.AreEqual(1, RTLModuleHelper.SizeOfEnum(typeof(OneStateZero)));
            Assert.AreEqual(1, RTLModuleHelper.SizeOfEnum(typeof(OneStateOne)));
            Assert.AreEqual(2, RTLModuleHelper.SizeOfEnum(typeof(OneStateTwo)));
            Assert.AreEqual(2, RTLModuleHelper.SizeOfEnum(typeof(OneStateThree)));
            Assert.AreEqual(3, RTLModuleHelper.SizeOfEnum(typeof(OneStateFour)));
            Assert.AreEqual(8, RTLModuleHelper.SizeOfEnum(typeof(BitMask)));
        }
    }
}
