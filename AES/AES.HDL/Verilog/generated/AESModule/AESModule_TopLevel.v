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
// System configuration name is AESModule_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module AESModule_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  Clock,
	input  Reset
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  AESModule_L36F29T30_Expr = 1'b0;
wire  AESModule_L37F29T30_Expr = 1'b0;
reg  [128:1] NextState_Value = 128'b00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000;
wire  [8:1] SBoxAddress;
wire  [8:1] RBoxAddress;
wire  [128:1] sbox_Value;
wire  [128:1] sbox_Result;
wire  [8:1] box_SBoxAddress;
wire  [8:1] box_RBoxAddress;
reg  [8:1] box_RConAddress = 8'b00000000;
wire  [8:1] box_SBox;
wire  [8:1] box_RBox;
wire  [8:1] box_RCon;
wire  [128:1] sboxValuesbox_ValueHardLink;
wire  [128:1] sboxResultsbox_ResultHardLink;
wire  [8:1] boxSBoxAddressbox_SBoxAddressHardLink;
wire  [8:1] boxRBoxAddressbox_RBoxAddressHardLink;
wire  [8:1] boxRConAddressbox_RConAddressHardLink;
wire  [8:1] boxSBoxbox_SBoxHardLink;
wire  [8:1] boxRBoxbox_RBoxHardLink;
wire  [8:1] boxRConbox_RConHardLink;
reg  [128:1] State_Value = 128'b00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000;
wire  [128:1] State_ValueDefault = 128'b00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000;
wire  BoardSignals_Clock;
wire  BoardSignals_Reset;
wire  BoardSignals_Running;
wire  BoardSignals_Starting;
wire  BoardSignals_Started;
reg  InternalReset = 1'b0;
work_Quokka_BoardSignalsProc BoardSignalsConnection(BoardSignals_Clock,BoardSignals_Reset,BoardSignals_Running,BoardSignals_Starting,BoardSignals_Started,Clock,Reset,InternalReset);
always @(posedge Clock)
begin
if ( Reset == 1 ) begin
State_Value <= State_ValueDefault;
end
else begin
State_Value <= NextState_Value;
end
end
AESModule_TopLevel_AESModule_sbox AESModule_TopLevel_AESModule_sbox
(
// [BEGIN USER MAP FOR sbox]
// [END USER MAP FOR sbox]
	.Value (sboxValuesbox_ValueHardLink),
	.Result (sboxResultsbox_ResultHardLink)

);
AESModule_TopLevel_AESModule_box AESModule_TopLevel_AESModule_box
(
// [BEGIN USER MAP FOR box]
// [END USER MAP FOR box]
	.SBoxAddress (boxSBoxAddressbox_SBoxAddressHardLink),
	.RBoxAddress (boxRBoxAddressbox_RBoxAddressHardLink),
	.RConAddress (boxRConAddressbox_RConAddressHardLink),
	.SBox (boxSBoxbox_SBoxHardLink),
	.RBox (boxRBoxbox_RBoxHardLink),
	.RCon (boxRConbox_RConHardLink)

);
always @*
begin
NextState_Value = State_Value;

end
assign SBoxAddress = { {7{1'b0}}, AESModule_L36F29T30_Expr }/*expand*/;
assign RBoxAddress = { {7{1'b0}}, AESModule_L37F29T30_Expr }/*expand*/;
assign box_SBoxAddress = SBoxAddress;
assign box_RBoxAddress = RBoxAddress;
assign sbox_Value = State_Value;
assign sboxValuesbox_ValueHardLink = sbox_Value;
assign sbox_Result = sboxResultsbox_ResultHardLink;
assign boxSBoxAddressbox_SBoxAddressHardLink = box_SBoxAddress;
assign boxRBoxAddressbox_RBoxAddressHardLink = box_RBoxAddress;
assign boxRConAddressbox_RConAddressHardLink = box_RConAddress;
assign box_SBox = boxSBoxbox_SBoxHardLink;
assign box_RBox = boxRBoxbox_RBoxHardLink;
assign box_RCon = boxRConbox_RConHardLink;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule