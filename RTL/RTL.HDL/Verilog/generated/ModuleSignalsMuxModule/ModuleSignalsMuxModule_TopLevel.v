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
// System configuration name is ModuleSignalsMuxModule_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module ModuleSignalsMuxModule_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input wire  [1: 0] Addr,
	input wire  I1,
	input wire  I2,
	output wire O,
	output wire [2: 0] CombinedO
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  [1:0] Inputs_Addr;
wire  Inputs_I1;
wire  Inputs_I2;
wire  AndGate_I1;
wire  AndGate_I2;
wire  AndGate_O;
wire  OrGate_I1;
wire  OrGate_I2;
wire  OrGate_O;
wire  XorGate_I1;
wire  XorGate_I2;
wire  XorGate_O;
wire  [2:0] ModuleSignalsMuxModule_L29F41T80_Source;
wire  AndGateI1AndGate_I1HardLink;
wire  AndGateI2AndGate_I2HardLink;
wire  AndGateOAndGate_OHardLink;
wire  OrGateI1OrGate_I1HardLink;
wire  OrGateI2OrGate_I2HardLink;
wire  OrGateOOrGate_OHardLink;
wire  XorGateI1XorGate_I1HardLink;
wire  XorGateI2XorGate_I2HardLink;
wire  XorGateOXorGate_OHardLink;
reg  ModuleSignalsMuxModule_L27F26T46_Mux;
wire  [1:0] ModuleSignalsMuxModule_L27F26T46_MuxMultiplexerAddress;
wire  ModuleSignalsMuxModule_L27F26T46_Mux1;
wire  ModuleSignalsMuxModule_L27F26T46_Mux2;
wire  ModuleSignalsMuxModule_L27F26T46_Mux3;
ModuleSignalsMuxModule_TopLevel_ModuleSignalsMuxModule_AndGate ModuleSignalsMuxModule_TopLevel_ModuleSignalsMuxModule_AndGate
(
// [BEGIN USER MAP FOR AndGate]
// [END USER MAP FOR AndGate]
	.I1 (AndGateI1AndGate_I1HardLink),
	.I2 (AndGateI2AndGate_I2HardLink),
	.O (AndGateOAndGate_OHardLink)

);
ModuleSignalsMuxModule_TopLevel_ModuleSignalsMuxModule_OrGate ModuleSignalsMuxModule_TopLevel_ModuleSignalsMuxModule_OrGate
(
// [BEGIN USER MAP FOR OrGate]
// [END USER MAP FOR OrGate]
	.I1 (OrGateI1OrGate_I1HardLink),
	.I2 (OrGateI2OrGate_I2HardLink),
	.O (OrGateOOrGate_OHardLink)

);
ModuleSignalsMuxModule_TopLevel_ModuleSignalsMuxModule_XorGate ModuleSignalsMuxModule_TopLevel_ModuleSignalsMuxModule_XorGate
(
// [BEGIN USER MAP FOR XorGate]
// [END USER MAP FOR XorGate]
	.I1 (XorGateI1XorGate_I1HardLink),
	.I2 (XorGateI2XorGate_I2HardLink),
	.O (XorGateOXorGate_OHardLink)

);
always @*
begin
case (ModuleSignalsMuxModule_L27F26T46_MuxMultiplexerAddress)
    'b00:
ModuleSignalsMuxModule_L27F26T46_Mux = ModuleSignalsMuxModule_L27F26T46_Mux1;
    'b01:
ModuleSignalsMuxModule_L27F26T46_Mux = ModuleSignalsMuxModule_L27F26T46_Mux2;
    'b10:
ModuleSignalsMuxModule_L27F26T46_Mux = ModuleSignalsMuxModule_L27F26T46_Mux3;
  default:
ModuleSignalsMuxModule_L27F26T46_Mux = 'b0;
endcase

end
assign Inputs_Addr = Addr;
assign Inputs_I1 = I1;
assign Inputs_I2 = I2;
assign AndGate_I1 = Inputs_I1;
assign AndGate_I2 = Inputs_I2;
assign OrGate_I1 = Inputs_I1;
assign OrGate_I2 = Inputs_I2;
assign XorGate_I1 = Inputs_I1;
assign XorGate_I2 = Inputs_I2;
assign O = ModuleSignalsMuxModule_L27F26T46_Mux;
assign ModuleSignalsMuxModule_L29F41T80_Source[0] = XorGate_O;
assign ModuleSignalsMuxModule_L29F41T80_Source[1] = OrGate_O;
assign ModuleSignalsMuxModule_L29F41T80_Source[2] = AndGate_O;
assign CombinedO = ModuleSignalsMuxModule_L29F41T80_Source;
assign AndGateI1AndGate_I1HardLink = AndGate_I1;
assign AndGateI2AndGate_I2HardLink = AndGate_I2;
assign AndGate_O = AndGateOAndGate_OHardLink;
assign OrGateI1OrGate_I1HardLink = OrGate_I1;
assign OrGateI2OrGate_I2HardLink = OrGate_I2;
assign OrGate_O = OrGateOOrGate_OHardLink;
assign XorGateI1XorGate_I1HardLink = XorGate_I1;
assign XorGateI2XorGate_I2HardLink = XorGate_I2;
assign XorGate_O = XorGateOXorGate_OHardLink;
assign ModuleSignalsMuxModule_L27F26T46_Mux1 = AndGate_O;
assign ModuleSignalsMuxModule_L27F26T46_Mux2 = OrGate_O;
assign ModuleSignalsMuxModule_L27F26T46_Mux3 = XorGate_O;
assign ModuleSignalsMuxModule_L27F26T46_MuxMultiplexerAddress = Inputs_Addr;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
