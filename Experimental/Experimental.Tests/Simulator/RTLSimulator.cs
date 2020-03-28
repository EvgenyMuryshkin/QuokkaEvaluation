using Quokka.VCD;
using System;
using System.IO;

namespace Quokka.RTL
{
    public class RTLSimulator<TModule>
        where TModule : IRTLCombinationalModule, new()
    {
        TModule _topLevel;

        VCDBuilder _vcdBuilder;
        VCDSignalsSnapshot _topLevelSnapshot;
        RTLSimulatorContext _simulatorContext;

        protected RTLSimulatorCallback<TModule> CallbackData 
            => new RTLSimulatorCallback<TModule>()
            {
                TopLevel = _topLevel,
                Clock = _simulatorContext.Clock,
                StageIteration = _simulatorContext.Iteration
            };

        public TModule TopLevel => _topLevel;
        public Action<TModule> OnPostStage { get; set; }
        public Func<RTLSimulatorCallback<TModule>, bool> IsRunning { get; set; }

        public RTLSimulator()
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
            Console.WriteLine($"Tracing to: {outputFileName}");
            RecursiveCreateTargetDirectory(Path.GetDirectoryName(outputFileName));

            _vcdBuilder = new VCDBuilder(outputFileName);
            _topLevelSnapshot = new VCDSignalsSnapshot("TOP");
        }

        void VCDInit()
        {
            if (_vcdBuilder == null)
                return;

            _topLevel.PopulateSnapshot(_topLevelSnapshot);
            _vcdBuilder.Init(_topLevelSnapshot);
        }

        void VCDSnapshot()
        {
            if (_vcdBuilder == null)
                return;

            _topLevel.PopulateSnapshot(_topLevelSnapshot);
            _vcdBuilder.Snapshot(_simulatorContext.CurrentTime, _topLevelSnapshot);

        }

        public void Run()
        {
            _simulatorContext = new RTLSimulatorContext();

            var controlScope = _topLevelSnapshot?.Scope("Control");
            var clockSignal = controlScope?.Add(new VCDVariable("Clock", true, 1));

            VCDInit();

            while (_simulatorContext.Clock < _simulatorContext.MaxClockCycles)
            {
                if (!(IsRunning?.Invoke(CallbackData) ?? true))
                    break;

                _simulatorContext.CurrentTime = _simulatorContext.Clock * 2 * _simulatorContext.MaxStageIterations;
                clockSignal?.SetValue(true);

                _simulatorContext.Iteration = 0;
                do
                {
                    _simulatorContext.CurrentTime++;

                    var modified = _topLevel.Stage(_simulatorContext.Iteration);

                    VCDSnapshot();

                    // no modules were modified during stage iteration, all converged
                    if (!modified)
                        break;
                }
                while (++_simulatorContext.Iteration < _simulatorContext.MaxStageIterations);

                if (_simulatorContext.Iteration >= _simulatorContext.MaxStageIterations)
                    throw new MaxStageIterationReachedException();

                OnPostStage?.Invoke(_topLevel);

                _simulatorContext.CurrentTime = _simulatorContext.Clock * 2 * _simulatorContext.MaxStageIterations + _simulatorContext.MaxStageIterations;

                clockSignal?.SetValue(false);
                VCDSnapshot();

                _topLevel.Commit();
                _simulatorContext.Clock++;
            }
        }
    }
}
