## RTL Simulator

### RTLSimulator class

RTL Simulator is an experimental project. [Source code](https://github.com/EvgenyMuryshkin/QuokkaEvaluation/tree/master/RTL/Quokka.RTL.Simulator) is available as part of this repository for easy debugging and testing. 

**Functionality of simulator is under constant development, it most certainly will change**

#### Simulator instance

Create instance of RTL simulator with module type as argument.

```
    var sim = new RTLSimulator<CounterModule>();
```

#### Running conditions

On every cycle, simulator checks if it sould keep running module evaluation.

Any condition can be used e.g. number of clock cycles, state of module outputs, timeout, web response, user interaction etc.

```
    sim.IsRunning = (cb) => cb.Clock < 100;
```

#### Tracing

There is default support for tracing into VCD file. Source code for VCD trace can be found [here](https://github.com/EvgenyMuryshkin/Quokka.RTL/tree/master/Quokka.RTL/VCD)

```
    sim.TraceToVCD(PathTools.VCDOutputPath());
```

Support for custom tracing will be added as part of future work. 

**Hardcoded VCD tracer will be removed from RTL simulator and will be available as trace option**

#### Scheduling
Top level module is scheduled with instance of input parameters. This functions is called every time simulator needs a fresh state of input parameters.
```
    sim.TopLevel.Schedule(() => new CounterInputs() { Enabled = true });
```

#### Evaluation
Once simulator and top-level module are setup, simulation can run.
```
    sim.Run();
```

Internally, simulator will perform clock cycle simulation.

For every clock cycle, set of delta cycles will be evaluated. 

On every delta cycle, it will call scheduling function for each module in hierarhcy, evaluate outputs of each module. 

This process will repeat until all modules in hierarchy are converged, or max iterations is reached.

#### Asserting results
Once simulations is completed (or in running callback), state of the module can be tested for validity.

```
    Assert.AreEqual(0, sim.TopLevel.Value);
    sim.Run();
    Assert.AreEqual(100, sim.TopLevel.Value);
```

### CombinationalRTLSimulator class

This is a shortcut version of simulator, it wraps all setup, schedule and execution steps in simple one-liner for quick test of combinational modules.