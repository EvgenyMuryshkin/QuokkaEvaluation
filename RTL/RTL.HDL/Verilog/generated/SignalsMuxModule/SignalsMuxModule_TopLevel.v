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
// System configuration name is SignalsMuxModule_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module SignalsMuxModule_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  [1: 0] Addr,
	input  [7: 0] Sig0,
	input  [7: 0] Sig1,
	input  [7: 0] Sig2,
	input  [7: 0] Sig3,
	output [7: 0] Value
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
wire  [7:0] Inputs_Sig0;
wire  [7:0] Inputs_Sig1;
wire  [7:0] Inputs_Sig2;
wire  [7:0] Inputs_Sig3;
reg  [7:0] SignalsMuxModule_L24F30T50_Mux = 8'b00000000;
wire  [1:0] SignalsMuxModule_L24F30T50_MuxMultiplexerAddress;
wire  [7:0] SignalsMuxModule_L24F30T50_Mux1;
wire  [7:0] SignalsMuxModule_L24F30T50_Mux2;
wire  [7:0] SignalsMuxModule_L24F30T50_Mux3;
wire  [7:0] SignalsMuxModule_L24F30T50_Mux4;
always @*
begin
case (SignalsMuxModule_L24F30T50_MuxMultiplexerAddress)
    'b00:
SignalsMuxModule_L24F30T50_Mux = SignalsMuxModule_L24F30T50_Mux1;
    'b01:
SignalsMuxModule_L24F30T50_Mux = SignalsMuxModule_L24F30T50_Mux2;
    'b10:
SignalsMuxModule_L24F30T50_Mux = SignalsMuxModule_L24F30T50_Mux3;
    'b11:
SignalsMuxModule_L24F30T50_Mux = SignalsMuxModule_L24F30T50_Mux4;
  default:
SignalsMuxModule_L24F30T50_Mux = 'b00000000;
endcase

end
assign Inputs_Addr = Addr;
assign Inputs_Sig0 = Sig0;
assign Inputs_Sig1 = Sig1;
assign Inputs_Sig2 = Sig2;
assign Inputs_Sig3 = Sig3;
assign Value = SignalsMuxModule_L24F30T50_Mux;
assign SignalsMuxModule_L24F30T50_Mux1 = Inputs_Sig0;
assign SignalsMuxModule_L24F30T50_Mux2 = Inputs_Sig1;
assign SignalsMuxModule_L24F30T50_Mux3 = Inputs_Sig2;
assign SignalsMuxModule_L24F30T50_Mux4 = Inputs_Sig3;
assign SignalsMuxModule_L24F30T50_MuxMultiplexerAddress = Inputs_Addr;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
