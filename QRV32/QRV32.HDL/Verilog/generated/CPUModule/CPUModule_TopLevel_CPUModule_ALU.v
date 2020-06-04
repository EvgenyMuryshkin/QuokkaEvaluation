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
// System configuration name is CPUModule_TopLevel_CPUModule_ALU, clock frequency is 1Hz, Embedded
// FSM summary
// -- Packages
module CPUModule_TopLevel_CPUModule_ALU (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  [32: 1] Op1,
	input  [32: 1] Op2,
	input  [5: 1] SHAMT,
	output [32: 1] ADD,
	output [32: 1] SUB,
	output [32: 1] resAND,
	output [32: 1] resOR,
	output [32: 1] resXOR,
	output [32: 1] SHLL,
	output [32: 1] SHRL,
	output [32: 1] SHRA
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  [32:1] Inputs_Op1;
wire  [32:1] Inputs_Op2;
wire  [5:1] Inputs_SHAMT;
wire signed  [33:1] internalAdd;
wire signed  [34:1] internalSub;
wire signed  [32:1] ALUModule_L15F46T65_SignChange;
wire signed  [32:1] ALUModule_L15F68T87_SignChange;
wire signed  [32:1] ALUModule_L16F46T65_SignChange;
wire signed  [33:1] ALUModule_L16F46T77_Resize;
wire signed  [32:1] ALUModule_L16F80T99_SignChange;
wire signed  [33:1] ALUModule_L16F80T111_Resize;
wire  [32:1] ALUModule_L18F35T53_Index;
wire  [32:1] ALUModule_L19F35T53_Index;
wire  [32:1] ALUModule_L23F36T71_Index;
wire  [32:1] ALUModule_L24F37T58_SignChange;
wire  [32:1] ALUModule_L24F36T82_Index;
wire signed  [32:1] ALUModule_L25F37T56_SignChange;
wire  [32:1] ALUModule_L25F36T80_Index;
wire  [32:1] ALUModule_L20F38T61_Expr;
wire  [32:1] ALUModule_L20F38T61_Expr_1;
wire  [32:1] ALUModule_L20F38T61_Expr_2;
wire  [32:1] ALUModule_L21F37T60_Expr;
wire  [32:1] ALUModule_L21F37T60_Expr_1;
wire  [32:1] ALUModule_L21F37T60_Expr_2;
wire  [32:1] ALUModule_L22F38T61_Expr;
wire  [32:1] ALUModule_L22F38T61_Expr_1;
wire  [32:1] ALUModule_L22F38T61_Expr_2;
wire  [32:1] ALUModule_L23F37T63_Expr;
wire  [32:1] ALUModule_L23F37T63_Expr_1;
wire  [32:1] ALUModule_L24F37T74_Expr;
wire  [32:1] ALUModule_L24F37T74_Expr_1;
wire signed  [32:1] ALUModule_L25F37T72_Expr;
wire signed  [32:1] ALUModule_L25F37T72_Expr_1;
wire signed  [33:1] ALUModule_L15F46T87_Expr;
wire signed  [33:1] ALUModule_L15F46T87_Expr_1;
wire signed  [33:1] ALUModule_L15F46T87_Expr_2;
wire signed  [34:1] ALUModule_L16F46T111_Expr;
wire signed  [34:1] ALUModule_L16F46T111_Expr_1;
wire signed  [34:1] ALUModule_L16F46T111_Expr_2;
assign ALUModule_L20F38T61_Expr = ALUModule_L20F38T61_Expr_1 & ALUModule_L20F38T61_Expr_2;
assign ALUModule_L21F37T60_Expr = ALUModule_L21F37T60_Expr_1 | ALUModule_L21F37T60_Expr_2;
assign ALUModule_L22F38T61_Expr = ALUModule_L22F38T61_Expr_1 ^ ALUModule_L22F38T61_Expr_2;
assign ALUModule_L23F37T63_Expr = ALUModule_L23F37T63_Expr_1 << Inputs_SHAMT;
assign ALUModule_L24F37T74_Expr = ALUModule_L24F37T74_Expr_1 >> Inputs_SHAMT;
assign ALUModule_L25F37T72_Expr = ALUModule_L25F37T72_Expr_1 >> Inputs_SHAMT;
assign ALUModule_L15F46T87_Expr = ALUModule_L15F46T87_Expr_1 + ALUModule_L15F46T87_Expr_2;
assign ALUModule_L16F46T111_Expr = ALUModule_L16F46T111_Expr_1 - ALUModule_L16F46T111_Expr_2;
assign ALUModule_L20F38T61_Expr_1 = Inputs_Op1/*cast*/;
assign ALUModule_L20F38T61_Expr_2 = Inputs_Op2/*cast*/;
assign ALUModule_L21F37T60_Expr_1 = Inputs_Op1/*cast*/;
assign ALUModule_L21F37T60_Expr_2 = Inputs_Op2/*cast*/;
assign ALUModule_L22F38T61_Expr_1 = Inputs_Op1/*cast*/;
assign ALUModule_L22F38T61_Expr_2 = Inputs_Op2/*cast*/;
assign ALUModule_L23F37T63_Expr_1 = Inputs_Op1/*cast*/;
assign ALUModule_L24F37T74_Expr_1 = ALUModule_L24F37T58_SignChange/*cast*/;
assign ALUModule_L25F37T72_Expr_1 = ALUModule_L25F37T56_SignChange/*cast*/;
assign ALUModule_L15F46T87_Expr_1 = { {1{ALUModule_L15F46T65_SignChange[32]}}, ALUModule_L15F46T65_SignChange }/*expand*/;
assign ALUModule_L15F46T87_Expr_2 = { {1{ALUModule_L15F68T87_SignChange[32]}}, ALUModule_L15F68T87_SignChange }/*expand*/;
assign ALUModule_L16F46T111_Expr_1 = { {1{ALUModule_L16F46T77_Resize[33]}}, ALUModule_L16F46T77_Resize }/*expand*/;
assign ALUModule_L16F46T111_Expr_2 = { {1{ALUModule_L16F80T111_Resize[33]}}, ALUModule_L16F80T111_Resize }/*expand*/;
assign Inputs_Op1 = Op1/*cast*/;
assign Inputs_Op2 = Op2/*cast*/;
assign Inputs_SHAMT = SHAMT/*cast*/;
assign ALUModule_L15F46T65_SignChange = Inputs_Op1/*cast*/;
assign ALUModule_L15F68T87_SignChange = Inputs_Op2/*cast*/;
assign internalAdd = ALUModule_L15F46T87_Expr/*cast*/;
assign ALUModule_L16F46T65_SignChange = Inputs_Op1/*cast*/;
assign ALUModule_L16F46T77_Resize = { {1{ALUModule_L16F46T65_SignChange[32]}}, ALUModule_L16F46T65_SignChange }/*expand*/;
assign ALUModule_L16F80T99_SignChange = Inputs_Op2/*cast*/;
assign ALUModule_L16F80T111_Resize = { {1{ALUModule_L16F80T99_SignChange[32]}}, ALUModule_L16F80T99_SignChange }/*expand*/;
assign internalSub = ALUModule_L16F46T111_Expr/*cast*/;
assign ALUModule_L18F35T53_Index = internalAdd[32:1]/*cast*/;
assign ADD = ALUModule_L18F35T53_Index/*cast*/;
assign ALUModule_L19F35T53_Index = internalSub[32:1]/*cast*/;
assign SUB = ALUModule_L19F35T53_Index/*cast*/;
assign resAND = ALUModule_L20F38T61_Expr/*cast*/;
assign resOR = ALUModule_L21F37T60_Expr/*cast*/;
assign resXOR = ALUModule_L22F38T61_Expr/*cast*/;
assign ALUModule_L23F36T71_Index = ALUModule_L23F37T63_Expr/*cast*/;
assign SHLL = ALUModule_L23F36T71_Index/*cast*/;
assign ALUModule_L24F37T58_SignChange = Inputs_Op1/*cast*/;
assign ALUModule_L24F36T82_Index = ALUModule_L24F37T74_Expr/*cast*/;
assign SHRL = ALUModule_L24F36T82_Index/*cast*/;
assign ALUModule_L25F37T56_SignChange = Inputs_Op1/*cast*/;
assign ALUModule_L25F36T80_Index = ALUModule_L25F37T72_Expr/*cast*/;
assign SHRA = ALUModule_L25F36T80_Index/*cast*/;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
