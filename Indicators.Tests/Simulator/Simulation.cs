using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Quokka.Simulator
{
    public class Simulation<T> where T : ISimulationIteration
    {
        public List<T> Snapshots = new List<T>();

        T Copy(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public void Run(
            T initialState, 
            int iterations,
            Action<T, int> onIteration)
        {
            var lastState = Copy(initialState);

            var steps = Enumerable.Range(0, iterations);
            foreach (var step in steps)
            {
                onIteration(lastState, step);
                Snapshots.Add(lastState);
                lastState = Copy(lastState);
            }
        }

        public void AssertCompleteTransitions<TValue>(
            Func<T, TValue> property,
            params Transition<TValue>[] transitions)
        {
            var values = Snapshots.Select(i => new
            {
                Iteration = i.Iteration,
                Value = property(i)
            });

            var pairs = values.Take(Snapshots.Count - 1)
                .Zip(values.Skip(1), (l, r) => new { Prev = l, Next = r });

            var changes = pairs
                .Where(p => !p.Prev.Value.Equals(p.Next.Value))
                .ToList();

            Assert.AreEqual(transitions.Count(), changes.Count, "number of transitions do no match");

            for(var idx = 0; idx < changes.Count; idx++)
            {
                var expected = transitions[idx];
                var actual = changes[idx];

                Assert.AreEqual(expected.Iteration, actual.Prev.Iteration, $"Transition iteration does not match");
                Assert.AreEqual(expected.From, actual.Prev.Value, $"From value does not match for iteration {expected.Iteration}");
                Assert.AreEqual(expected.To, actual.Next.Value, $"To value does not match for iteration {expected.Iteration}");
            }
        }
    }
}
