# QuokkaEvaluation

This repository contains 
* Basic CLI for running C# => VHDL or Verilog transformation
* Test examples for toolkit functionality
* Working implementation of Keccak1600 algorythm
* SnakeGame for 8x8 LED matrix

## Brief
Please read brief statement about the product here https://github.com/EvgenyMuryshkin/QuokkaEvaluation/raw/master/Docs/Quokka%20Whitepaper.pdf

## Setup 
Toolkit runs on all OS, where .NET Core 2.0 or higher is installed.

Please refer to framework installation guide for Linux https://www.microsoft.com/net/download/linux-package-manager/centos/sdk-current

* Get code from github repository git clone https://github.com/EvgenyMuryshkin/QuokkaEvaluation.git
* Open QuokkaCLI location in command line and run "dotnet run -s ..\SnakeGame"
* Run CLI in watch mode by providing -w argument "dotnet run -s ..\SnakeGame\vhdl.json -w"

Output should look something like next:
```
Using launch settings from C:\code\QuokkaEvaluation\QuokkaCLI\Properties\launchSettings.json...
Transforming C:\code\QuokkaEvaluation\SnakeGame
Configration validated
License loaded
4 controllers, 3 objects
Transformation completed
Produced 16 files, 101130282 bytes, Duration 14373 ms
Extracted 16 files to C:\code\QuokkaEvaluation\VHDL\generated\SnakeGame
Configuration [Quokka.qsf] was not found
Configuration [Test.qsf] was not found
=== Directory transformation completed
```

QuokkaEvaluation.sln has all the projects and QuokkaCLI in one place, QuokkaCLI can be run from directly from VS as well

Project is in active development, please contact me if you get any issues of have feedback.
