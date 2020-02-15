using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FPGA.OrbitalCalc.Controllers;
using FPGA.OrbitalCalc;
using QuokkaIntegrationTests;

namespace FPGA.OrbitalCalc.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void Quokka_VEsc()
        {
            var rnd = new Random(Environment.TickCount);

            using (var port = new QuokkaPort())
            {
                // escape velocities from the surface of objects
                var cases = new[]
                {
                    // sun
                    new
                    {
                        // radius, in meters
                        R = 695510e+3f,
                        // mass, in kgs
                        M = 1.989e+30f,
                        // VEsc, is m/s
                        V = 617500f
                    },
                    // earth
                    new 
                    {
                        R = 6371e+3f,
                        M = 5.972e+24f,
                        V = 11186f
                    },
                    // mars
                    new
                    {
                        R = 3389.5e+3f,
                        M = 6.39e+23f,
                        V = 5030f
                    },
                };

                foreach (var testCase in cases)
                {
                    float calculated = FPGAOrbitalCalc.VEsc(testCase.M, testCase.R);

                    var err = Math.Abs((calculated - testCase.V) / testCase.V);
                    Assert.IsTrue(err < 0.01);

                    port.WriteFloat(testCase.M);
                    port.WriteFloat(testCase.R);

                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(calculated, actual, $"Failed for {testCase}");
                }
            }
        }

        [TestMethod]
        public void Quokka_VOrbit()
        {
            using (var port = new QuokkaPort())
            {
                var r0 = 6371e+3f;
                // orbit velocities around the earth
                var cases = new[]
                {
                    // low orbit
                    new
                    {
                        R = r0 + 200e+3f,
                        M = 5.972e+24f,
                        V = 7800f
                    },

                    // ISS orbit
                    new
                    {
                        R = r0 + 409e+3f,
                        M = 5.972e+24f,
                        V = 7660f
                    },

                    // geostationary orbit
                    new
                    {
                        R = r0 + 35786e+3f,
                        M = 5.972e+24f,
                        V = 3100f
                    },

                    // moon orbit
                    new
                    {
                        R = r0 + 399000e+3f,
                        M = 5.972e+24f,
                        V = 991f
                    },
                };

                foreach (var testCase in cases)
                {
                    float calculated = FPGAOrbitalCalc.VOrbit(testCase.M, testCase.R);

                    var err = Math.Abs((calculated - testCase.V) / testCase.V);
                    Assert.IsTrue(err < 0.01);

                    port.WriteFloat(testCase.M);
                    port.WriteFloat(testCase.R);

                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(calculated, actual, $"Failed for {testCase}");
                }
            }
        }

        [TestMethod]
        public void Quokka_TOrbit()
        {
            using (var port = new QuokkaPort())
            {
                var r0 = 6371e+3f;
                // orbit periods around the earth
                var cases = new[]
                {
                    // low orbit
                    new
                    {
                        R = r0 + 200e+3f,
                        M = 5.972e+24f,
                        T = 5301.04f
                    },

                    // ISS orbit
                    new
                    {
                        R = r0 + 409e+3f,
                        M = 5.972e+24f,
                        T = 5555.96f
                    },

                    // geostationary orbit
                    new
                    {
                        R = r0 + 35786e+3f,
                        M = 5.972e+24f,
                        T = 86142.8f
                    },

                    // moon orbit
                    new
                    {
                        R = r0 + 399000e+3f,
                        M = 5.972e+24f,
                        T = 2.56858e+6f
                    },
                };

                foreach (var testCase in cases)
                {
                    float calculated = FPGAOrbitalCalc.TOrbit(testCase.M, testCase.R);

                    var err = Math.Abs((calculated - testCase.T) / testCase.T);
                    Assert.IsTrue(err < 0.01);

                    port.WriteFloat(testCase.M);
                    port.WriteFloat(testCase.R);

                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(calculated, actual, $"Failed for {testCase}");
                }
            }
        }

        [TestMethod]
        public void Quokka_DeltaVInnerOrbit()
        {
            using (var port = new QuokkaPort())
            {
                var r0 = 6371e+3f;

                // http://www.satsig.net/orbit-research/delta-v-geo-injection-calculator.htm
                var cases = new[]
                {
                    // low orbit
                    new
                    {
                        R1 = r0 + 176e+3f,
                        R2 = r0 + 350e+3f,
                        M = 5.972e+24f,
                        DV = 50.92f
                    },
                    // to ISS
                    new
                    {
                        R1 = r0 + 100e+3f,
                        R2 = r0 + 410e+3f,
                        M = 5.972e+24f,
                        DV = 91.12f
                    },
                    // low orbit to GSO
                    new
                    {
                        R1 = r0 + 200e+3f,
                        R2 = r0 + 35786e+3f,
                        M = 5.972e+24f,
                        DV = 2454.58f
                    },
                };

                foreach (var testCase in cases)
                {
                    float calculated = FPGAOrbitalCalc.DeltaVInnerOrbit(testCase.M, testCase.R1, testCase.R2);

                    var err = Math.Abs((calculated - testCase.DV) / testCase.DV);
                    Assert.IsTrue(err < 0.01);

                    port.WriteFloat(testCase.M);
                    port.WriteFloat(testCase.R1);
                    port.WriteFloat(testCase.R2);

                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(calculated, actual, $"Failed for {testCase}");
                }
            }
        }

        [TestMethod]
        public void Quokka_DeltaVOuterOrbit()
        {
            using (var port = new QuokkaPort())
            {
                var r0 = 6371e+3f;

                // http://www.satsig.net/orbit-research/delta-v-geo-injection-calculator.htm
                var cases = new[]
                {
                    // low orbit
                    new
                    {
                        R1 = r0 + 176e+3f,
                        R2 = r0 + 350e+3f,
                        M = 5.972e+24f,
                        DV = 50.58f
                    },
                    // to ISS
                    new
                    {
                        R1 = r0 + 100e+3f,
                        R2 = r0 + 410e+3f,
                        M = 5.972e+24f,
                        DV = 90.06f
                    },
                    // low orbit to GSO
                    new
                    {
                        R1 = r0 + 200e+3f,
                        R2 = r0 + 35786e+3f,
                        M = 5.972e+24f,
                        DV = 1477.27f
                    },
                };

                foreach (var testCase in cases)
                {
                    float calculated = FPGAOrbitalCalc.DeltaVOuterOrbit(testCase.M, testCase.R1, testCase.R2);

                    var err = Math.Abs((calculated - testCase.DV) / testCase.DV);
                    Assert.IsTrue(err < 0.01);

                    port.WriteFloat(testCase.M);
                    port.WriteFloat(testCase.R1);
                    port.WriteFloat(testCase.R2);

                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(calculated, actual, $"Failed for {testCase}");
                }
            }
        }

        [TestMethod]
        public void Quokka_DeltaVInclinationOrbitOptimized()
        {
            using (var port = new QuokkaPort())
            {
                var r0 = 6371e+3f;

                // http://www.satsig.net/orbit-research/delta-v-geo-injection-calculator.htm
                var cases = new[]
                {
                    // low orbit to GSO from Kennedy
                    new
                    {
                        R1 = r0 + 200e+3f,
                        R2 = r0 + 35786e+3f,
                        M = 5.972e+24f,
                        Inc = 0.4974188f, // ~28.5*
                        DV = 1836.49f
                    },
                };

                foreach (var testCase in cases)
                {
                    float calculated = FPGAOrbitalCalc.DeltaVInclinationOrbitOptimized(testCase.M, testCase.R1, testCase.R2, testCase.Inc);

                    var err = Math.Abs((calculated - testCase.DV) / testCase.DV);
                    Assert.IsTrue(err < 0.01);

                    port.WriteFloat(testCase.M);
                    port.WriteFloat(testCase.R1);
                    port.WriteFloat(testCase.R2);
                    port.WriteFloat(testCase.Inc);

                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(calculated, actual, $"Failed for {testCase}");
                }
            }
        }

        [TestMethod]
        public void Quokka_DeltaVInclinationOrbit()
        {
            using (var port = new QuokkaPort())
            {
                var r0 = 6371e+3f;

                // http://www.satsig.net/orbit-research/delta-v-geo-injection-calculator.htm
                var cases = new[]
                {
                    // low orbit to GSO from Kennedy
                    new
                    {
                        R1 = r0 + 200e+3f,
                        R2 = r0 + 35786e+3f,
                        M = 5.972e+24f,
                        Inc = 0.4974188f, // ~28.5*
                        DV = 1836.49f
                    },
                };

                foreach (var testCase in cases)
                {
                    float calculated = FPGAOrbitalCalc.DeltaVInclinationOrbit(testCase.M, testCase.R1, testCase.R2, testCase.Inc);

                    var err = Math.Abs((calculated - testCase.DV) / testCase.DV);
                    Assert.IsTrue(err < 0.01);

                    port.WriteFloat(testCase.M);
                    port.WriteFloat(testCase.R1);
                    port.WriteFloat(testCase.R2);
                    port.WriteFloat(testCase.Inc);

                    var actualBytes = port.Read(4, true, port.DefaultTimeout);
                    var actual = TestConverters.FloatFromByteArray(actualBytes);

                    Assert.AreEqual(calculated, actual, $"Failed for {testCase}");
                }
            }
        }
    }
}
