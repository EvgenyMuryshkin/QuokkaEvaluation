`default_nettype none
// PLEASE READ THIS, IT MAY SAVE YOU SOME TIME AND MONEY, THANK YOU!
// * This file was generated by Quokka FPGA Toolkit.
// * Generated code is your property, do whatever you want with it
// * Place custom code between [BEGIN USER ***] and [END USER ***].
// * CAUTION: All code outside of [USER] scope is subject to regeneration.
// * Bad things happen sometimes in developer's life,
//   it is recommended to use source control management software (e.g. git, bzr etc) to keep your custom code safe'n'sound.
// * Internal structure of code is subject to change.
//   You can use some of signals in custom code, but most likely they will not exist in future (e.g. will get shorter or gone completely)
// * Please send your feedback, comments, improvement ideas etc. to evmuryshkin@gmail.com
// * Visit https://github.com/EvgenyMuryshkin/QuokkaEvaluation to access latest version of playground
// 
// DISCLAIMER:
//   Code comes AS-IS, it is your responsibility to make sure it is working as expected
//   no responsibility will be taken for any loss or damage caused by use of Quokka toolkit.
// 
// System configuration name is BlinkerInf_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module BlinkerInf_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  Clock,
	input  Reset,
	output [31: 0] Counter
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  QuSoCModule_L108F13L143T14_QuSoCModule_L109F34T39_Expr = 1'b0;
wire  QuSoCModule_L108F13L143T14_QuSoCModule_L110F32T33_Expr = 1'b0;
wire  QuSoCModule_L108F13L143T14_QuSoCModule_L122F17L124T18_QuSoCModule_L123F31T32_Expr = 1'b0;
wire  QuSoCModule_L108F13L143T14_QuSoCModule_L126F17L128T18_QuSoCModule_L127F31T32_Expr = 1'b1;
wire  [1:0] QuSoCModule_L108F13L143T14_QuSoCModule_L130F17L132T18_QuSoCModule_L131F31T32_Expr = 2'b10;
wire  [1:0] QuSoCModule_L108F13L143T14_QuSoCModule_L134F17L136T18_QuSoCModule_L135F31T32_Expr = 2'b11;
wire  QuSoCModule_L108F13L143T14_QuSoCModule_L138F17L140T18_QuSoCModule_L139F33T38_Expr = 1'b0;
wire  QuSoCModule_L71F31T33_Expr = 1'b0;
wire  RISCVModule_Types_L11F30T35_Expr = 1'b0;
wire  QuSoCModule_L79F33T43_Expr = 1'b0;
wire  [31:0] QuSoCModule_L86F33T43_Expr = 32'b10000000000000000000000000000000;
wire  [31:0] QuSoCModule_L92F33T43_Expr = 32'b10000000000100000000000000000000;
wire  [31:0] QuSoCModule_L99F33T43_Expr = 32'b10000000001000000000000000000000;
wire  QuSoCModule_L155F9L174T10_QuSoCModule_L162F13L173T14_QuSoCModule_L168F17L172T18_QuSoCModule_L171F42T46_Expr = 1'b1;
reg  NextState_MemReady = 1'b0;
wire  [31:0] ModuleCommon_Address;
wire  [31:0] ModuleCommon_WriteValue;
wire  ModuleCommon_WE;
wire  ModuleCommon_RE;
wire  [7:0] BusCS_Item1;
wire  BusCS_Item2;
wire  [7:0] ModuleIndex;
wire  HasActiveModule;
wire  [31:0] internalModuleReadData;
wire  internalModuleIsReady;
wire  internalMemReady;
wire  [31:0] CPU_BaseAddress;
wire  [31:0] CPU_MemReadData;
wire  CPU_MemReady;
wire  CPU_ExtIRQ;
wire  CPU_IsHalted;
wire  [31:0] CPU_MemWriteData;
wire  [2:0] CPU_MemAccessMode;
wire  CPU_MemRead;
wire  CPU_MemWrite;
wire  [31:0] CPU_MemAddress;
wire  [31:0] InstructionsRAM_Common_Address;
wire  [31:0] InstructionsRAM_Common_WriteValue;
wire  InstructionsRAM_Common_WE;
wire  InstructionsRAM_Common_RE;
wire  [31:0] InstructionsRAM_DeviceAddress;
wire  [1:0] InstructionsRAM_MemAccessMode;
wire  [31:0] InstructionsRAM_ReadValue;
wire  InstructionsRAM_IsReady;
wire  InstructionsRAM_IsActive;
wire  [31:0] CounterRegister_Common_Address;
wire  [31:0] CounterRegister_Common_WriteValue;
wire  CounterRegister_Common_WE;
wire  CounterRegister_Common_RE;
wire  [31:0] CounterRegister_DeviceAddress;
wire  [31:0] CounterRegister_ReadValue;
wire  CounterRegister_IsReady;
wire  CounterRegister_IsActive;
wire  [31:0] BlockRAM_Common_Address;
wire  [31:0] BlockRAM_Common_WriteValue;
wire  BlockRAM_Common_WE;
wire  BlockRAM_Common_RE;
wire  [31:0] BlockRAM_DeviceAddress;
wire  [1:0] BlockRAM_MemAccessMode;
wire  [31:0] BlockRAM_ReadValue;
wire  BlockRAM_IsReady;
wire  BlockRAM_IsActive;
wire  [31:0] UARTSim_Common_Address;
wire  [31:0] UARTSim_Common_WriteValue;
wire  UARTSim_Common_WE;
wire  UARTSim_Common_RE;
wire  [31:0] UARTSim_DeviceAddress;
wire  UARTSim_IsActive;
wire  UARTSim_IsReady;
wire  [31:0] UARTSim_ReadValue;
reg  QuSoCModule_L108F13L143T14_hasActive = 1'b0;
reg  [7:0] QuSoCModule_L108F13L143T14_address = 8'b00000000;
wire  [1:0] QuSoCModule_L80F33T56_Index;
wire  [1:0] QuSoCModule_L93F33T55_Index;
wire  [31:0] CPUBaseAddressCPU_BaseAddressHardLink;
wire  [31:0] CPUMemReadDataCPU_MemReadDataHardLink;
wire  CPUMemReadyCPU_MemReadyHardLink;
wire  CPUExtIRQCPU_ExtIRQHardLink;
wire  CPUIsHaltedCPU_IsHaltedHardLink;
wire  [31:0] CPUMemWriteDataCPU_MemWriteDataHardLink;
wire  [2:0] CPUMemAccessModeCPU_MemAccessModeHardLink;
wire  CPUMemReadCPU_MemReadHardLink;
wire  CPUMemWriteCPU_MemWriteHardLink;
wire  [31:0] CPUMemAddressCPU_MemAddressHardLink;
wire  [31:0] InstructionsRAMCommon_AddressInstructionsRAM_Common_AddressHardLink;
wire  [31:0] InstructionsRAMCommon_WriteValueInstructionsRAM_Common_WriteValueHardLink;
wire  InstructionsRAMCommon_WEInstructionsRAM_Common_WEHardLink;
wire  InstructionsRAMCommon_REInstructionsRAM_Common_REHardLink;
wire  [31:0] InstructionsRAMDeviceAddressInstructionsRAM_DeviceAddressHardLink;
wire  [1:0] InstructionsRAMMemAccessModeInstructionsRAM_MemAccessModeHardLink;
wire  [31:0] InstructionsRAMReadValueInstructionsRAM_ReadValueHardLink;
wire  InstructionsRAMIsReadyInstructionsRAM_IsReadyHardLink;
wire  InstructionsRAMIsActiveInstructionsRAM_IsActiveHardLink;
wire  [31:0] CounterRegisterCommon_AddressCounterRegister_Common_AddressHardLink;
wire  [31:0] CounterRegisterCommon_WriteValueCounterRegister_Common_WriteValueHardLink;
wire  CounterRegisterCommon_WECounterRegister_Common_WEHardLink;
wire  CounterRegisterCommon_RECounterRegister_Common_REHardLink;
wire  [31:0] CounterRegisterDeviceAddressCounterRegister_DeviceAddressHardLink;
wire  [31:0] CounterRegisterReadValueCounterRegister_ReadValueHardLink;
wire  CounterRegisterIsReadyCounterRegister_IsReadyHardLink;
wire  CounterRegisterIsActiveCounterRegister_IsActiveHardLink;
wire  [31:0] BlockRAMCommon_AddressBlockRAM_Common_AddressHardLink;
wire  [31:0] BlockRAMCommon_WriteValueBlockRAM_Common_WriteValueHardLink;
wire  BlockRAMCommon_WEBlockRAM_Common_WEHardLink;
wire  BlockRAMCommon_REBlockRAM_Common_REHardLink;
wire  [31:0] BlockRAMDeviceAddressBlockRAM_DeviceAddressHardLink;
wire  [1:0] BlockRAMMemAccessModeBlockRAM_MemAccessModeHardLink;
wire  [31:0] BlockRAMReadValueBlockRAM_ReadValueHardLink;
wire  BlockRAMIsReadyBlockRAM_IsReadyHardLink;
wire  BlockRAMIsActiveBlockRAM_IsActiveHardLink;
wire  [31:0] UARTSimCommon_AddressUARTSim_Common_AddressHardLink;
wire  [31:0] UARTSimCommon_WriteValueUARTSim_Common_WriteValueHardLink;
wire  UARTSimCommon_WEUARTSim_Common_WEHardLink;
wire  UARTSimCommon_REUARTSim_Common_REHardLink;
wire  [31:0] UARTSimDeviceAddressUARTSim_DeviceAddressHardLink;
wire  UARTSimIsActiveUARTSim_IsActiveHardLink;
wire  UARTSimIsReadyUARTSim_IsReadyHardLink;
wire  [31:0] UARTSimReadValueUARTSim_ReadValueHardLink;
reg  State_MemReady = 1'b0;
wire  State_MemReadyDefault = 1'b0;
reg  [31:0] QuSoCModule_L149F40T73_Mux = 32'b00000000000000000000000000000000;
reg  QuSoCModule_L150F39T70_Mux = 1'b0;
wire  [1:0] QuSoCModule_L149F40T73_MuxMultiplexerAddress;
wire  [31:0] QuSoCModule_L149F40T73_Mux1;
wire  [31:0] QuSoCModule_L149F40T73_Mux2;
wire  [31:0] QuSoCModule_L149F40T73_Mux3;
wire  [31:0] QuSoCModule_L149F40T73_Mux4;
wire  [1:0] QuSoCModule_L150F39T70_MuxMultiplexerAddress;
wire  QuSoCModule_L150F39T70_Mux1;
wire  QuSoCModule_L150F39T70_Mux2;
wire  QuSoCModule_L150F39T70_Mux3;
wire  QuSoCModule_L150F39T70_Mux4;
wire  BoardSignals_Clock;
wire  BoardSignals_Reset;
wire  BoardSignals_Running;
wire  BoardSignals_Starting;
wire  BoardSignals_Started;
reg  InternalReset = 1'b0;
work_Quokka_BoardSignalsProc BoardSignalsConnection(BoardSignals_Clock,BoardSignals_Reset,BoardSignals_Running,BoardSignals_Starting,BoardSignals_Started,Clock,Reset,InternalReset);
always @(posedge Clock)
begin
if ( Reset == 1 ) begin
State_MemReady <= State_MemReadyDefault;
end
else begin
State_MemReady <= NextState_MemReady;
end
end
BlinkerInf_TopLevel_QuSoCModule_CPU BlinkerInf_TopLevel_QuSoCModule_CPU
(
// [BEGIN USER MAP FOR CPU]
// [END USER MAP FOR CPU]
	.BoardSignals_Clock (BoardSignals_Clock),
	.BoardSignals_Reset (BoardSignals_Reset),
	.BoardSignals_Running (BoardSignals_Running),
	.BoardSignals_Starting (BoardSignals_Starting),
	.BoardSignals_Started (BoardSignals_Started),
	.BaseAddress (CPUBaseAddressCPU_BaseAddressHardLink),
	.MemReadData (CPUMemReadDataCPU_MemReadDataHardLink),
	.MemReady (CPUMemReadyCPU_MemReadyHardLink),
	.ExtIRQ (CPUExtIRQCPU_ExtIRQHardLink),
	.IsHalted (CPUIsHaltedCPU_IsHaltedHardLink),
	.MemWriteData (CPUMemWriteDataCPU_MemWriteDataHardLink),
	.MemAccessMode (CPUMemAccessModeCPU_MemAccessModeHardLink),
	.MemRead (CPUMemReadCPU_MemReadHardLink),
	.MemWrite (CPUMemWriteCPU_MemWriteHardLink),
	.MemAddress (CPUMemAddressCPU_MemAddressHardLink)

);
BlinkerInf_TopLevel_QuSoCModule_InstructionsRAM BlinkerInf_TopLevel_QuSoCModule_InstructionsRAM
(
// [BEGIN USER MAP FOR InstructionsRAM]
// [END USER MAP FOR InstructionsRAM]
	.BoardSignals_Clock (BoardSignals_Clock),
	.BoardSignals_Reset (BoardSignals_Reset),
	.BoardSignals_Running (BoardSignals_Running),
	.BoardSignals_Starting (BoardSignals_Starting),
	.BoardSignals_Started (BoardSignals_Started),
	.Common_Address (InstructionsRAMCommon_AddressInstructionsRAM_Common_AddressHardLink),
	.Common_WriteValue (InstructionsRAMCommon_WriteValueInstructionsRAM_Common_WriteValueHardLink),
	.Common_WE (InstructionsRAMCommon_WEInstructionsRAM_Common_WEHardLink),
	.Common_RE (InstructionsRAMCommon_REInstructionsRAM_Common_REHardLink),
	.DeviceAddress (InstructionsRAMDeviceAddressInstructionsRAM_DeviceAddressHardLink),
	.MemAccessMode (InstructionsRAMMemAccessModeInstructionsRAM_MemAccessModeHardLink),
	.ReadValue (InstructionsRAMReadValueInstructionsRAM_ReadValueHardLink),
	.IsReady (InstructionsRAMIsReadyInstructionsRAM_IsReadyHardLink),
	.IsActive (InstructionsRAMIsActiveInstructionsRAM_IsActiveHardLink)

);
BlinkerInf_TopLevel_QuSoCModule_CounterRegister BlinkerInf_TopLevel_QuSoCModule_CounterRegister
(
// [BEGIN USER MAP FOR CounterRegister]
// [END USER MAP FOR CounterRegister]
	.BoardSignals_Clock (BoardSignals_Clock),
	.BoardSignals_Reset (BoardSignals_Reset),
	.BoardSignals_Running (BoardSignals_Running),
	.BoardSignals_Starting (BoardSignals_Starting),
	.BoardSignals_Started (BoardSignals_Started),
	.Common_Address (CounterRegisterCommon_AddressCounterRegister_Common_AddressHardLink),
	.Common_WriteValue (CounterRegisterCommon_WriteValueCounterRegister_Common_WriteValueHardLink),
	.Common_WE (CounterRegisterCommon_WECounterRegister_Common_WEHardLink),
	.Common_RE (CounterRegisterCommon_RECounterRegister_Common_REHardLink),
	.DeviceAddress (CounterRegisterDeviceAddressCounterRegister_DeviceAddressHardLink),
	.ReadValue (CounterRegisterReadValueCounterRegister_ReadValueHardLink),
	.IsReady (CounterRegisterIsReadyCounterRegister_IsReadyHardLink),
	.IsActive (CounterRegisterIsActiveCounterRegister_IsActiveHardLink)

);
BlinkerInf_TopLevel_QuSoCModule_BlockRAM BlinkerInf_TopLevel_QuSoCModule_BlockRAM
(
// [BEGIN USER MAP FOR BlockRAM]
// [END USER MAP FOR BlockRAM]
	.BoardSignals_Clock (BoardSignals_Clock),
	.BoardSignals_Reset (BoardSignals_Reset),
	.BoardSignals_Running (BoardSignals_Running),
	.BoardSignals_Starting (BoardSignals_Starting),
	.BoardSignals_Started (BoardSignals_Started),
	.Common_Address (BlockRAMCommon_AddressBlockRAM_Common_AddressHardLink),
	.Common_WriteValue (BlockRAMCommon_WriteValueBlockRAM_Common_WriteValueHardLink),
	.Common_WE (BlockRAMCommon_WEBlockRAM_Common_WEHardLink),
	.Common_RE (BlockRAMCommon_REBlockRAM_Common_REHardLink),
	.DeviceAddress (BlockRAMDeviceAddressBlockRAM_DeviceAddressHardLink),
	.MemAccessMode (BlockRAMMemAccessModeBlockRAM_MemAccessModeHardLink),
	.ReadValue (BlockRAMReadValueBlockRAM_ReadValueHardLink),
	.IsReady (BlockRAMIsReadyBlockRAM_IsReadyHardLink),
	.IsActive (BlockRAMIsActiveBlockRAM_IsActiveHardLink)

);
BlinkerInf_TopLevel_QuSoCModule_UARTSim BlinkerInf_TopLevel_QuSoCModule_UARTSim
(
// [BEGIN USER MAP FOR UARTSim]
// [END USER MAP FOR UARTSim]
	.BoardSignals_Clock (BoardSignals_Clock),
	.BoardSignals_Reset (BoardSignals_Reset),
	.BoardSignals_Running (BoardSignals_Running),
	.BoardSignals_Starting (BoardSignals_Starting),
	.BoardSignals_Started (BoardSignals_Started),
	.Common_Address (UARTSimCommon_AddressUARTSim_Common_AddressHardLink),
	.Common_WriteValue (UARTSimCommon_WriteValueUARTSim_Common_WriteValueHardLink),
	.Common_WE (UARTSimCommon_WEUARTSim_Common_WEHardLink),
	.Common_RE (UARTSimCommon_REUARTSim_Common_REHardLink),
	.DeviceAddress (UARTSimDeviceAddressUARTSim_DeviceAddressHardLink),
	.IsActive (UARTSimIsActiveUARTSim_IsActiveHardLink),
	.IsReady (UARTSimIsReadyUARTSim_IsReadyHardLink),
	.ReadValue (UARTSimReadValueUARTSim_ReadValueHardLink)

);
always @*
begin
case (QuSoCModule_L149F40T73_MuxMultiplexerAddress)
    'b00:
QuSoCModule_L149F40T73_Mux = QuSoCModule_L149F40T73_Mux1;
    'b01:
QuSoCModule_L149F40T73_Mux = QuSoCModule_L149F40T73_Mux2;
    'b10:
QuSoCModule_L149F40T73_Mux = QuSoCModule_L149F40T73_Mux3;
    'b11:
QuSoCModule_L149F40T73_Mux = QuSoCModule_L149F40T73_Mux4;
  default:
QuSoCModule_L149F40T73_Mux = 'b00000000000000000000000000000000;
endcase

end
always @*
begin
case (QuSoCModule_L150F39T70_MuxMultiplexerAddress)
    'b00:
QuSoCModule_L150F39T70_Mux = QuSoCModule_L150F39T70_Mux1;
    'b01:
QuSoCModule_L150F39T70_Mux = QuSoCModule_L150F39T70_Mux2;
    'b10:
QuSoCModule_L150F39T70_Mux = QuSoCModule_L150F39T70_Mux3;
    'b11:
QuSoCModule_L150F39T70_Mux = QuSoCModule_L150F39T70_Mux4;
  default:
QuSoCModule_L150F39T70_Mux = 'b0;
endcase

end
always @*
begin
QuSoCModule_L108F13L143T14_hasActive = QuSoCModule_L108F13L143T14_QuSoCModule_L109F34T39_Expr;
QuSoCModule_L108F13L143T14_address = { {7{1'b0}}, QuSoCModule_L108F13L143T14_QuSoCModule_L110F32T33_Expr }/*expand*/;
if ( InstructionsRAM_IsActive == 1 ) begin
QuSoCModule_L108F13L143T14_address = { {7{1'b0}}, QuSoCModule_L108F13L143T14_QuSoCModule_L122F17L124T18_QuSoCModule_L123F31T32_Expr }/*expand*/;
end
else if ( CounterRegister_IsActive == 1 ) begin
QuSoCModule_L108F13L143T14_address = { {7{1'b0}}, QuSoCModule_L108F13L143T14_QuSoCModule_L126F17L128T18_QuSoCModule_L127F31T32_Expr }/*expand*/;
end
else if ( BlockRAM_IsActive == 1 ) begin
QuSoCModule_L108F13L143T14_address = { {6{1'b0}}, QuSoCModule_L108F13L143T14_QuSoCModule_L130F17L132T18_QuSoCModule_L131F31T32_Expr }/*expand*/;
end
else if ( UARTSim_IsActive == 1 ) begin
QuSoCModule_L108F13L143T14_address = { {6{1'b0}}, QuSoCModule_L108F13L143T14_QuSoCModule_L134F17L136T18_QuSoCModule_L135F31T32_Expr }/*expand*/;
end
else begin
QuSoCModule_L108F13L143T14_hasActive = QuSoCModule_L108F13L143T14_QuSoCModule_L138F17L140T18_QuSoCModule_L139F33T38_Expr;
end

end
always @*
begin
NextState_MemReady = State_MemReady;
NextState_MemReady = CPU_MemRead;
if ( CPU_MemWrite == 1 ) begin
if ( HasActiveModule == 1 ) begin
NextState_MemReady = internalModuleIsReady;
end
else begin
NextState_MemReady = QuSoCModule_L155F9L174T10_QuSoCModule_L162F13L173T14_QuSoCModule_L168F17L172T18_QuSoCModule_L171F42T46_Expr;
end
end

end
assign ModuleCommon_Address = CPU_MemAddress;
assign ModuleCommon_WriteValue = CPU_MemWriteData;
assign ModuleCommon_WE = CPU_MemWrite;
assign ModuleCommon_RE = CPU_MemRead;
assign BusCS_Item1 = QuSoCModule_L108F13L143T14_address;
assign BusCS_Item2 = QuSoCModule_L108F13L143T14_hasActive;
assign ModuleIndex = BusCS_Item1;
assign HasActiveModule = BusCS_Item2;
assign internalModuleReadData = QuSoCModule_L149F40T73_Mux;
assign internalModuleIsReady = QuSoCModule_L150F39T70_Mux;
assign internalMemReady = State_MemReady;
assign CPU_BaseAddress = { {31{1'b0}}, QuSoCModule_L71F31T33_Expr }/*expand*/;
assign CPU_MemReadData = internalModuleReadData;
assign CPU_MemReady = internalMemReady;
assign CPU_ExtIRQ = RISCVModule_Types_L11F30T35_Expr;
assign QuSoCModule_L80F33T56_Index = CPU_MemAccessMode[1:0];
assign InstructionsRAM_MemAccessMode = QuSoCModule_L80F33T56_Index;
assign InstructionsRAM_Common_Address = ModuleCommon_Address;
assign InstructionsRAM_Common_WriteValue = ModuleCommon_WriteValue;
assign InstructionsRAM_Common_WE = ModuleCommon_WE;
assign InstructionsRAM_Common_RE = ModuleCommon_RE;
assign InstructionsRAM_DeviceAddress = { {31{1'b0}}, QuSoCModule_L79F33T43_Expr }/*expand*/;
assign CounterRegister_Common_Address = ModuleCommon_Address;
assign CounterRegister_Common_WriteValue = ModuleCommon_WriteValue;
assign CounterRegister_Common_WE = ModuleCommon_WE;
assign CounterRegister_Common_RE = ModuleCommon_RE;
assign CounterRegister_DeviceAddress = QuSoCModule_L86F33T43_Expr;
assign QuSoCModule_L93F33T55_Index = CPU_MemAccessMode[1:0];
assign BlockRAM_MemAccessMode = QuSoCModule_L93F33T55_Index;
assign BlockRAM_Common_Address = ModuleCommon_Address;
assign BlockRAM_Common_WriteValue = ModuleCommon_WriteValue;
assign BlockRAM_Common_WE = ModuleCommon_WE;
assign BlockRAM_Common_RE = ModuleCommon_RE;
assign BlockRAM_DeviceAddress = QuSoCModule_L92F33T43_Expr;
assign UARTSim_Common_Address = ModuleCommon_Address;
assign UARTSim_Common_WriteValue = ModuleCommon_WriteValue;
assign UARTSim_Common_WE = ModuleCommon_WE;
assign UARTSim_Common_RE = ModuleCommon_RE;
assign UARTSim_DeviceAddress = QuSoCModule_L99F33T43_Expr;
assign Counter = CounterRegister_ReadValue;
assign CPUBaseAddressCPU_BaseAddressHardLink = CPU_BaseAddress;
assign CPUMemReadDataCPU_MemReadDataHardLink = CPU_MemReadData;
assign CPUMemReadyCPU_MemReadyHardLink = CPU_MemReady;
assign CPUExtIRQCPU_ExtIRQHardLink = CPU_ExtIRQ;
assign CPU_IsHalted = CPUIsHaltedCPU_IsHaltedHardLink;
assign CPU_MemWriteData = CPUMemWriteDataCPU_MemWriteDataHardLink;
assign CPU_MemAccessMode = CPUMemAccessModeCPU_MemAccessModeHardLink;
assign CPU_MemRead = CPUMemReadCPU_MemReadHardLink;
assign CPU_MemWrite = CPUMemWriteCPU_MemWriteHardLink;
assign CPU_MemAddress = CPUMemAddressCPU_MemAddressHardLink;
assign InstructionsRAMCommon_AddressInstructionsRAM_Common_AddressHardLink = InstructionsRAM_Common_Address;
assign InstructionsRAMCommon_WriteValueInstructionsRAM_Common_WriteValueHardLink = InstructionsRAM_Common_WriteValue;
assign InstructionsRAMCommon_WEInstructionsRAM_Common_WEHardLink = InstructionsRAM_Common_WE;
assign InstructionsRAMCommon_REInstructionsRAM_Common_REHardLink = InstructionsRAM_Common_RE;
assign InstructionsRAMDeviceAddressInstructionsRAM_DeviceAddressHardLink = InstructionsRAM_DeviceAddress;
assign InstructionsRAMMemAccessModeInstructionsRAM_MemAccessModeHardLink = InstructionsRAM_MemAccessMode;
assign InstructionsRAM_ReadValue = InstructionsRAMReadValueInstructionsRAM_ReadValueHardLink;
assign InstructionsRAM_IsReady = InstructionsRAMIsReadyInstructionsRAM_IsReadyHardLink;
assign InstructionsRAM_IsActive = InstructionsRAMIsActiveInstructionsRAM_IsActiveHardLink;
assign CounterRegisterCommon_AddressCounterRegister_Common_AddressHardLink = CounterRegister_Common_Address;
assign CounterRegisterCommon_WriteValueCounterRegister_Common_WriteValueHardLink = CounterRegister_Common_WriteValue;
assign CounterRegisterCommon_WECounterRegister_Common_WEHardLink = CounterRegister_Common_WE;
assign CounterRegisterCommon_RECounterRegister_Common_REHardLink = CounterRegister_Common_RE;
assign CounterRegisterDeviceAddressCounterRegister_DeviceAddressHardLink = CounterRegister_DeviceAddress;
assign CounterRegister_ReadValue = CounterRegisterReadValueCounterRegister_ReadValueHardLink;
assign CounterRegister_IsReady = CounterRegisterIsReadyCounterRegister_IsReadyHardLink;
assign CounterRegister_IsActive = CounterRegisterIsActiveCounterRegister_IsActiveHardLink;
assign BlockRAMCommon_AddressBlockRAM_Common_AddressHardLink = BlockRAM_Common_Address;
assign BlockRAMCommon_WriteValueBlockRAM_Common_WriteValueHardLink = BlockRAM_Common_WriteValue;
assign BlockRAMCommon_WEBlockRAM_Common_WEHardLink = BlockRAM_Common_WE;
assign BlockRAMCommon_REBlockRAM_Common_REHardLink = BlockRAM_Common_RE;
assign BlockRAMDeviceAddressBlockRAM_DeviceAddressHardLink = BlockRAM_DeviceAddress;
assign BlockRAMMemAccessModeBlockRAM_MemAccessModeHardLink = BlockRAM_MemAccessMode;
assign BlockRAM_ReadValue = BlockRAMReadValueBlockRAM_ReadValueHardLink;
assign BlockRAM_IsReady = BlockRAMIsReadyBlockRAM_IsReadyHardLink;
assign BlockRAM_IsActive = BlockRAMIsActiveBlockRAM_IsActiveHardLink;
assign UARTSimCommon_AddressUARTSim_Common_AddressHardLink = UARTSim_Common_Address;
assign UARTSimCommon_WriteValueUARTSim_Common_WriteValueHardLink = UARTSim_Common_WriteValue;
assign UARTSimCommon_WEUARTSim_Common_WEHardLink = UARTSim_Common_WE;
assign UARTSimCommon_REUARTSim_Common_REHardLink = UARTSim_Common_RE;
assign UARTSimDeviceAddressUARTSim_DeviceAddressHardLink = UARTSim_DeviceAddress;
assign UARTSim_IsActive = UARTSimIsActiveUARTSim_IsActiveHardLink;
assign UARTSim_IsReady = UARTSimIsReadyUARTSim_IsReadyHardLink;
assign UARTSim_ReadValue = UARTSimReadValueUARTSim_ReadValueHardLink;
assign QuSoCModule_L149F40T73_Mux1 = InstructionsRAM_ReadValue;
assign QuSoCModule_L149F40T73_Mux2 = CounterRegister_ReadValue;
assign QuSoCModule_L149F40T73_Mux3 = BlockRAM_ReadValue;
assign QuSoCModule_L149F40T73_Mux4 = UARTSim_ReadValue;
assign QuSoCModule_L149F40T73_MuxMultiplexerAddress = ModuleIndex[1:0]/*truncate*/;
assign QuSoCModule_L150F39T70_Mux1 = InstructionsRAM_IsReady;
assign QuSoCModule_L150F39T70_Mux2 = CounterRegister_IsReady;
assign QuSoCModule_L150F39T70_Mux3 = BlockRAM_IsReady;
assign QuSoCModule_L150F39T70_Mux4 = UARTSim_IsReady;
assign QuSoCModule_L150F39T70_MuxMultiplexerAddress = ModuleIndex[1:0]/*truncate*/;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
