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
// System configuration name is Arrays_TopLevel_QuSoCModule_CSCounterModule, clock frequency is 1Hz, Embedded
// FSM summary
// -- Packages
module Arrays_TopLevel_QuSoCModule_CSCounterModule (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  BoardSignals_Clock,
	input  BoardSignals_Reset,
	input  BoardSignals_Running,
	input  BoardSignals_Starting,
	input  BoardSignals_Started,
	input  [32: 1] Address,
	input  [32: 1] WriteValue,
	input  WE,
	input  [32: 1] RegisterAddress,
	output [32: 1] ReadValue,
	output IsReady,
	output IsActive
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  SoCRegisterModule_L23F41T45_Expr = 1'b1;
wire  [32:1] Inputs_Address;
wire  [32:1] Inputs_WriteValue;
wire  Inputs_WE;
wire  [32:1] Inputs_RegisterAddress;
reg  [32:1] NextState_Value = 32'b00000000000000000000000000000000;
wire  internalIsActive;
reg  [32:1] State_Value = 32'b00000000000000000000000000000000;
wire  [32:1] State_ValueDefault = 32'b00000000000000000000000000000000;
wire  SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr;
wire  SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr_1;
wire  SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr_2;
wire  SoCRegisterModule_L20F34T74_Expr;
wire signed  [33:1] SoCRegisterModule_L20F34T74_ExprLhs;
wire signed  [33:1] SoCRegisterModule_L20F34T74_ExprRhs;
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
State_Value <= State_ValueDefault;
end
else begin
State_Value <= NextState_Value;
end
end
assign SoCRegisterModule_L20F34T74_Expr = SoCRegisterModule_L20F34T74_ExprLhs == SoCRegisterModule_L20F34T74_ExprRhs ? 1'b1 : 1'b0;
assign SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr = SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr_1 & SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr_2;
always @*
begin
NextState_Value = State_Value;
if ( SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr == 1 ) begin
NextState_Value = Inputs_WriteValue;
end

end
assign SoCRegisterModule_L20F34T74_ExprLhs = { {1{1'b0}}, Inputs_Address }/*expand*/;
assign SoCRegisterModule_L20F34T74_ExprRhs = { {1{1'b0}}, Inputs_RegisterAddress }/*expand*/;
assign SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr_1 = Inputs_WE;
assign SoCRegisterModule_L26F9L31T10_SoCRegisterModule_L27F17T46_Expr_2 = internalIsActive;
assign Inputs_Address = Address;
assign Inputs_WriteValue = WriteValue;
assign Inputs_WE = WE;
assign Inputs_RegisterAddress = RegisterAddress;
assign internalIsActive = SoCRegisterModule_L20F34T74_Expr;
assign ReadValue = State_Value;
assign IsReady = SoCRegisterModule_L23F41T45_Expr;
assign IsActive = internalIsActive;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
