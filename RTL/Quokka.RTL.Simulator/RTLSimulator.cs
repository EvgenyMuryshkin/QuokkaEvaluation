using Quokka.VCD;
using System;
using System.IO;

namespace Quokka.RTL.Simulator
{
    public class RTLInstanceSimulator<TModule>
        where TModule : IRTLCombinationalModule
    {
        protected TModule _topLevel;

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
        public Action<TModule> OnPostCommit { get; set; }
        public Func<RTLSimulatorCallback<TModule>, bool> IsRunning { get; set; }

        protected RTLInstanceSimulator()
        {

        }

        public RTLInstanceSimulator(TModule topLevel)
        {
            Initialize(topLevel);
        }

        protected void Initialize(TModule topLevel)
        {
            _topLevel = topLevel;
            _topLevel.Setup();
            _simulatorContext = new RTLSimulatorContext();
        }

        internal void RecursiveCreateTargetDirectory(string directory)
        {
            if (Directory.Exists(directory))
                return;

            RecursiveCreateTargetDirectory(Path.GetDirectoryName(directory));

            Directory.CreateDirectory(directory);
        }

        public void TraceToVCD(string outputFileName)
        {
            Console.WriteLine($"Tracing to: {outputFileName}");
            RecursiveCreateTargetDirectory(Path.GetDirectoryName(outputFileName));

            _vcdBuilder = new VCDBuilder(outputFileName);
            _topLevelSnapshot = new VCDSignalsSnapshot("TOP");
            _simulatorContext.ControlScope = _topLevelSnapshot.Scope("Control");
            _simulatorContext.ClockSignal = _simulatorContext.ControlScope.Add(new VCDVariable("Clock", true, 1));

            _topLevel.PopulateSnapshot(_topLevelSnapshot);
            _vcdBuilder.Init(_topLevelSnapshot);
        }

        protected virtual void TraceSignals()
        {
            if (_vcdBuilder == null)
                return;

            _topLevel.PopulateSnapshot(_topLevelSnapshot);
            _vcdBuilder.Snapshot(_simulatorContext.CurrentTime, _topLevelSnapshot);
        }

        public void ClockCycle()
        {
            _simulatorContext.CurrentTime = _simulatorContext.Clock * 2 * _simulatorContext.MaxStageIterations;
            _simulatorContext.ClockSignal?.SetValue(true);

            _simulatorContext.Iteration = 0;
            do
            {
                _simulatorContext.CurrentTime++;

                var modified = _topLevel.Stage(_simulatorContext.Iteration);

                TraceSignals();

                // no modules were modified during stage iteration, all converged
                if (!modified)
                    break;
            }
            while (++_simulatorContext.Iteration < _simulatorContext.MaxStageIterations);

            if (_simulatorContext.Iteration >= _simulatorContext.MaxStageIterations)
                throw new MaxStageIterationReachedException();

            OnPostStage?.Invoke(_topLevel);

            _simulatorContext.CurrentTime = _simulatorContext.Clock * 2 * _simulatorContext.MaxStageIterations + _simulatorContext.MaxStageIterations;
            _simulatorContext.ClockSignal?.SetValue(false);
            TraceSignals();

            _topLevel.Commit();
            OnPostCommit?.Invoke(_topLevel);

            _simulatorContext.Clock++;
        }

        public void Run()
        {
            while (_simulatorContext.Clock < _simulatorContext.MaxClockCycles)
            {
                if (!(IsRunning?.Invoke(CallbackData) ?? true))
                    break;

                ClockCycle();
            }
        }
    }

    public class RTLInstanceSimulator<TModule, TInputs> : RTLInstanceSimulator<TModule>
        where TModule : IRTLCombinationalModule<TInputs>
        where TInputs : new()
    {
        public RTLInstanceSimulator(TModule topLevel) : base(topLevel)
        {

        }

        public virtual void ClockCycle(TInputs inputs)
        {
            _topLevel.Schedule(() => inputs);
            ClockCycle();
        }
    }

    public class RTLSimulator<TModule> : RTLInstanceSimulator<TModule>
        where TModule : IRTLCombinationalModule, new()
    {
        public RTLSimulator() : base(new TModule())
        {

        }
    }
     
    public class RTLSimulator<TModule, TInputs> : RTLInstanceSimulator<TModule, TInputs>
        where TModule : IRTLCombinationalModule<TInputs>, new()
        where TInputs : new()
    {
        public RTLSimulator() : base(new TModule())
        {

        }
    }
}
