## Create simple combinational module

It is assumed that you are familiar with general structure of .NET class library (assemblies, namespaces etc.) and unit test system.

### Declare class for module inputs. 
This is simple class with raw fields\properties
```
    public class BitArrayInputs
    {
        public byte Value;
    }
```

### Declare combinational module
Every combinational module must derive from base generic class **RTLCombinationalModule<>**, and must provide type of module inputs.

Every public property on class are **module outputs** (e.g. Direct, High, Low etc.)

Every non-public property are **internal signals** (e.g. Bits)

This example module performs some simple bit rearrangements of input value

Size of the outputs are determined in translation time by fetching value of proerpty and checking its size.
e.g. width of **High* signal will be 4, as it is accessing 4 bits of internal signal **Bits**

```
    public class BitArrayModule : RTLCombinationalModule<BitArrayInputs>
    {
        RTLBitArray Bits => Inputs.Value;
        public RTLBitArray Direct => Bits;
        public RTLBitArray High => Bits[7, 4];
        public RTLBitArray Low => Bits[3, 0];
        public RTLBitArray Reversed => Bits[0, 7];
        public RTLBitArray ReversedHigh => Bits[4, 7];
        public RTLBitArray ReversedLow => Bits[0, 3];
        public RTLBitArray Picks => new RTLBitArray(Bits[6,5], Bits[1,2]);
    }
```

### Create unit test
Any available unit testing frameworks can be used for RTL simulation.
Here is an example of unit test using MSTest. 

**CombinationalRTLSimulator** provides a shortcut for simulating combinational modules, with one-liner to run test case

**NOTE: Combinational loops cannot be simulated, runtime will crash with StackOverflow exception.**

**HDL will be translated and can be used**


```
    [TestClass]
    public class ExampleTests
    {
        [TestMethod]
        public void BitArrayModuleTest()
        {
            // create simulator with module type
            var sim = new CombinationalRTLSimulator<BitArrayModule>();

            // run single iteration with provided inputs
            sim.TopLevel.Cycle(new BitArrayInputs() { Value = 0xC2 });

            // assert output values
            Assert.AreEqual(0xC2, (byte)sim.TopLevel.Direct);
            Assert.AreEqual(0xC,  (byte)sim.TopLevel.High);
            Assert.AreEqual(0x2,  (byte)sim.TopLevel.Low);
            Assert.AreEqual(0x43, (byte)sim.TopLevel.Reversed);
            Assert.AreEqual(0x3,  (byte)sim.TopLevel.ReversedHigh);
            Assert.AreEqual(0x4,  (byte)sim.TopLevel.ReversedLow);
            Assert.AreEqual(0xA,  (byte)sim.TopLevel.Picks);
        }
    }
```    

Various dev tools can be used to calculate unit test metrics e.g. test coverage.
