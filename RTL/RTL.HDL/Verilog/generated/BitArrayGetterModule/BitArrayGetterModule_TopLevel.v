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
// System configuration name is BitArrayGetterModule_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module BitArrayGetterModule_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  [7: 0] Value,
	output [7: 0] Getter
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  [8:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L17F46T59_Expr = 8'b11111111;
wire  [6:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F36T38_Expr = 6'b110010;
wire  [7:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F41T44_Expr = 7'b1100100;
wire  [8:1] Inputs_Value;
wire  [8:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L17F30T60_Source;
reg  [8:1] BitArrayGetterModule_L16F13L25T14_result = 8'b00000000;
reg  [8:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L20F30T38_Cast = 8'b00101010;
wire  [8:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L22F30T59_Source;
wire  [8:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L22F30T70_Resize;
wire  BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_Expr;
wire signed  [9:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_ExprLhs;
wire signed  [9:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_ExprRhs;
wire  BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_Expr;
wire signed  [9:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_ExprLhs;
wire signed  [9:1] BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_ExprRhs;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_Expr = BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_ExprLhs < BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_ExprRhs ? 1'b1 : 1'b0;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_Expr = BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_ExprLhs < BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_ExprRhs ? 1'b1 : 1'b0;
always @*
begin
BitArrayGetterModule_L16F13L25T14_result = BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L17F30T60_Source;
if ( BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_Expr == 1 ) begin
BitArrayGetterModule_L16F13L25T14_result = BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L20F30T38_Cast;
end
else if ( BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_Expr == 1 ) begin
BitArrayGetterModule_L16F13L25T14_result = BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L22F30T70_Resize;
end

end
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_ExprLhs = { {1{1'b0}}, Inputs_Value }/*expand*/;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F21T38_ExprRhs = { {3{1'b0}}, BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L19F36T38_Expr }/*expand*/;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_ExprLhs = { {1{1'b0}}, Inputs_Value }/*expand*/;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F26T44_ExprRhs = { {2{1'b0}}, BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L21F41T44_Expr }/*expand*/;
assign Inputs_Value = Value;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L17F30T60_Source = BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L17F46T59_Expr;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L22F30T59_Source = Inputs_Value;
assign BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L22F30T70_Resize = BitArrayGetterModule_L16F13L25T14_BitArrayGetterModule_L22F30T59_Source;
assign Getter = BitArrayGetterModule_L16F13L25T14_result;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
