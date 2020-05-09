Setup Dev Environment

* Install .NET Core from https://dotnet.microsoft.com/ for your OS
* Clone QuokkaEvaluation repository
* Install Visual Studio or Visual Studio Code
* Open QuokkaEvaluation.RTL.sln in VS of QuokkaEvaluation in VSCode.

RTL folder contains examples and test cases for RTL module demos.
RTL.Modules - modules source code
RTL.Tests - test cases
RTL.HDL - generated HDL, Quartus projects, memory block templates

Run RTL/RTL.Modules/Verilog.cmd or RTL/RTL.Modules/VHDL.cmd

output should look like next:
```
Transforming C:\code\QuokkaEvaluation\RTL\RTL.Modules
Templates: C:\code\QuokkaEvaluation\RTL\RTL.HDL\templates
Vendor: intel
Device: generic
HDL: Verilog
Transformation completed: BitArrayModule
Transformation completed: CompositionModule
Transformation completed: CounterModule
Transformation completed: EmitterModule
Transformation completed: FullAdderModule
Transformation completed: AndGateModule
Transformation completed: NotGateModule
Transformation completed: OrGateModule
Transformation completed: XorGateModule
Transformation completed: CombinationalROMModule
Transformation completed: LogicRAMModule
Transformation completed: SDP_RF_RAMModule
Transformation completed: SDP_WF_RAMModule
Transformation completed: SynchronousROMModule
Transformation completed: ReceiverModule
Transformation completed: TransmitterModule
=== Directory transformation completed
```

Congratulations! 
All configured and ready to work.

