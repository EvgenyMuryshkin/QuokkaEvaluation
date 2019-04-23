using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quokka.Simulator;
using System;
using System.Diagnostics;

namespace Indicators.Tests
{
    class IndicatorsSimulationIteration : IndicatorsControlsState, ISimulationIteration
    {
        public uint Iteration { get; set; }
    }

    [TestClass]
    public class Blinking
    {
        Simulation<IndicatorsSimulationIteration> Simulate(
            IndicatorsSimulationIteration state,
            int durationMs,
            Action<IndicatorsSimulationIteration> modifier = null)
        {
            var sim = new Simulation<IndicatorsSimulationIteration>();
            sim.Run(state, durationMs, (currentState, ms) =>
            {
                currentState.Iteration = (uint)ms;
                currentState.counterMs = (uint)ms;
                modifier?.Invoke(currentState);

                IndicatorsEngine.Process(currentState);
            });

            return sim;
        }

        [TestMethod]
        public void LeftAutoBlinking()
        {
            IndicatorsSimulationIteration state = new IndicatorsSimulationIteration()
            {
                flashSpeedMs = 500,
            };

            var sim = Simulate(state, 5001, (i) =>
            {
                // simulate short button press
                if (i.Iteration == 100)
                {
                    i.nextIndicator = eIndicatorType.Left;
                    i.nextIndicatorKeyEventTimeStamp = i.Iteration;
                }

                if (i.Iteration == 200)
                {
                    i.nextIndicator = eIndicatorType.None;
                    i.nextIndicatorKeyEventTimeStamp = i.Iteration;
                }
            });

            sim.AssertCompleteTransitions(
                t => t.isIndicatorActive,
                new Transition<bool>(99, false, true),
                new Transition<bool>(599, true, false),
                new Transition<bool>(1099, false, true),
                new Transition<bool>(1599, true, false),
                new Transition<bool>(2099, false, true),
                new Transition<bool>(2599, true, false)
                );
        }

        [TestMethod]
        public void LeftConstantBlinking()
        {
            IndicatorsSimulationIteration state = new IndicatorsSimulationIteration()
            {
                flashSpeedMs = 500,
                nextIndicator = eIndicatorType.Left
            };

            var sim = Simulate(state, 1001);

            sim.AssertCompleteTransitions(
                t => t.isIndicatorActive,
                new Transition<bool>(499, true, false),
                new Transition<bool>(999, false, true));
        }
    }
}
