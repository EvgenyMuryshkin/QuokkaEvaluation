using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FPGA.Trigonometry.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Tools_Normalize()
        {
            var rnd = new Random(Environment.TickCount);

            List<double> items = new List<double>()
            {
                -Math.PI * 2,
                -Math.PI,
                0,
                Math.PI,
                Math.PI * 2,
            };

            items
                .AddRange(
                    Enumerable
                        .Range(-500000, 1000000)
                        .Select(idx => Math.PI * 2 * rnd.NextDouble() * idx));

            foreach (var data in items)
            {
                var normalized = FPGATrigonometryTools.Normalize((float)data);

                Assert.IsTrue(normalized >= 0 && normalized < FPGATrigonometryConstants.TwoPI, $"failed for {data}, got {normalized}");
            }
        }

        [TestMethod]
        public void Tools_Q1Project()
        {
            var rnd = new Random(Environment.TickCount);

            foreach (var idx in Enumerable.Range(0, 10000))
            {
                float rad = (float)(Math.PI * 2 * rnd.NextDouble());

                var q1 = FPGATrigonometryTools.Q1Project(rad);

                Assert.IsTrue(q1 >= 0 && q1 <= FPGATrigonometryConstants.HalfPI);
            }
        }

        const int Steps = 500000;
        [TestMethod]
        public void Lib_Sin()
        {
            float stepDelta = FPGATrigonometryConstants.TwoPI / Steps;

            var posError = double.MinValue;
            var negError = double.MaxValue;
            var absError = 0d;

            // able to achieve about this accuracy
            var eps = 3.7e-6;

            for (var step = 0; step < Steps; step++)
            {
                var rad = step * stepDelta;

                // calculate sin
                var taylorSin = FPGATrigonometry.Sin(rad);
                var calculatedSin = Math.Sin(rad);
                var sinDelta = taylorSin - calculatedSin;

                absError = Math.Max(absError, Math.Abs(sinDelta));
                posError = Math.Max(sinDelta, posError);
                negError = Math.Min(sinDelta, negError);

                Assert.IsTrue(sinDelta < eps, $"Failed for sin({rad})");
            }
        }

        [TestMethod]
        public void Lib_Cos()
        {
            float stepDelta = FPGATrigonometryConstants.TwoPI / Steps;

            var posError = double.MinValue;
            var negError = double.MaxValue;
            var absError = 0d;

            // able to achieve about this accuracy
            var eps = 2.5e-5;

            for (var step = 0; step < Steps; step++)
            {
                var rad = step * stepDelta;

                // calculate cos
                var taylorCos = FPGATrigonometry.Cos(rad);
                var calculatedCos = Math.Cos(rad);
                var cosDelta = taylorCos - calculatedCos;

                absError = Math.Max(absError, Math.Abs(cosDelta));
                posError = Math.Max(cosDelta, posError);
                negError = Math.Min(cosDelta, negError);

                Assert.IsTrue(absError < eps, $"Failed for cos({rad})");
            }
        }
    }
}
