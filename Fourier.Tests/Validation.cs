using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Fourier.Tests
{
    public class Validation
    {
        public static float Epsilon = 1e-5f;

        public static void AssertFloatEqual(float f1, float f2)
        {
            if (Math.Abs(f1 - f2) > Epsilon)
            {
                Assert.Fail("Values are different");
            }
        }

        public static void AssertSpectres(
            ComplexFloat[] expected, 
            ComplexFloat[] actual,
            bool assertRe,
            bool assertIm)
        {
            for (int i = 0; i < expected.Length; i++)
            {
                var e = expected[i];
                var a = actual[i];

                if (assertRe)
                    AssertFloatEqual(e.Re, a.Re);

                if (assertIm)
                    AssertFloatEqual(e.Im, a.Im);
            }
        }

        /*
        public static void CompareSpectres(int bits, ComplexFloat[] data, Direction direction)
        {
            var recursiveSequenceCopy = data.ToArray();
            var recursiveFFTSequence = FFTSequencer.RecursiveFFTSequence(bits, direction);
            QuokkaFourier.SequentialFFT(bits, recursiveFFTSequence, recursiveSequenceCopy, direction);

            var loopsSequenceCopy = data.ToArray();
            var loopsFFTSequence = FFTSequencer.LoopsFFTSequence(bits);
            QuokkaFourier.SequentialFFT(bits, loopsFFTSequence, loopsSequenceCopy, direction);

            var dftCopy = data.ToArray();
            var dftOperations = QuokkaFourier.DFT(bits, dftCopy, direction);

            var fftCopy = data.ToArray();
            QuokkaFourier.RecursiveFFT(bits, fftCopy, direction);

            var iterativeCopy = data.ToArray();
            var loopsOperations = QuokkaFourier.IterativeFFT(bits, iterativeCopy, direction);

            AssertSpectres(dftCopy, fftCopy);
            AssertSpectres(dftCopy, recursiveSequenceCopy);
            AssertSpectres(dftCopy, loopsSequenceCopy);
            AssertSpectres(dftCopy, iterativeCopy);
        }
        */
        /*
        public static void SequenceCheck(int bits, Direction direction)
        {
            int n = FFTTools.ArrayLength(bits);
            float nFloat = FFTTools.ToFloat(n);
            float[] cosMap = FFTTools.Cos(n, direction);

            var pairs = FFTSequencer.RecursiveFFTSequence(bits, direction);
            // ensure target indexes are same on all iteration
            var sequentialResultCheck = pairs.GroupBy(g => g.Level)
                .Select(g =>
                {
                    var items = g.ToList();
                    return Enumerable
                        .Range(0, n / 2)
                        .Where(idx => items[idx].RPlus != idx || items[idx].RMinus != idx + n / 2)
                        .ToList();
                });

            if (sequentialResultCheck.Any(r => r.Any()))
            {
                throw new Exception("Sequential result indexes rule failed");
            }

            var sequentalGroups = pairs.GroupBy(g => g.Level)
                .Select(g =>
                {
                    var groups = new List<List<SequnceRec>>();
                    var last = new List<SequnceRec>();

                    foreach (var rec in g)
                    {
                        if (last.Count == 0)
                        {
                            last.Add(rec);
                        }
                        else
                        {
                            if (last.Last().E == rec.E - 1)
                            {
                                last.Add(rec);
                            }
                            else
                            {
                                groups.Add(last);
                                last = new List<SequnceRec>() { rec };
                            }
                        }
                    }

                    if (last != null)
                        groups.Add(last);

                    return new
                    {
                        Level = g.Key,
                        Groups = groups
                    };
                });

            var groupsCheck = sequentalGroups.ToList();

            // make sure all groups on each level have the same size
            var misaligned = groupsCheck.Where(g =>
            {
                return !g.Groups.All(gg => gg.Count == g.Groups.First().Count);
            });

            if (misaligned.Any())
            {
                throw new Exception("Aligned groupds rule failed");
            }

            var loopsFFTSequence = FFTSequencer.LoopsFFTSequence(bits);
            CompareSequences(pairs, loopsFFTSequence);
        }

        public static void CompareSequences(List<SequnceRec> baseList, List<SequnceRec> fastList)
        {
            if (baseList.Count != fastList.Count)
            {
                throw new Exception($"Count does not match");
            }

            var mismatch = baseList.Zip(fastList, (b, f) =>
            {
                if (b.Level == f.Level &&
                    b.E == f.E &&
                    b.O == f.O &&
                    b.CosIdx == f.CosIdx &&
                    b.RPlus == f.RPlus &&
                    b.RMinus == f.RMinus
                )
                {
                    return null;
                }

                return new
                {
                    Base = b,
                    Fast = f
                };
            })
            .Where(f => f != null)
            .ToList();

            if (mismatch.Any())
            {
                throw new Exception();
            }
        }
        */
    }
}
