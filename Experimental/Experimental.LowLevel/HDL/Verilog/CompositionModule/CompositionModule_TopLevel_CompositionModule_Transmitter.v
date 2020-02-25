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
// System configuration name is CompositionModule_TopLevel_CompositionModule_Transmitter, clock frequency is 1Hz, Embedded
// FSM summary
// -- Packages
module CompositionModule_TopLevel_CompositionModule_Transmitter (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  BoardSignals_Clock,
	input  BoardSignals_Reset,
	input  BoardSignals_Running,
	input  BoardSignals_Starting,
	input  BoardSignals_Started,
	input  TransmitterModuleTrigger,
	input  TransmitterModuleAck,
	input  [8: 1] TransmitterModuleData,
	output TransmitterModuleBit,
	output TransmitterModuleIsReady,
	output TransmitterModuleIsTransmitting,
	output TransmitterModuleIsTransmissionStarted
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleTrigger;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleAck;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleData;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleBit;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsReady;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsTransmitting;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsTransmissionStarted;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Trigger;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Ack;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Data;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Bit;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsReady;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsTransmitting;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsTransmissionStarted;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Zero = 1'b0;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_One = 1'b1;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_true = 1'b1;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_false = 1'b0;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F46T65_Expr = 1'b0;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F52T79_Expr = 1'b1;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F59T78_Expr = 1'b0;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F99T126_Expr = 1'b1;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Inputs_Trigger;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Inputs_Ack;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Inputs_Data;
reg signed  [32:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_FSM = 32'b00000000000000000000000000000000;
reg  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_Data = 8'b00000000;
reg  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_Counter = 8'b00000000;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L8F28T41_Index;
wire signed  [32:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM;
wire signed  [32:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMDefault = 32'b00000000000000000000000000000000;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMWriteEnable;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_Data;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataDefault = 8'b00000000;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataWriteEnable;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_Counter;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterDefault = 8'b00000000;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterWriteEnable;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr_1;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr_2;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_Expr;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_ExprLhs;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_ExprRhs;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_Expr;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_ExprLhs;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_ExprRhs;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_Expr;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_ExprLhs;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_ExprRhs;
wire  CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_Expr;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_ExprLhs;
wire signed  [33:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_ExprRhs;
reg signed  [32:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMQ = 32'b00000000000000000000000000000000;
wire signed  [32:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMD;
reg  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataQ = 8'b00000000;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataD;
reg  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterQ = 8'b00000000;
wire  [8:1] CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterD;
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMDefault;
end
else if ( CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMWriteEnable == 1 ) begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMD;
end
else begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMQ;
end
end
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataDefault;
end
else if ( CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataWriteEnable == 1 ) begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataD;
end
else begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataQ;
end
end
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterDefault;
end
else if ( CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterWriteEnable == 1 ) begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterD;
end
else begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterQ <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterQ;
end
end
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_Expr = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_ExprLhs == CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_ExprRhs ? 1'b1 : 1'b0;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_Expr = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_ExprLhs == CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_ExprRhs ? 1'b1 : 1'b0;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_Expr = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_ExprLhs == CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_ExprRhs ? 1'b1 : 1'b0;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_Expr = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_ExprLhs == CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_ExprRhs ? 1'b1 : 1'b0;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr_1 & CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr_2;
always @(posedge CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Clock)
begin
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_FSM/*cast*/;
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_Data <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_Data/*cast*/;
CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_Counter <= CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_Counter/*cast*/;
end
	assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleTrigger = TransmitterModuleTrigger;
	assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleAck = TransmitterModuleAck;
	assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleData = TransmitterModuleData;
assign TransmitterModuleBit = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleBit;
assign TransmitterModuleIsReady = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsReady;
assign TransmitterModuleIsTransmitting = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsTransmitting;
assign TransmitterModuleIsTransmissionStarted = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsTransmissionStarted;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMQ;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMD = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_FSM;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSMWriteEnable = HiSignal;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_Data = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataQ;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataD = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_Data;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_DataWriteEnable = HiSignal;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_Counter = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterQ;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterD = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_Counter;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_CounterWriteEnable = HiSignal;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Trigger = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleTrigger;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Ack = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleAck;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Data = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleData;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleBit = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Bit;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsReady = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsReady;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsTransmitting = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsTransmitting;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModuleIsTransmissionStarted = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsTransmissionStarted;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_ExprLhs = { {1{CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM[32]}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_ExprRhs = { {32{1'b0}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F46T65_Expr }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_ExprLhs = { {1{CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM[32]}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_ExprRhs = { {32{1'b0}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F52T79_Expr }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_ExprLhs = { {1{CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM[32]}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_FSM }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_ExprRhs = { {32{1'b0}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F59T78_Expr }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_ExprLhs = { {1{CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_FSM[32]}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_NextState_FSM }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_ExprRhs = { {32{1'b0}}, CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F99T126_Expr }/*expand*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr_1 = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T78_Expr;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr_2 = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F82T126_Expr;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Inputs_Trigger = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Trigger;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Inputs_Ack = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Ack;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Inputs_Data = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Data/*cast*/;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L8F28T41_Index = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_State_Data[1];
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_Bit = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L8F28T41_Index;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsReady = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L9F32T65_Expr;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsTransmitting = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L10F39T79_Expr;
assign CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_IsTransmissionStarted = CompositionModule_TopLevel_CompositionModule_Transmitter_TransmitterModule_TransmitterModule_L11F46T126_Expr;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
