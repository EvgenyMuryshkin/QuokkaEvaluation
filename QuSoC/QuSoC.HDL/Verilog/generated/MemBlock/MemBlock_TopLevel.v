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
// System configuration name is MemBlock_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module MemBlock_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  Clock,
	input  Reset,
	output [31: 0] Counter,
	output [7: 0] UARTWriteData
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  [2:1] QuSoCModule_L90F58T59_Expr = 2'b10;
wire  QuSoCModule_L97F13L123T14_QuSoCModule_L98F31T32_Expr = 1'b0;
wire  [2:1] QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F30T31_Expr = 2'b10;
wire  [2:1] QuSoCModule_L135F38T39_Expr = 2'b10;
wire  QuSoCModule_L135F44T45_Expr = 1'b0;
wire  QuSoCModule_L63F31T33_Expr = 1'b0;
wire  RISCVModule_Types_L11F30T35_Expr = 1'b0;
wire  [32:1] QuSoCModule_L71F33T43_Expr = 32'b10000000000000000000000000000000;
wire  [32:1] QuSoCModule_L77F33T43_Expr = 32'b10000000000100000000000000000000;
wire  QuSoCModule_L84F33T43_Expr = 1'b0;
wire  QuSoCModule_L138F9L182T10_QuSoCModule_L144F33T38_Expr = 1'b0;
wire  [2:1] QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F30T31_Expr = 2'b10;
wire  QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L169F48T49_Expr = 1'b0;
wire  [2:1] QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L170F48T49_Expr = 2'b10;
wire  QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L170F53T54_Expr = 1'b0;
wire  QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L171F53T57_Expr = 1'b1;
wire  QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L172F54T58_Expr = 1'b1;
wire  QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L177F50T54_Expr = 1'b1;
wire  QuSoCModule_L133F49T50_Expr = 1'b0;
reg  NextState_MemReady = 1'b0;
reg  NextState_UART_TX = 1'b0;
wire  [32:1] ModuleCommon_Address;
wire  [32:1] ModuleCommon_WriteValue;
wire  ModuleCommon_WE;
wire  ModuleCommon_RE;
wire  [32:1] internalMemAddress;
wire  [32:1] wordAddress;
wire  [32:1] uartReadData;
wire  [32:1] internalMemReadData;
wire  internalMemReady;
wire  [22:1] memSegment;
wire  [10:1] blockRamAddress;
wire  [2:1] uartAddress;
wire  UARTReady;
wire  [32:1] InstructionsRAM_Common_Address;
wire  [32:1] InstructionsRAM_Common_WriteValue;
wire  InstructionsRAM_Common_WE;
wire  InstructionsRAM_Common_RE;
wire  [32:1] InstructionsRAM_DeviceAddress;
wire  [2:1] InstructionsRAM_MemAccessMode;
wire  [32:1] InstructionsRAM_ReadValue;
wire  InstructionsRAM_IsReady;
wire  InstructionsRAM_IsActive;
wire  [32:1] CPU_BaseAddress;
wire  [32:1] CPU_MemReadData;
wire  CPU_MemReady;
wire  CPU_ExtIRQ;
wire  CPU_IsHalted;
wire  [32:1] CPU_MemWriteData;
wire  [3:1] CPU_MemAccessMode;
wire  CPU_MemRead;
wire  CPU_MemWrite;
wire  [32:1] CPU_MemAddress;
wire  [32:1] CounterRegister_Common_Address;
wire  [32:1] CounterRegister_Common_WriteValue;
wire  CounterRegister_Common_WE;
wire  CounterRegister_Common_RE;
wire  [32:1] CounterRegister_DeviceAddress;
wire  [32:1] CounterRegister_ReadValue;
wire  CounterRegister_IsReady;
wire  CounterRegister_IsActive;
wire  [32:1] BlockRAM_Common_Address;
wire  [32:1] BlockRAM_Common_WriteValue;
wire  BlockRAM_Common_WE;
wire  BlockRAM_Common_RE;
wire  [32:1] BlockRAM_DeviceAddress;
wire  [2:1] BlockRAM_MemAccessMode;
wire  [32:1] BlockRAM_ReadValue;
wire  BlockRAM_IsReady;
wire  BlockRAM_IsActive;
wire  [32:1] QuSoCModule_L89F43T74_Source;
wire  [8:1] QuSoCModule_L91F37T77_Source;
wire  [8:1] QuSoCModule_L91F53T76_Index;
wire  [32:1] QuSoCModule_L91F37T89_Resize;
reg  [32:1] QuSoCModule_L97F13L123T14_result = 32'b00000000000000000000000000000000;
wire  [32:1] QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F29T45_Cast;
wire  [22:1] QuSoCModule_L128F35T54_Index;
wire  [10:1] QuSoCModule_L129F40T57_Index;
wire  [2:1] QuSoCModule_L131F36T60_Index;
wire  [8:1] QuSoCModule_L135F27T40_Index;
wire  [2:1] QuSoCModule_L78F33T55_Index;
wire  [2:1] QuSoCModule_L85F33T56_Index;
wire  [32:1] QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F29T45_Cast;
wire  [8:1] QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L169F53T75_Cast;
reg  [8:1] State_UARTDefault = 8'b00000000;
wire  [8:1] QuSoCModule_L133F38T51_Index;
wire  [32:1] InstructionsRAMCommon_AddressInstructionsRAM_Common_AddressHardLink;
wire  [32:1] InstructionsRAMCommon_WriteValueInstructionsRAM_Common_WriteValueHardLink;
wire  InstructionsRAMCommon_WEInstructionsRAM_Common_WEHardLink;
wire  InstructionsRAMCommon_REInstructionsRAM_Common_REHardLink;
wire  [32:1] InstructionsRAMDeviceAddressInstructionsRAM_DeviceAddressHardLink;
wire  [2:1] InstructionsRAMMemAccessModeInstructionsRAM_MemAccessModeHardLink;
wire  [32:1] InstructionsRAMReadValueInstructionsRAM_ReadValueHardLink;
wire  InstructionsRAMIsReadyInstructionsRAM_IsReadyHardLink;
wire  InstructionsRAMIsActiveInstructionsRAM_IsActiveHardLink;
wire  [32:1] CPUBaseAddressCPU_BaseAddressHardLink;
wire  [32:1] CPUMemReadDataCPU_MemReadDataHardLink;
wire  CPUMemReadyCPU_MemReadyHardLink;
wire  CPUExtIRQCPU_ExtIRQHardLink;
wire  CPUIsHaltedCPU_IsHaltedHardLink;
wire  [32:1] CPUMemWriteDataCPU_MemWriteDataHardLink;
wire  [3:1] CPUMemAccessModeCPU_MemAccessModeHardLink;
wire  CPUMemReadCPU_MemReadHardLink;
wire  CPUMemWriteCPU_MemWriteHardLink;
wire  [32:1] CPUMemAddressCPU_MemAddressHardLink;
wire  [32:1] CounterRegisterCommon_AddressCounterRegister_Common_AddressHardLink;
wire  [32:1] CounterRegisterCommon_WriteValueCounterRegister_Common_WriteValueHardLink;
wire  CounterRegisterCommon_WECounterRegister_Common_WEHardLink;
wire  CounterRegisterCommon_RECounterRegister_Common_REHardLink;
wire  [32:1] CounterRegisterDeviceAddressCounterRegister_DeviceAddressHardLink;
wire  [32:1] CounterRegisterReadValueCounterRegister_ReadValueHardLink;
wire  CounterRegisterIsReadyCounterRegister_IsReadyHardLink;
wire  CounterRegisterIsActiveCounterRegister_IsActiveHardLink;
wire  [32:1] BlockRAMCommon_AddressBlockRAM_Common_AddressHardLink;
wire  [32:1] BlockRAMCommon_WriteValueBlockRAM_Common_WriteValueHardLink;
wire  BlockRAMCommon_WEBlockRAM_Common_WEHardLink;
wire  BlockRAMCommon_REBlockRAM_Common_REHardLink;
wire  [32:1] BlockRAMDeviceAddressBlockRAM_DeviceAddressHardLink;
wire  [2:1] BlockRAMMemAccessModeBlockRAM_MemAccessModeHardLink;
wire  [32:1] BlockRAMReadValueBlockRAM_ReadValueHardLink;
wire  BlockRAMIsReadyBlockRAM_IsReadyHardLink;
wire  BlockRAMIsActiveBlockRAM_IsActiveHardLink;
reg  State_MemReady = 1'b0;
wire  State_MemReadyDefault = 1'b0;
reg  State_UART_TX = 1'b0;
wire  State_UART_TXDefault = 1'b0;
wire  [32:1] QuSoCModule_L90F36T59_Expr;
wire  [32:1] QuSoCModule_L90F36T59_Expr_1;
wire  QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_Case;
wire signed  [33:1] QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_CaseLhs;
wire signed  [33:1] QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_CaseRhs;
wire  QuSoCModule_L135F27T45_Expr;
wire signed  [9:1] QuSoCModule_L135F27T45_ExprLhs;
wire signed  [9:1] QuSoCModule_L135F27T45_ExprRhs;
wire  QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_Case;
wire signed  [33:1] QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_CaseLhs;
wire signed  [33:1] QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_CaseRhs;
integer State_UART_Iterator;
reg [8:1] State_UART [0 : 3];
initial
begin
	for (State_UART_Iterator = 0; State_UART_Iterator < 4; State_UART_Iterator = State_UART_Iterator + 1)
		State_UART[State_UART_Iterator] = 0;
end
integer NextState_UART_Iterator;
reg [8:1] NextState_UART [0 : 3];
initial
begin
	for (NextState_UART_Iterator = 0; NextState_UART_Iterator < 4; NextState_UART_Iterator = NextState_UART_Iterator + 1)
		NextState_UART[NextState_UART_Iterator] = 0;
end
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
State_UART_TX <= State_UART_TXDefault;
end
else begin
State_MemReady <= NextState_MemReady;
State_UART_TX <= NextState_UART_TX;
end
end
always @(posedge Clock)
begin
if ( Reset == 1 ) begin
for (State_UART_Iterator = 0; State_UART_Iterator < 4; State_UART_Iterator = State_UART_Iterator + 1)
begin
State_UART[State_UART_Iterator] <= State_UARTDefault;
end
end
else begin
for (State_UART_Iterator = 0; State_UART_Iterator < 4; State_UART_Iterator = State_UART_Iterator + 1)
begin
State_UART[State_UART_Iterator] <= NextState_UART[State_UART_Iterator];
end
end
end
assign QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_Case = QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_CaseLhs == QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_CaseRhs ? 1'b1 : 1'b0;
assign QuSoCModule_L135F27T45_Expr = QuSoCModule_L135F27T45_ExprLhs != QuSoCModule_L135F27T45_ExprRhs ? 1'b1 : 1'b0;
assign QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_Case = QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_CaseLhs == QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_CaseRhs ? 1'b1 : 1'b0;
// Output: QuSoCModule_L90F36T59_Expr, Width: 32, ShiftBy: 2, Sources: 1
assign QuSoCModule_L90F36T59_Expr[1] = QuSoCModule_L90F36T59_Expr_1[3];
assign QuSoCModule_L90F36T59_Expr[2] = QuSoCModule_L90F36T59_Expr_1[4];
assign QuSoCModule_L90F36T59_Expr[3] = QuSoCModule_L90F36T59_Expr_1[5];
assign QuSoCModule_L90F36T59_Expr[4] = QuSoCModule_L90F36T59_Expr_1[6];
assign QuSoCModule_L90F36T59_Expr[5] = QuSoCModule_L90F36T59_Expr_1[7];
assign QuSoCModule_L90F36T59_Expr[6] = QuSoCModule_L90F36T59_Expr_1[8];
assign QuSoCModule_L90F36T59_Expr[7] = QuSoCModule_L90F36T59_Expr_1[9];
assign QuSoCModule_L90F36T59_Expr[8] = QuSoCModule_L90F36T59_Expr_1[10];
assign QuSoCModule_L90F36T59_Expr[9] = QuSoCModule_L90F36T59_Expr_1[11];
assign QuSoCModule_L90F36T59_Expr[10] = QuSoCModule_L90F36T59_Expr_1[12];
assign QuSoCModule_L90F36T59_Expr[11] = QuSoCModule_L90F36T59_Expr_1[13];
assign QuSoCModule_L90F36T59_Expr[12] = QuSoCModule_L90F36T59_Expr_1[14];
assign QuSoCModule_L90F36T59_Expr[13] = QuSoCModule_L90F36T59_Expr_1[15];
assign QuSoCModule_L90F36T59_Expr[14] = QuSoCModule_L90F36T59_Expr_1[16];
assign QuSoCModule_L90F36T59_Expr[15] = QuSoCModule_L90F36T59_Expr_1[17];
assign QuSoCModule_L90F36T59_Expr[16] = QuSoCModule_L90F36T59_Expr_1[18];
assign QuSoCModule_L90F36T59_Expr[17] = QuSoCModule_L90F36T59_Expr_1[19];
assign QuSoCModule_L90F36T59_Expr[18] = QuSoCModule_L90F36T59_Expr_1[20];
assign QuSoCModule_L90F36T59_Expr[19] = QuSoCModule_L90F36T59_Expr_1[21];
assign QuSoCModule_L90F36T59_Expr[20] = QuSoCModule_L90F36T59_Expr_1[22];
assign QuSoCModule_L90F36T59_Expr[21] = QuSoCModule_L90F36T59_Expr_1[23];
assign QuSoCModule_L90F36T59_Expr[22] = QuSoCModule_L90F36T59_Expr_1[24];
assign QuSoCModule_L90F36T59_Expr[23] = QuSoCModule_L90F36T59_Expr_1[25];
assign QuSoCModule_L90F36T59_Expr[24] = QuSoCModule_L90F36T59_Expr_1[26];
assign QuSoCModule_L90F36T59_Expr[25] = QuSoCModule_L90F36T59_Expr_1[27];
assign QuSoCModule_L90F36T59_Expr[26] = QuSoCModule_L90F36T59_Expr_1[28];
assign QuSoCModule_L90F36T59_Expr[27] = QuSoCModule_L90F36T59_Expr_1[29];
assign QuSoCModule_L90F36T59_Expr[28] = QuSoCModule_L90F36T59_Expr_1[30];
assign QuSoCModule_L90F36T59_Expr[29] = QuSoCModule_L90F36T59_Expr_1[31];
assign QuSoCModule_L90F36T59_Expr[30] = QuSoCModule_L90F36T59_Expr_1[32];
assign QuSoCModule_L90F36T59_Expr[31] = 0;
assign QuSoCModule_L90F36T59_Expr[32] = 0;
MemBlock_TopLevel_QuSoCModule_InstructionsRAM MemBlock_TopLevel_QuSoCModule_InstructionsRAM
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
MemBlock_TopLevel_QuSoCModule_CPU MemBlock_TopLevel_QuSoCModule_CPU
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
MemBlock_TopLevel_QuSoCModule_CounterRegister MemBlock_TopLevel_QuSoCModule_CounterRegister
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
MemBlock_TopLevel_QuSoCModule_BlockRAM MemBlock_TopLevel_QuSoCModule_BlockRAM
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
always @*
begin
QuSoCModule_L97F13L123T14_result = { {31{1'b0}}, QuSoCModule_L97F13L123T14_QuSoCModule_L98F31T32_Expr }/*expand*/;
if ( CounterRegister_IsActive == 1 ) begin
QuSoCModule_L97F13L123T14_result = CounterRegister_ReadValue;
end
else if ( BlockRAM_IsActive == 1 ) begin
QuSoCModule_L97F13L123T14_result = BlockRAM_ReadValue;
end
else if ( InstructionsRAM_IsActive == 1 ) begin
QuSoCModule_L97F13L123T14_result = InstructionsRAM_ReadValue;
end
else begin
if ( QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_Case == 1 ) begin
QuSoCModule_L97F13L123T14_result = uartReadData;
end
end

end
always @*
begin
for (NextState_UART_Iterator = 0; NextState_UART_Iterator < 4; NextState_UART_Iterator = NextState_UART_Iterator + 1)
begin
NextState_UART[NextState_UART_Iterator] = State_UART[NextState_UART_Iterator];
end
NextState_MemReady = State_MemReady;
NextState_UART_TX = State_UART_TX;
NextState_MemReady = CPU_MemRead;
NextState_UART_TX = QuSoCModule_L138F9L182T10_QuSoCModule_L144F33T38_Expr;
if ( CPU_MemWrite == 1 ) begin
if ( CounterRegister_IsActive == 1 ) begin
NextState_MemReady = CounterRegister_IsReady;
end
else if ( BlockRAM_IsActive == 1 ) begin
NextState_MemReady = BlockRAM_IsReady;
end
else if ( InstructionsRAM_IsActive == 1 ) begin
NextState_MemReady = InstructionsRAM_IsReady;
end
else begin
if ( QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_Case == 1 ) begin
if ( UARTReady == 1 ) begin
NextState_UART[QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L169F48T49_Expr] = QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L169F53T75_Cast;
NextState_UART[QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L170F48T49_Expr] = { {7{1'b0}}, QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L170F53T54_Expr }/*expand*/;
NextState_UART_TX = QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L171F53T57_Expr;
NextState_MemReady = QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L172F54T58_Expr;
end
end
else begin
NextState_MemReady = QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L177F50T54_Expr;
end
end
end

end
assign QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_CaseLhs = { {1{1'b0}}, QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F29T45_Cast }/*expand*/;
assign QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F25L118T35_CaseRhs = { {31{1'b0}}, QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F21L119T22_QuSoCModule_L116F30T31_Expr }/*expand*/;
assign QuSoCModule_L135F27T45_ExprLhs = { {1{1'b0}}, QuSoCModule_L135F27T40_Index }/*expand*/;
assign QuSoCModule_L135F27T45_ExprRhs = { {8{1'b0}}, QuSoCModule_L135F44T45_Expr }/*expand*/;
assign QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_CaseLhs = { {1{1'b0}}, QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F29T45_Cast }/*expand*/;
assign QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F25L174T35_CaseRhs = { {31{1'b0}}, QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L164F30T31_Expr }/*expand*/;
assign QuSoCModule_L90F36T59_Expr_1 = internalMemAddress;
assign ModuleCommon_Address = CPU_MemAddress;
assign ModuleCommon_WriteValue = CPU_MemWriteData;
assign ModuleCommon_WE = CPU_MemWrite;
assign ModuleCommon_RE = CPU_MemRead;
assign QuSoCModule_L89F43T74_Source = CPU_MemAddress;
assign internalMemAddress = QuSoCModule_L89F43T74_Source;
assign wordAddress = QuSoCModule_L90F36T59_Expr;
assign QuSoCModule_L91F37T77_Source = QuSoCModule_L91F53T76_Index;
assign QuSoCModule_L91F37T89_Resize = { {24{1'b0}}, QuSoCModule_L91F37T77_Source }/*expand*/;
assign uartReadData = QuSoCModule_L91F37T89_Resize;
assign QuSoCModule_L97F13L123T14_QuSoCModule_L113F17L120T18_QuSoCModule_L114F29T45_Cast = { {10{1'b0}}, memSegment }/*expand*/;
assign internalMemReadData = QuSoCModule_L97F13L123T14_result;
assign internalMemReady = State_MemReady;
assign QuSoCModule_L128F35T54_Index = wordAddress[32:11];
assign memSegment = QuSoCModule_L128F35T54_Index;
assign QuSoCModule_L129F40T57_Index = wordAddress[10:1];
assign blockRamAddress = QuSoCModule_L129F40T57_Index;
assign QuSoCModule_L131F36T60_Index = internalMemAddress[2:1];
assign uartAddress = QuSoCModule_L131F36T60_Index;
assign UARTReady = QuSoCModule_L135F27T45_Expr;
assign CPU_BaseAddress = { {31{1'b0}}, QuSoCModule_L63F31T33_Expr }/*expand*/;
assign CPU_MemReadData = internalMemReadData;
assign CPU_MemReady = internalMemReady;
assign CPU_ExtIRQ = RISCVModule_Types_L11F30T35_Expr;
assign CounterRegister_Common_Address = ModuleCommon_Address;
assign CounterRegister_Common_WriteValue = ModuleCommon_WriteValue;
assign CounterRegister_Common_WE = ModuleCommon_WE;
assign CounterRegister_Common_RE = ModuleCommon_RE;
assign CounterRegister_DeviceAddress = QuSoCModule_L71F33T43_Expr;
assign QuSoCModule_L78F33T55_Index = CPU_MemAccessMode[2:1];
assign BlockRAM_MemAccessMode = QuSoCModule_L78F33T55_Index;
assign BlockRAM_Common_Address = ModuleCommon_Address;
assign BlockRAM_Common_WriteValue = ModuleCommon_WriteValue;
assign BlockRAM_Common_WE = ModuleCommon_WE;
assign BlockRAM_Common_RE = ModuleCommon_RE;
assign BlockRAM_DeviceAddress = QuSoCModule_L77F33T43_Expr;
assign QuSoCModule_L85F33T56_Index = CPU_MemAccessMode[2:1];
assign InstructionsRAM_MemAccessMode = QuSoCModule_L85F33T56_Index;
assign InstructionsRAM_Common_Address = ModuleCommon_Address;
assign InstructionsRAM_Common_WriteValue = ModuleCommon_WriteValue;
assign InstructionsRAM_Common_WE = ModuleCommon_WE;
assign InstructionsRAM_Common_RE = ModuleCommon_RE;
assign InstructionsRAM_DeviceAddress = { {31{1'b0}}, QuSoCModule_L84F33T43_Expr }/*expand*/;
assign QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F29T45_Cast = { {10{1'b0}}, memSegment }/*expand*/;
assign QuSoCModule_L138F9L182T10_QuSoCModule_L147F13L181T14_QuSoCModule_L161F17L180T18_QuSoCModule_L162F21L179T22_QuSoCModule_L167F29L173T30_QuSoCModule_L169F53T75_Cast = CPU_MemWriteData[8:1]/*truncate*/;
assign Counter = CounterRegister_ReadValue;
assign UARTWriteData = QuSoCModule_L133F38T51_Index;
assign InstructionsRAMCommon_AddressInstructionsRAM_Common_AddressHardLink = InstructionsRAM_Common_Address;
assign InstructionsRAMCommon_WriteValueInstructionsRAM_Common_WriteValueHardLink = InstructionsRAM_Common_WriteValue;
assign InstructionsRAMCommon_WEInstructionsRAM_Common_WEHardLink = InstructionsRAM_Common_WE;
assign InstructionsRAMCommon_REInstructionsRAM_Common_REHardLink = InstructionsRAM_Common_RE;
assign InstructionsRAMDeviceAddressInstructionsRAM_DeviceAddressHardLink = InstructionsRAM_DeviceAddress;
assign InstructionsRAMMemAccessModeInstructionsRAM_MemAccessModeHardLink = InstructionsRAM_MemAccessMode;
assign InstructionsRAM_ReadValue = InstructionsRAMReadValueInstructionsRAM_ReadValueHardLink;
assign InstructionsRAM_IsReady = InstructionsRAMIsReadyInstructionsRAM_IsReadyHardLink;
assign InstructionsRAM_IsActive = InstructionsRAMIsActiveInstructionsRAM_IsActiveHardLink;
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
assign QuSoCModule_L91F53T76_Index = State_UART[uartAddress];
assign QuSoCModule_L135F27T40_Index = State_UART[QuSoCModule_L135F38T39_Expr];
assign QuSoCModule_L133F38T51_Index = State_UART[QuSoCModule_L133F49T50_Expr];
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
