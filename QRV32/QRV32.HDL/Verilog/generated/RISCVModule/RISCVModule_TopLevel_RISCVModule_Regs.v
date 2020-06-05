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
// System configuration name is RISCVModule_TopLevel_RISCVModule_Regs, clock frequency is 1Hz, Embedded
// FSM summary
// -- Packages
module RISCVModule_TopLevel_RISCVModule_Regs (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  BoardSignals_Clock,
	input  BoardSignals_Reset,
	input  BoardSignals_Running,
	input  BoardSignals_Starting,
	input  BoardSignals_Started,
	input  Read,
	input  [5: 1] RS1Addr,
	input  [5: 1] RS2Addr,
	input  [5: 1] RD,
	input  WE,
	input  [32: 1] WriteData,
	output [32: 1] RS1,
	output [32: 1] RS2,
	output Ready
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  RegistersRAMModule_L24F50T51_Expr = 1'b0;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F48T49_Expr = 1'b0;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L35F31T36_Expr = 1'b0;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F46T47_Expr = 1'b0;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L38F13L40T14_RegistersRAMModule_L39F34T35_Expr = 1'b1;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F31T32_Expr = 1'b1;
wire  [2:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L43F13L46T14_RegistersRAMModule_L44F34T35_Expr = 2'b10;
wire  [2:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F31T32_Expr = 2'b10;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L49F13L53T14_RegistersRAMModule_L50F35T39_Expr = 1'b1;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L49F13L53T14_RegistersRAMModule_L51F34T35_Expr = 1'b0;
wire  Inputs_Read;
wire  [5:1] Inputs_RS1Addr;
wire  [5:1] Inputs_RS2Addr;
wire  [5:1] Inputs_RD;
wire  Inputs_WE;
wire  [32:1] Inputs_WriteData;
reg  [8:1] NextState_Mode = 8'b00000000;
reg  [32:1] NextState_RS1 = 32'b00000000000000000000000000000000;
reg  [32:1] NextState_RS2 = 32'b00000000000000000000000000000000;
reg  NextState_Ready = 1'b1;
wire  [5:1] ReadAddress;
wire  RegistersRAMModule_L27F9L54T10_we;
reg  [32:1] State_ReadData = 32'b00000000000000000000000000000000;
reg  [8:1] State_Mode = 8'b00000000;
wire  [8:1] State_ModeDefault = 8'b00000000;
reg  [32:1] State_RS1 = 32'b00000000000000000000000000000000;
wire  [32:1] State_RS1Default = 32'b00000000000000000000000000000000;
reg  [32:1] State_RS2 = 32'b00000000000000000000000000000000;
wire  [32:1] State_RS2Default = 32'b00000000000000000000000000000000;
reg  State_Ready = 1'b0;
wire  State_ReadyDefault = 1'b1;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr_1;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr_2;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr_1;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr_2;
wire  RegistersRAMModule_L24F36T51_Expr;
wire signed  [9:1] RegistersRAMModule_L24F36T51_ExprLhs;
wire signed  [9:1] RegistersRAMModule_L24F36T51_ExprRhs;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_Expr;
wire signed  [6:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_ExprLhs;
wire signed  [6:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_ExprRhs;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_Expr;
wire signed  [9:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_ExprLhs;
wire signed  [9:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_ExprRhs;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_Expr;
wire signed  [9:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_ExprLhs;
wire signed  [9:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_ExprRhs;
wire  RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_Expr;
wire signed  [9:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_ExprLhs;
wire signed  [9:1] RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_ExprRhs;
reg  [5:1] RegistersRAMModule_L24F36T85_Lookup = 5'b00000;
wire  RegistersRAMModule_L24F36T85_LookupMultiplexerAddress;
wire  [5:1] RegistersRAMModule_L24F36T85_Lookup1;
wire  [5:1] RegistersRAMModule_L24F36T85_Lookup2;
reg [32:1] State_x [0 : 31];
initial
begin
	integer i;
	for (i = 0; i < 32; i = i + 1)
		State_x[i] = 0;
end
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
State_Mode <= State_ModeDefault;
State_RS1 <= State_RS1Default;
State_RS2 <= State_RS2Default;
State_Ready <= State_ReadyDefault;
end
else begin
State_Mode <= NextState_Mode;
State_RS1 <= NextState_RS1;
State_RS2 <= NextState_RS2;
State_Ready <= NextState_Ready;
end
end
assign RegistersRAMModule_L24F36T51_Expr = RegistersRAMModule_L24F36T51_ExprLhs == RegistersRAMModule_L24F36T51_ExprRhs ? 1'b1 : 1'b0;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_Expr = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_ExprLhs != RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_ExprRhs ? 1'b1 : 1'b0;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_Expr = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_ExprLhs == RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_ExprRhs ? 1'b1 : 1'b0;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_Expr = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_ExprLhs == RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_ExprRhs ? 1'b1 : 1'b0;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_Expr = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_ExprLhs == RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_ExprRhs ? 1'b1 : 1'b0;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr_1 & RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr_2;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr_1 & RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr_2;
always @*
begin
case (RegistersRAMModule_L24F36T85_LookupMultiplexerAddress)
    'b0:
RegistersRAMModule_L24F36T85_Lookup = RegistersRAMModule_L24F36T85_Lookup1;
    'b1:
RegistersRAMModule_L24F36T85_Lookup = RegistersRAMModule_L24F36T85_Lookup2;
  default:
RegistersRAMModule_L24F36T85_Lookup = 'b00000;
endcase

end
always @*
begin
NextState_Mode = State_Mode/*cast*/;
NextState_RS1 = State_RS1/*cast*/;
NextState_RS2 = State_RS2/*cast*/;
NextState_Ready = State_Ready;
if ( RegistersRAMModule_L27F9L54T10_we == 1 ) begin
end
NextState_Ready = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L35F31T36_Expr;
if ( RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr == 1 ) begin
NextState_Mode = { {7{1'b0}}, RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L38F13L40T14_RegistersRAMModule_L39F34T35_Expr }/*expand*/;
end
if ( RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_Expr == 1 ) begin
NextState_Mode = { {6{1'b0}}, RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L43F13L46T14_RegistersRAMModule_L44F34T35_Expr }/*expand*/;
NextState_RS1 = State_ReadData/*cast*/;
end
if ( RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_Expr == 1 ) begin
NextState_Ready = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L49F13L53T14_RegistersRAMModule_L50F35T39_Expr;
NextState_Mode = { {7{1'b0}}, RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L49F13L53T14_RegistersRAMModule_L51F34T35_Expr }/*expand*/;
NextState_RS2 = State_ReadData/*cast*/;
end

end
// inferred simple dual port RAM with read-first behaviour
always @ (posedge BoardSignals_Clock)
begin
	if (RegistersRAMModule_L27F9L54T10_we)
		State_x[Inputs_RD] <= Inputs_WriteData;

	State_ReadData <= State_x[ReadAddress];
end

assign RegistersRAMModule_L24F36T85_LookupMultiplexerAddress = RegistersRAMModule_L24F36T51_Expr;
assign RegistersRAMModule_L24F36T51_ExprLhs = { {1{1'b0}}, State_Mode }/*expand*/;
assign RegistersRAMModule_L24F36T51_ExprRhs = { {8{1'b0}}, RegistersRAMModule_L24F50T51_Expr }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_ExprLhs = { {1{1'b0}}, Inputs_RD }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_ExprRhs = { {5{1'b0}}, RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F48T49_Expr }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_ExprLhs = { {1{1'b0}}, State_Mode }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_ExprRhs = { {8{1'b0}}, RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F46T47_Expr }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_ExprLhs = { {1{1'b0}}, State_Mode }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F17T32_ExprRhs = { {8{1'b0}}, RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L42F31T32_Expr }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_ExprLhs = { {1{1'b0}}, State_Mode }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F17T32_ExprRhs = { {7{1'b0}}, RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L48F31T32_Expr }/*expand*/;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr_1 = Inputs_WE;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr_2 = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F35T49_Expr;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr_1 = Inputs_Read;
assign RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F17T47_Expr_2 = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L37F32T47_Expr;
assign Inputs_Read = Read;
assign Inputs_RS1Addr = RS1Addr/*cast*/;
assign Inputs_RS2Addr = RS2Addr/*cast*/;
assign Inputs_RD = RD/*cast*/;
assign Inputs_WE = WE;
assign Inputs_WriteData = WriteData/*cast*/;
assign ReadAddress = RegistersRAMModule_L24F36T85_Lookup/*cast*/;
assign RegistersRAMModule_L27F9L54T10_we = RegistersRAMModule_L27F9L54T10_RegistersRAMModule_L28F22T49_Expr;
assign RS1 = State_RS1/*cast*/;
assign RS2 = State_RS2/*cast*/;
assign Ready = State_Ready;
assign RegistersRAMModule_L24F36T85_Lookup1 = Inputs_RS2Addr/*cast*/;
assign RegistersRAMModule_L24F36T85_Lookup2 = Inputs_RS1Addr/*cast*/;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
