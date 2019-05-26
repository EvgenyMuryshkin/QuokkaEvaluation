using Quokka.VCD;
using System;
using System.IO;

namespace Quokka.RTL
{
    public class RTLSynchronousSimulator<TModule>
        where TModule : IRTLCombinationalModule, new()
    {
        TModule _topLevel;

        public int MaxClockCycles { get; set; } = 100000;
        public int MaxStageIterations { get; set; } = 1000;
        VCDBuilder _vcdBuilder;

        public TModule TopLevel => _topLevel;
        public Action<TModule> OnPostStage { get; set; }
        public Func<TModule, bool> IsRunning { get; set; }

        public RTLSynchronousSimulator()
        {
            _topLevel = new TModule();
            _topLevel.Setup();
        }

        internal void RecursiveCreateTargetDirectory(string directory)
        {
            if (Directory.Exists(directory))
                return;

            RecursiveCreateTargetDirectory(Path.GetDirectoryName(directory));

            Directory.CreateDirectory(directory);
        }

        public void Trace(string outputFileName)
        {
            RecursiveCreateTargetDirectory(Path.GetDirectoryName(outputFileName));

            _vcdBuilder = new VCDBuilder(outputFileName);
        }

        public void Run()
        {
            VCDSignalsSnapshot topLevelSnapshot = new VCDSignalsSnapshot("TOP");
            var controlScope = topLevelSnapshot.Scope("Control");
            var clockSignal = controlScope.Add(new VCDVariable("Clock", true, 1));

            _topLevel.PopulateSnapshot(topLevelSnapshot);

            _vcdBuilder?.Init(topLevelSnapshot);

            var clock = 0;
            var stageIteration = 0;

            while (clock < MaxClockCycles && (IsRunning?.Invoke(_topLevel) ?? true))
            {
                var currentTime = clock * 2 * MaxStageIterations;
                clockSignal.Value = true;

                stageIteration = 0;
                do
                {
                    currentTime++;

                    var modified = _topLevel.Stage(stageIteration);

                    _topLevel.PopulateSnapshot(topLevelSnapshot);
                    _vcdBuilder?.Snapshot(currentTime, topLevelSnapshot);

                    // no modules were modified during stage iteration, all converged
                    if (!modified)
                        break;
                }
                while (++stageIteration < MaxStageIterations);

                if (stageIteration >= MaxStageIterations)
                    throw new MaxStageIterationReachedException();

                OnPostStage?.Invoke(_topLevel);

                currentTime = clock * 2 * MaxStageIterations + MaxStageIterations;

                clockSignal.Value = false;
                _topLevel.PopulateSnapshot(topLevelSnapshot);
                _vcdBuilder?.Snapshot(currentTime, topLevelSnapshot);

                _topLevel.Commit();
                clock++;
            }
        }
    }
}
