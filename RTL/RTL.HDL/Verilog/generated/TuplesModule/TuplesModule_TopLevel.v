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
// System configuration name is TuplesModule_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module TuplesModule_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  Value1,
	input  Value2,
	output Same,
	output Diff
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  Inputs_Value1;
wire  Inputs_Value2;
wire  Logic_Item1;
wire  Logic_Item2;
reg  TuplesModule_L16F13L21T14_same = 1'b0;
reg  TuplesModule_L16F13L21T14_diff = 1'b0;
wire  TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_Expr;
wire signed  [1:0] TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_ExprLhs;
wire signed  [1:0] TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_ExprRhs;
wire  TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_Expr;
wire signed  [1:0] TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_ExprLhs;
wire signed  [1:0] TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_ExprRhs;
assign TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_Expr = TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_ExprLhs == TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_ExprRhs ? 1'b1 : 1'b0;
assign TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_Expr = TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_ExprLhs != TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_ExprRhs ? 1'b1 : 1'b0;
always @*
begin
TuplesModule_L16F13L21T14_same = TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_Expr;
TuplesModule_L16F13L21T14_diff = TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_Expr;

end
assign TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_ExprLhs = { {1{1'b0}}, Inputs_Value1 }/*expand*/;
assign TuplesModule_L16F13L21T14_TuplesModule_L17F28T58_ExprRhs = { {1{1'b0}}, Inputs_Value2 }/*expand*/;
assign TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_ExprLhs = { {1{1'b0}}, Inputs_Value1 }/*expand*/;
assign TuplesModule_L16F13L21T14_TuplesModule_L18F28T58_ExprRhs = { {1{1'b0}}, Inputs_Value2 }/*expand*/;
assign Inputs_Value1 = Value1;
assign Inputs_Value2 = Value2;
assign Logic_Item1 = TuplesModule_L16F13L21T14_same;
assign Logic_Item2 = TuplesModule_L16F13L21T14_diff;
assign Same = Logic_Item1;
assign Diff = Logic_Item2;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
