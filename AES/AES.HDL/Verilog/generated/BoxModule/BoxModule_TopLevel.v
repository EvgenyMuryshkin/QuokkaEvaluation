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
// System configuration name is BoxModule_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module BoxModule_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  [7: 0] SBoxAddress,
	input  [7: 0] RBoxAddress,
	input  [7: 0] RConAddress,
	output [7: 0] SBox,
	output [7: 0] RBox,
	output [7: 0] RCon
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  [8:1] Inputs_SBoxAddress;
wire  [8:1] Inputs_RBoxAddress;
wire  [8:1] Inputs_RConAddress;
wire  [8:1] BoxModule_L62F29T57_Index;
wire  [8:1] BoxModule_L63F29T57_Index;
wire  [8:1] BoxModule_L64F29T57_Index;
reg [8:1] sboxData [0 : 255];
initial
begin
	$readmemh("BoxModule_TopLevel_sboxData.hex", sboxData);
end
reg [8:1] rboxData [0 : 255];
initial
begin
	$readmemh("BoxModule_TopLevel_rboxData.hex", rboxData);
end
reg [8:1] rconData [0 : 10];
initial
begin
	$readmemh("BoxModule_TopLevel_rconData.hex", rconData);
end
assign Inputs_SBoxAddress = SBoxAddress;
assign Inputs_RBoxAddress = RBoxAddress;
assign Inputs_RConAddress = RConAddress;
assign SBox = BoxModule_L62F29T57_Index;
assign RBox = BoxModule_L63F29T57_Index;
assign RCon = BoxModule_L64F29T57_Index;
assign BoxModule_L62F29T57_Index = sboxData[Inputs_SBoxAddress];
assign BoxModule_L63F29T57_Index = rboxData[Inputs_RBoxAddress];
assign BoxModule_L64F29T57_Index = rconData[Inputs_RConAddress];
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule