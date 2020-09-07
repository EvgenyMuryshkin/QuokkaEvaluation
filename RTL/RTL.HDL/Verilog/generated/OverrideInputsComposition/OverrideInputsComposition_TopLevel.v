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
// System configuration name is OverrideInputsComposition_TopLevel, clock frequency is 1Hz, Top-level
// FSM summary
// -- Packages
module OverrideInputsComposition_TopLevel (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  InOverride,
	input  [7: 0] InValue,
	output [7: 0] NoOverrideValue,
	output [7: 0] AutoOverrideValue,
	output [7: 0] L1Value,
	output [7: 0] L2Value,
	output [7: 0] L3Value,
	output [7: 0] GetValue,
	output RawInputs_InOverride,
	output [7: 0] RawInputs_InValue,
	output OverrideInputs_InOverride,
	output [7: 0] OverrideInputs_InValue
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  Inputs_InOverride;
wire  [8:1] Inputs_InValue;
wire  ModulesInputs_InOverride;
wire  [8:1] ModulesInputs_InValue;
wire  [8:1] InvertedInput;
wire  NoOverride_InOverride;
wire  [8:1] NoOverride_InValue;
wire  [8:1] NoOverride_OutValue;
wire  AutoOverride_InOverride;
wire  [8:1] AutoOverride_InValue;
wire  [8:1] AutoOverride_OutValue;
wire  GetOverride_InOverride;
wire  [8:1] GetOverride_InValue;
wire  [8:1] GetOverride_OutValue;
wire  L1Override_InOverride;
wire  [8:1] L1Override_InValue;
wire  [8:1] L1Override_OutValue;
wire  L2Override_InOverride;
wire  [8:1] L2Override_InValue;
wire  [8:1] L2Override_OutValue;
wire  L3Override_InOverride;
wire  [8:1] L3Override_InValue;
wire  [8:1] L3Override_OutValue;
wire  [8:1] OverrideInputsComposition_L7F40T71_Source;
wire  NoOverrideInOverrideNoOverride_InOverrideHardLink;
wire  [8:1] NoOverrideInValueNoOverride_InValueHardLink;
wire  [8:1] NoOverrideOutValueNoOverride_OutValueHardLink;
wire  AutoOverrideInOverrideAutoOverride_InOverrideHardLink;
wire  [8:1] AutoOverrideInValueAutoOverride_InValueHardLink;
wire  [8:1] AutoOverrideOutValueAutoOverride_OutValueHardLink;
wire  GetOverrideInOverrideGetOverride_InOverrideHardLink;
wire  [8:1] GetOverrideInValueGetOverride_InValueHardLink;
wire  [8:1] GetOverrideOutValueGetOverride_OutValueHardLink;
wire  L1OverrideInOverrideL1Override_InOverrideHardLink;
wire  [8:1] L1OverrideInValueL1Override_InValueHardLink;
wire  [8:1] L1OverrideOutValueL1Override_OutValueHardLink;
wire  L2OverrideInOverrideL2Override_InOverrideHardLink;
wire  [8:1] L2OverrideInValueL2Override_InValueHardLink;
wire  [8:1] L2OverrideOutValueL2Override_OutValueHardLink;
wire  L3OverrideInOverrideL3Override_InOverrideHardLink;
wire  [8:1] L3OverrideInValueL3Override_InValueHardLink;
wire  [8:1] L3OverrideOutValueL3Override_OutValueHardLink;
wire  OverrideInputsComposition_L14F30T48_Expr;
wire  OverrideInputsComposition_L14F30T48_Expr_1;
wire  [8:1] OverrideInputsComposition_L7F38T72_Expr;
wire  [8:1] OverrideInputsComposition_L7F38T72_Expr_1;
assign OverrideInputsComposition_L14F30T48_Expr = ~OverrideInputsComposition_L14F30T48_Expr_1;
assign OverrideInputsComposition_L7F38T72_Expr = ~OverrideInputsComposition_L7F38T72_Expr_1;
OverrideInputsComposition_TopLevel_OverrideInputsComposition_NoOverride OverrideInputsComposition_TopLevel_OverrideInputsComposition_NoOverride
(
// [BEGIN USER MAP FOR NoOverride]
// [END USER MAP FOR NoOverride]
	.InOverride (NoOverrideInOverrideNoOverride_InOverrideHardLink),
	.InValue (NoOverrideInValueNoOverride_InValueHardLink),
	.OutValue (NoOverrideOutValueNoOverride_OutValueHardLink)

);
OverrideInputsComposition_TopLevel_OverrideInputsComposition_AutoOverride OverrideInputsComposition_TopLevel_OverrideInputsComposition_AutoOverride
(
// [BEGIN USER MAP FOR AutoOverride]
// [END USER MAP FOR AutoOverride]
	.InOverride (AutoOverrideInOverrideAutoOverride_InOverrideHardLink),
	.InValue (AutoOverrideInValueAutoOverride_InValueHardLink),
	.OutValue (AutoOverrideOutValueAutoOverride_OutValueHardLink)

);
OverrideInputsComposition_TopLevel_OverrideInputsComposition_GetOverride OverrideInputsComposition_TopLevel_OverrideInputsComposition_GetOverride
(
// [BEGIN USER MAP FOR GetOverride]
// [END USER MAP FOR GetOverride]
	.InOverride (GetOverrideInOverrideGetOverride_InOverrideHardLink),
	.InValue (GetOverrideInValueGetOverride_InValueHardLink),
	.OutValue (GetOverrideOutValueGetOverride_OutValueHardLink)

);
OverrideInputsComposition_TopLevel_OverrideInputsComposition_L1Override OverrideInputsComposition_TopLevel_OverrideInputsComposition_L1Override
(
// [BEGIN USER MAP FOR L1Override]
// [END USER MAP FOR L1Override]
	.InOverride (L1OverrideInOverrideL1Override_InOverrideHardLink),
	.InValue (L1OverrideInValueL1Override_InValueHardLink),
	.OutValue (L1OverrideOutValueL1Override_OutValueHardLink)

);
OverrideInputsComposition_TopLevel_OverrideInputsComposition_L2Override OverrideInputsComposition_TopLevel_OverrideInputsComposition_L2Override
(
// [BEGIN USER MAP FOR L2Override]
// [END USER MAP FOR L2Override]
	.InOverride (L2OverrideInOverrideL2Override_InOverrideHardLink),
	.InValue (L2OverrideInValueL2Override_InValueHardLink),
	.OutValue (L2OverrideOutValueL2Override_OutValueHardLink)

);
OverrideInputsComposition_TopLevel_OverrideInputsComposition_L3Override OverrideInputsComposition_TopLevel_OverrideInputsComposition_L3Override
(
// [BEGIN USER MAP FOR L3Override]
// [END USER MAP FOR L3Override]
	.InOverride (L3OverrideInOverrideL3Override_InOverrideHardLink),
	.InValue (L3OverrideInValueL3Override_InValueHardLink),
	.OutValue (L3OverrideOutValueL3Override_OutValueHardLink)

);
assign OverrideInputsComposition_L14F30T48_Expr_1 = Inputs_InOverride;
assign OverrideInputsComposition_L7F38T72_Expr_1 = OverrideInputsComposition_L7F40T71_Source;
assign Inputs_InOverride = InOverride;
assign Inputs_InValue = InValue;
assign ModulesInputs_InOverride = OverrideInputsComposition_L14F30T48_Expr;
assign ModulesInputs_InValue = InvertedInput;
assign OverrideInputsComposition_L7F40T71_Source = Inputs_InValue;
assign InvertedInput = OverrideInputsComposition_L7F38T72_Expr;
assign NoOverride_InOverride = ModulesInputs_InOverride;
assign NoOverride_InValue = ModulesInputs_InValue;
assign AutoOverride_InOverride = ModulesInputs_InOverride;
assign AutoOverride_InValue = ModulesInputs_InValue;
assign L1Override_InOverride = ModulesInputs_InOverride;
assign L1Override_InValue = ModulesInputs_InValue;
assign L2Override_InOverride = ModulesInputs_InOverride;
assign L2Override_InValue = ModulesInputs_InValue;
assign L3Override_InOverride = ModulesInputs_InOverride;
assign L3Override_InValue = ModulesInputs_InValue;
assign GetOverride_InOverride = ModulesInputs_InOverride;
assign GetOverride_InValue = ModulesInputs_InValue;
assign NoOverrideValue = NoOverride_OutValue;
assign AutoOverrideValue = AutoOverride_OutValue;
assign L1Value = L1Override_OutValue;
assign L2Value = L2Override_OutValue;
assign L3Value = L3Override_OutValue;
assign GetValue = GetOverride_OutValue;
assign RawInputs_InOverride = Inputs_InOverride;
assign RawInputs_InValue = Inputs_InValue;
assign OverrideInputs_InOverride = ModulesInputs_InOverride;
assign OverrideInputs_InValue = ModulesInputs_InValue;
assign NoOverrideInOverrideNoOverride_InOverrideHardLink = NoOverride_InOverride;
assign NoOverrideInValueNoOverride_InValueHardLink = NoOverride_InValue;
assign NoOverride_OutValue = NoOverrideOutValueNoOverride_OutValueHardLink;
assign AutoOverrideInOverrideAutoOverride_InOverrideHardLink = AutoOverride_InOverride;
assign AutoOverrideInValueAutoOverride_InValueHardLink = AutoOverride_InValue;
assign AutoOverride_OutValue = AutoOverrideOutValueAutoOverride_OutValueHardLink;
assign GetOverrideInOverrideGetOverride_InOverrideHardLink = GetOverride_InOverride;
assign GetOverrideInValueGetOverride_InValueHardLink = GetOverride_InValue;
assign GetOverride_OutValue = GetOverrideOutValueGetOverride_OutValueHardLink;
assign L1OverrideInOverrideL1Override_InOverrideHardLink = L1Override_InOverride;
assign L1OverrideInValueL1Override_InValueHardLink = L1Override_InValue;
assign L1Override_OutValue = L1OverrideOutValueL1Override_OutValueHardLink;
assign L2OverrideInOverrideL2Override_InOverrideHardLink = L2Override_InOverride;
assign L2OverrideInValueL2Override_InValueHardLink = L2Override_InValue;
assign L2Override_OutValue = L2OverrideOutValueL2Override_OutValueHardLink;
assign L3OverrideInOverrideL3Override_InOverrideHardLink = L3Override_InOverride;
assign L3OverrideInValueL3Override_InValueHardLink = L3Override_InValue;
assign L3Override_OutValue = L3OverrideOutValueL3Override_OutValueHardLink;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
