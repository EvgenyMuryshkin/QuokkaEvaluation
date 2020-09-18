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
// System configuration name is BitArrayModule_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module BitArrayModule_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  [7: 0] Value,
	output [7: 0] Direct,
	output [3: 0] High,
	output [3: 0] Low,
	output [7: 0] Reversed,
	output [3: 0] ReversedHigh,
	output [3: 0] ReversedLow,
	output [3: 0] Picks,
	output [3: 0] FromBits1,
	output [3: 0] FromBits2
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  BitArrayModule_L21F57T61_Expr = 1'b1;
wire  BitArrayModule_L21F63T67_Expr = 1'b1;
wire  BitArrayModule_L21F69T74_Expr = 1'b0;
wire  BitArrayModule_L21F76T80_Expr = 1'b1;
wire  BitArrayModule_L22F57T62_Expr = 1'b0;
wire  BitArrayModule_L22F64T68_Expr = 1'b1;
wire  BitArrayModule_L22F70T74_Expr = 1'b1;
wire  BitArrayModule_L22F76T80_Expr = 1'b1;
wire  [7:0] Inputs_Value;
wire  [7:0] Bits;
wire  [3:0] BitArrayModule_L15F36T46_Index;
wire  [3:0] BitArrayModule_L16F35T45_Index;
wire  [7:0] BitArrayModule_L17F40T50_Index;
wire  [3:0] BitArrayModule_L18F44T54_Index;
wire  [3:0] BitArrayModule_L19F43T53_Index;
wire  [3:0] BitArrayModule_L20F37T74_Source;
wire  [1:0] BitArrayModule_L20F53T62_Index;
wire  [1:0] BitArrayModule_L20F64T73_Index;
wire  [3:0] BitArrayModule_L21F41T81_Source;
wire  [3:0] BitArrayModule_L22F41T81_Source;
assign Inputs_Value = Value;
assign Bits = Inputs_Value;
assign Direct = Bits;
assign BitArrayModule_L15F36T46_Index = Bits[7:4];
assign High = BitArrayModule_L15F36T46_Index;
assign BitArrayModule_L16F35T45_Index = Bits[3:0];
assign Low = BitArrayModule_L16F35T45_Index;
assign BitArrayModule_L17F40T50_Index[0] = Bits[7];
assign BitArrayModule_L17F40T50_Index[1] = Bits[6];
assign BitArrayModule_L17F40T50_Index[2] = Bits[5];
assign BitArrayModule_L17F40T50_Index[3] = Bits[4];
assign BitArrayModule_L17F40T50_Index[4] = Bits[3];
assign BitArrayModule_L17F40T50_Index[5] = Bits[2];
assign BitArrayModule_L17F40T50_Index[6] = Bits[1];
assign BitArrayModule_L17F40T50_Index[7] = Bits[0];
assign Reversed = BitArrayModule_L17F40T50_Index;
assign BitArrayModule_L18F44T54_Index[0] = Bits[7];
assign BitArrayModule_L18F44T54_Index[1] = Bits[6];
assign BitArrayModule_L18F44T54_Index[2] = Bits[5];
assign BitArrayModule_L18F44T54_Index[3] = Bits[4];
assign ReversedHigh = BitArrayModule_L18F44T54_Index;
assign BitArrayModule_L19F43T53_Index[0] = Bits[3];
assign BitArrayModule_L19F43T53_Index[1] = Bits[2];
assign BitArrayModule_L19F43T53_Index[2] = Bits[1];
assign BitArrayModule_L19F43T53_Index[3] = Bits[0];
assign ReversedLow = BitArrayModule_L19F43T53_Index;
assign BitArrayModule_L20F53T62_Index = Bits[6:5];
assign BitArrayModule_L20F64T73_Index[0] = Bits[1];
assign BitArrayModule_L20F64T73_Index[1] = Bits[0];
assign BitArrayModule_L20F37T74_Source[0] = BitArrayModule_L20F64T73_Index[0];
assign BitArrayModule_L20F37T74_Source[1] = BitArrayModule_L20F64T73_Index[1];
assign BitArrayModule_L20F37T74_Source[2] = BitArrayModule_L20F53T62_Index[0];
assign BitArrayModule_L20F37T74_Source[3] = BitArrayModule_L20F53T62_Index[1];
assign Picks = BitArrayModule_L20F37T74_Source;
assign BitArrayModule_L21F41T81_Source[0] = BitArrayModule_L21F76T80_Expr;
assign BitArrayModule_L21F41T81_Source[1] = BitArrayModule_L21F69T74_Expr;
assign BitArrayModule_L21F41T81_Source[2] = BitArrayModule_L21F63T67_Expr;
assign BitArrayModule_L21F41T81_Source[3] = BitArrayModule_L21F57T61_Expr;
assign FromBits1 = BitArrayModule_L21F41T81_Source;
assign BitArrayModule_L22F41T81_Source[0] = BitArrayModule_L22F76T80_Expr;
assign BitArrayModule_L22F41T81_Source[1] = BitArrayModule_L22F70T74_Expr;
assign BitArrayModule_L22F41T81_Source[2] = BitArrayModule_L22F64T68_Expr;
assign BitArrayModule_L22F41T81_Source[3] = BitArrayModule_L22F57T62_Expr;
assign FromBits2 = BitArrayModule_L22F41T81_Source;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
