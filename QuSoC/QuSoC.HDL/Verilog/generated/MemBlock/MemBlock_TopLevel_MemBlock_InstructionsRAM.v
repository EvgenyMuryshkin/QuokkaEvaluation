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
// System configuration name is MemBlock_TopLevel_MemBlock_InstructionsRAM, clock frequency is 1Hz, Embedded
// FSM summary
// -- Packages
module MemBlock_TopLevel_MemBlock_InstructionsRAM (
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  BoardSignals_Clock,
	input  BoardSignals_Reset,
	input  BoardSignals_Running,
	input  BoardSignals_Starting,
	input  BoardSignals_Started,
	input  [32: 1] Common_Address,
	input  [32: 1] Common_WriteValue,
	input  Common_WE,
	input  Common_RE,
	input  [32: 1] DeviceAddress,
	input  [2: 1] MemAccessMode,
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
wire  [12:0] addressSpan = 13'b1000000000000;
wire  [1:0] SoCComponentModule_L46F83T84_Expr = 2'b11;
wire  [1:0] SoCBlockRAMModule_L38F37T38_Expr = 2'b10;
wire  [1:0] SoCBlockRAMModule_L42F57T58_Expr = 2'b10;
wire  [31:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L50F44T57_Expr = 32'b11111111111111111111111111111111;
wire  SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F45T46_Expr = 1'b0;
wire  [7:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F44T57_Expr = 8'b11111111;
wire  SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F50T51_Expr = 1'b1;
wire  [15:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F44T59_Expr = 16'b1111111111111111;
wire  [31:0] Inputs_Common_Address;
wire  [31:0] Inputs_Common_WriteValue;
wire  Inputs_Common_WE;
wire  Inputs_Common_RE;
wire  [31:0] Inputs_DeviceAddress;
wire  [1:0] Inputs_MemAccessMode;
reg  NextState_Ready = 1'b0;
reg  NextState_ReadBeforeWrite = 1'b0;
wire  addressMatch;
wire  [31:0] internalAddressBits;
wire  [4:0] internalByteAddress;
wire  [9:0] internalWordAddress;
wire  internalIsActive;
wire  [31:0] internalWriteValueBits;
wire  [31:0] writeMask;
wire  [31:0] internalWriteData;
wire  readBeforeWrite;
wire  internalWE;
wire  [31:0] memAccessMask;
wire  [31:0] SoCComponentModule_L45F54T92_Source;
wire  [1:0] SoCComponentModule_L46F54T79_Index;
wire  [9:0] SoCBlockRAMModule_L28F44T70_Index;
wire  [31:0] SoCBlockRAMModule_L36F34T84_Resize;
wire  [31:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L50F28T58_Source;
reg  [31:0] SoCBlockRAMModule_L49F13L58T14_mask = 32'b00000000000000000000000000000000;
wire  [7:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F28T58_Source;
wire  [31:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F28T70_Resize;
wire  [15:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F28T60_Source;
wire  [31:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F28T72_Resize;
reg  [31:0] State_ReadValue = 32'b00000000000000000000000000000000;
reg  State_Ready = 1'b0;
wire  State_ReadyDefault = 1'b0;
reg  State_ReadBeforeWrite = 1'b0;
wire  State_ReadBeforeWriteDefault = 1'b0;
wire  SoCComponentModule_L44F48T157_Expr;
wire  SoCComponentModule_L44F48T157_Expr_1;
wire  SoCComponentModule_L44F48T157_Expr_2;
wire  [7:0] SoCComponentModule_L46F54T84_Expr;
wire  [7:0] SoCComponentModule_L46F54T84_Expr_1;
wire  SoCBlockRAMModule_L29F34T88_Expr;
wire  SoCBlockRAMModule_L29F34T88_Expr_1;
wire  SoCBlockRAMModule_L29F34T88_Expr_2;
wire  SoCBlockRAMModule_L29F35T71_Expr;
wire  SoCBlockRAMModule_L29F35T71_Expr_1;
wire  SoCBlockRAMModule_L29F35T71_Expr_2;
wire  [31:0] SoCBlockRAMModule_L36F35T71_Expr;
wire  [31:0] SoCBlockRAMModule_L36F35T71_Expr_1;
wire  [31:0] SoCBlockRAMModule_L40F15T109_Expr;
wire  [31:0] SoCBlockRAMModule_L40F15T109_Expr_1;
wire  [31:0] SoCBlockRAMModule_L40F15T109_Expr_2;
wire  [31:0] SoCBlockRAMModule_L40F16T44_Expr;
wire  [31:0] SoCBlockRAMModule_L40F16T44_Expr_1;
wire  [31:0] SoCBlockRAMModule_L40F16T44_Expr_2;
wire  [31:0] SoCBlockRAMModule_L40F34T44_Expr;
wire  [31:0] SoCBlockRAMModule_L40F34T44_Expr_1;
wire  [31:0] SoCBlockRAMModule_L40F49T108_Expr;
wire  [31:0] SoCBlockRAMModule_L40F49T108_Expr_1;
wire  [31:0] SoCBlockRAMModule_L40F49T108_Expr_2;
wire  [31:0] SoCBlockRAMModule_L40F50T95_Expr;
wire  [31:0] SoCBlockRAMModule_L40F50T95_Expr_1;
wire  SoCBlockRAMModule_L44F28T69_Expr;
wire  SoCBlockRAMModule_L44F28T69_Expr_1;
wire  SoCBlockRAMModule_L44F28T69_Expr_2;
wire  SoCBlockRAMModule_L44F28T44_Expr;
wire  SoCBlockRAMModule_L44F28T44_Expr_1;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr_1;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr_2;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr_1;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr_2;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr_1;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr_2;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F60T82_Expr;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F60T82_Expr_1;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr_1;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr_2;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr_1;
wire  SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr_2;
wire  [31:0] SoCBlockRAMModule_L31F43T99_Expr;
wire  [31:0] SoCBlockRAMModule_L31F43T99_Expr_1;
wire  [31:0] SoCBlockRAMModule_L31F43T99_Expr_2;
wire  [31:0] SoCBlockRAMModule_L31F44T82_Expr;
wire  [31:0] SoCBlockRAMModule_L31F44T82_Expr_1;
wire  [33:0] SoCComponentModule_L44F122T156_Expr;
wire signed  [33:0] SoCComponentModule_L44F122T156_Expr_1;
wire signed  [33:0] SoCComponentModule_L44F122T156_Expr_2;
wire  SoCComponentModule_L44F48T93_Expr;
wire signed  [32:0] SoCComponentModule_L44F48T93_ExprLhs;
wire signed  [32:0] SoCComponentModule_L44F48T93_ExprRhs;
wire  SoCComponentModule_L44F97T157_Expr;
wire signed  [34:0] SoCComponentModule_L44F97T157_ExprLhs;
wire signed  [34:0] SoCComponentModule_L44F97T157_ExprRhs;
wire  SoCBlockRAMModule_L38F13T38_Expr;
wire signed  [2:0] SoCBlockRAMModule_L38F13T38_ExprLhs;
wire signed  [2:0] SoCBlockRAMModule_L38F13T38_ExprRhs;
wire  SoCBlockRAMModule_L42F33T58_Expr;
wire signed  [2:0] SoCBlockRAMModule_L42F33T58_ExprLhs;
wire signed  [2:0] SoCBlockRAMModule_L42F33T58_ExprRhs;
wire  SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_Expr;
wire signed  [2:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_ExprLhs;
wire signed  [2:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_ExprRhs;
wire  SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_Expr;
wire signed  [2:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_ExprLhs;
wire signed  [2:0] SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_ExprRhs;
reg  [31:0] SoCBlockRAMModule_L38F13L40T109_Lookup = 32'b00000000000000000000000000000000;
wire  SoCBlockRAMModule_L38F13L40T109_LookupMultiplexerAddress;
wire  [31:0] SoCBlockRAMModule_L38F13L40T109_Lookup1;
wire  [31:0] SoCBlockRAMModule_L38F13L40T109_Lookup2;
reg [31:0] State_BlockRAM [0 : 1023];
initial
begin
	$readmemh("MemBlock_TopLevel_MemBlock_InstructionsRAM_State_BlockRAM.hex", State_BlockRAM);
end
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
State_Ready <= State_ReadyDefault;
State_ReadBeforeWrite <= State_ReadBeforeWriteDefault;
end
else begin
State_Ready <= NextState_Ready;
State_ReadBeforeWrite <= NextState_ReadBeforeWrite;
end
end
assign SoCComponentModule_L44F48T93_Expr = SoCComponentModule_L44F48T93_ExprLhs >= SoCComponentModule_L44F48T93_ExprRhs ? 1'b1 : 1'b0;
assign SoCComponentModule_L44F97T157_Expr = SoCComponentModule_L44F97T157_ExprLhs < SoCComponentModule_L44F97T157_ExprRhs ? 1'b1 : 1'b0;
assign SoCBlockRAMModule_L38F13T38_Expr = SoCBlockRAMModule_L38F13T38_ExprLhs == SoCBlockRAMModule_L38F13T38_ExprRhs ? 1'b1 : 1'b0;
assign SoCBlockRAMModule_L42F33T58_Expr = SoCBlockRAMModule_L42F33T58_ExprLhs != SoCBlockRAMModule_L42F33T58_ExprRhs ? 1'b1 : 1'b0;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_Expr = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_ExprLhs == SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_ExprRhs ? 1'b1 : 1'b0;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_Expr = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_ExprLhs == SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_ExprRhs ? 1'b1 : 1'b0;
assign SoCComponentModule_L44F48T157_Expr = SoCComponentModule_L44F48T157_Expr_1 & SoCComponentModule_L44F48T157_Expr_2;
assign SoCComponentModule_L46F54T84_Expr[0] = 0;
assign SoCComponentModule_L46F54T84_Expr[1] = 0;
assign SoCComponentModule_L46F54T84_Expr[2] = 0;
assign SoCComponentModule_L46F54T84_Expr[3] = SoCComponentModule_L46F54T84_Expr_1[0];
assign SoCComponentModule_L46F54T84_Expr[4] = SoCComponentModule_L46F54T84_Expr_1[1];
assign SoCComponentModule_L46F54T84_Expr[5] = 0;
assign SoCComponentModule_L46F54T84_Expr[6] = 0;
assign SoCComponentModule_L46F54T84_Expr[7] = 0;
assign SoCBlockRAMModule_L29F34T88_Expr = SoCBlockRAMModule_L29F34T88_Expr_1 & SoCBlockRAMModule_L29F34T88_Expr_2;
assign SoCBlockRAMModule_L29F35T71_Expr = SoCBlockRAMModule_L29F35T71_Expr_1 | SoCBlockRAMModule_L29F35T71_Expr_2;
assign SoCBlockRAMModule_L36F35T71_Expr = SoCBlockRAMModule_L36F35T71_Expr_1 << internalByteAddress;
assign SoCBlockRAMModule_L40F15T109_Expr = SoCBlockRAMModule_L40F15T109_Expr_1 | SoCBlockRAMModule_L40F15T109_Expr_2;
assign SoCBlockRAMModule_L40F16T44_Expr = SoCBlockRAMModule_L40F16T44_Expr_1 & SoCBlockRAMModule_L40F16T44_Expr_2;
assign SoCBlockRAMModule_L40F34T44_Expr = ~SoCBlockRAMModule_L40F34T44_Expr_1;
assign SoCBlockRAMModule_L40F49T108_Expr = SoCBlockRAMModule_L40F49T108_Expr_1 & SoCBlockRAMModule_L40F49T108_Expr_2;
assign SoCBlockRAMModule_L40F50T95_Expr = SoCBlockRAMModule_L40F50T95_Expr_1 << internalByteAddress;
assign SoCBlockRAMModule_L44F28T69_Expr = SoCBlockRAMModule_L44F28T69_Expr_1 | SoCBlockRAMModule_L44F28T69_Expr_2;
assign SoCBlockRAMModule_L44F28T44_Expr = ~SoCBlockRAMModule_L44F28T44_Expr_1;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr_1 & SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr_2;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr_1 | SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr_2;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr_1 & SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr_2;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F60T82_Expr = ~SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F60T82_Expr_1;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr_1 & SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr_2;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr_1 & SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr_2;
assign SoCBlockRAMModule_L31F43T99_Expr = SoCBlockRAMModule_L31F43T99_Expr_1 & SoCBlockRAMModule_L31F43T99_Expr_2;
assign SoCBlockRAMModule_L31F44T82_Expr = SoCBlockRAMModule_L31F44T82_Expr_1 >> internalByteAddress;
assign SoCComponentModule_L44F122T156_Expr = SoCComponentModule_L44F122T156_Expr_1 + SoCComponentModule_L44F122T156_Expr_2;
always @*
begin
case (SoCBlockRAMModule_L38F13L40T109_LookupMultiplexerAddress)
    'b0:
SoCBlockRAMModule_L38F13L40T109_Lookup = SoCBlockRAMModule_L38F13L40T109_Lookup1;
    'b1:
SoCBlockRAMModule_L38F13L40T109_Lookup = SoCBlockRAMModule_L38F13L40T109_Lookup2;
  default:
SoCBlockRAMModule_L38F13L40T109_Lookup = 'b00000000000000000000000000000000;
endcase

end
always @*
begin
SoCBlockRAMModule_L49F13L58T14_mask = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L50F28T58_Source;
if ( SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_Expr == 1 ) begin
SoCBlockRAMModule_L49F13L58T14_mask = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F28T70_Resize;
end
else if ( SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_Expr == 1 ) begin
SoCBlockRAMModule_L49F13L58T14_mask = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F28T72_Resize;
end

end
always @*
begin
NextState_Ready = State_Ready;
NextState_ReadBeforeWrite = State_ReadBeforeWrite;
NextState_Ready = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr;
NextState_ReadBeforeWrite = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr;
if ( SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr == 1 ) begin
end

end
// inferred simple dual port RAM with read-first behaviour
always @ (posedge BoardSignals_Clock)
begin
	if (SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr)
		State_BlockRAM[internalWordAddress] <= internalWriteData;

	State_ReadValue <= State_BlockRAM[internalWordAddress];
end

assign SoCComponentModule_L44F48T93_ExprLhs = { {1{1'b0}}, Inputs_Common_Address }/*expand*/;
assign SoCComponentModule_L44F48T93_ExprRhs = { {1{1'b0}}, Inputs_DeviceAddress }/*expand*/;
assign SoCComponentModule_L44F97T157_ExprLhs = { {3{1'b0}}, Inputs_Common_Address }/*expand*/;
assign SoCComponentModule_L44F97T157_ExprRhs = { {1{1'b0}}, SoCComponentModule_L44F122T156_Expr }/*expand*/;
assign SoCBlockRAMModule_L38F13T38_ExprLhs = { {1{1'b0}}, Inputs_MemAccessMode }/*expand*/;
assign SoCBlockRAMModule_L38F13T38_ExprRhs = { {1{1'b0}}, SoCBlockRAMModule_L38F37T38_Expr }/*expand*/;
assign SoCBlockRAMModule_L42F33T58_ExprLhs = { {1{1'b0}}, Inputs_MemAccessMode }/*expand*/;
assign SoCBlockRAMModule_L42F33T58_ExprRhs = { {1{1'b0}}, SoCBlockRAMModule_L42F57T58_Expr }/*expand*/;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_ExprLhs = { {1{1'b0}}, Inputs_MemAccessMode }/*expand*/;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F21T46_ExprRhs = { {2{1'b0}}, SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L52F45T46_Expr }/*expand*/;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_ExprLhs = { {1{1'b0}}, Inputs_MemAccessMode }/*expand*/;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F26T51_ExprRhs = { {2{1'b0}}, SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L54F50T51_Expr }/*expand*/;
assign SoCComponentModule_L44F48T157_Expr_1 = SoCComponentModule_L44F48T93_Expr;
assign SoCComponentModule_L44F48T157_Expr_2 = SoCComponentModule_L44F97T157_Expr;
assign SoCComponentModule_L46F54T84_Expr_1 = { {6{1'b0}}, SoCComponentModule_L46F54T79_Index }/*expand*/;
assign SoCBlockRAMModule_L29F34T88_Expr_1 = SoCBlockRAMModule_L29F35T71_Expr;
assign SoCBlockRAMModule_L29F34T88_Expr_2 = addressMatch;
assign SoCBlockRAMModule_L29F35T71_Expr_1 = Inputs_Common_RE;
assign SoCBlockRAMModule_L29F35T71_Expr_2 = Inputs_Common_WE;
assign SoCBlockRAMModule_L36F35T71_Expr_1 = memAccessMask;
assign SoCBlockRAMModule_L40F15T109_Expr_1 = SoCBlockRAMModule_L40F16T44_Expr;
assign SoCBlockRAMModule_L40F15T109_Expr_2 = SoCBlockRAMModule_L40F49T108_Expr;
assign SoCBlockRAMModule_L40F16T44_Expr_1 = State_ReadValue;
assign SoCBlockRAMModule_L40F16T44_Expr_2 = SoCBlockRAMModule_L40F34T44_Expr;
assign SoCBlockRAMModule_L40F34T44_Expr_1 = writeMask;
assign SoCBlockRAMModule_L40F49T108_Expr_1 = SoCBlockRAMModule_L40F50T95_Expr;
assign SoCBlockRAMModule_L40F49T108_Expr_2 = writeMask;
assign SoCBlockRAMModule_L40F50T95_Expr_1 = internalWriteValueBits;
assign SoCBlockRAMModule_L44F28T69_Expr_1 = SoCBlockRAMModule_L44F28T44_Expr;
assign SoCBlockRAMModule_L44F28T69_Expr_2 = State_ReadBeforeWrite;
assign SoCBlockRAMModule_L44F28T44_Expr_1 = readBeforeWrite;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr_1 = internalIsActive;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F31T83_Expr_2 = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr_1 = internalWE;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L63F52T82_Expr_2 = Inputs_Common_RE;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr_1 = readBeforeWrite;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F41T82_Expr_2 = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F60T82_Expr;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L64F60T82_Expr_1 = State_ReadBeforeWrite;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr_1 = SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T67_Expr_2 = internalWE;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr_1 = Inputs_Common_WE;
assign SoCBlockRAMModule_L62F9L72T10_SoCBlockRAMModule_L66F17T53_Expr_2 = internalIsActive;
assign SoCBlockRAMModule_L31F43T99_Expr_1 = SoCBlockRAMModule_L31F44T82_Expr;
assign SoCBlockRAMModule_L31F43T99_Expr_2 = memAccessMask;
assign SoCBlockRAMModule_L31F44T82_Expr_1 = State_ReadValue;
assign SoCComponentModule_L44F122T156_Expr_1 = { {2{1'b0}}, Inputs_DeviceAddress }/*expand*/;
assign SoCComponentModule_L44F122T156_Expr_2 = { {21{1'b0}}, addressSpan }/*expand*/;
assign Inputs_Common_Address = Common_Address;
assign Inputs_Common_WriteValue = Common_WriteValue;
assign Inputs_Common_WE = Common_WE;
assign Inputs_Common_RE = Common_RE;
assign Inputs_DeviceAddress = DeviceAddress;
assign Inputs_MemAccessMode = MemAccessMode;
assign addressMatch = SoCComponentModule_L44F48T157_Expr;
assign SoCComponentModule_L45F54T92_Source = Inputs_Common_Address;
assign internalAddressBits = SoCComponentModule_L45F54T92_Source;
assign SoCComponentModule_L46F54T79_Index = internalAddressBits[1:0];
assign internalByteAddress = SoCComponentModule_L46F54T84_Expr[4:0]/*truncate*/;
assign SoCBlockRAMModule_L28F44T70_Index = internalAddressBits[11:2];
assign internalWordAddress = SoCBlockRAMModule_L28F44T70_Index;
assign internalIsActive = SoCBlockRAMModule_L29F34T88_Expr;
assign internalWriteValueBits = Inputs_Common_WriteValue;
assign SoCBlockRAMModule_L36F34T84_Resize = SoCBlockRAMModule_L36F35T71_Expr;
assign writeMask = SoCBlockRAMModule_L36F34T84_Resize;
assign internalWriteData = SoCBlockRAMModule_L38F13L40T109_Lookup;
assign readBeforeWrite = SoCBlockRAMModule_L42F33T58_Expr;
assign internalWE = SoCBlockRAMModule_L44F28T69_Expr;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L50F28T58_Source = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L50F44T57_Expr;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F28T58_Source = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F44T57_Expr;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F28T70_Resize = { {24{1'b0}}, SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L53F28T58_Source }/*expand*/;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F28T60_Source = SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F44T59_Expr;
assign SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F28T72_Resize = { {16{1'b0}}, SoCBlockRAMModule_L49F13L58T14_SoCBlockRAMModule_L55F28T60_Source }/*expand*/;
assign memAccessMask = SoCBlockRAMModule_L49F13L58T14_mask;
assign ReadValue = SoCBlockRAMModule_L31F43T99_Expr;
assign IsReady = State_Ready;
assign IsActive = internalIsActive;
assign SoCBlockRAMModule_L38F13L40T109_Lookup1 = SoCBlockRAMModule_L40F15T109_Expr;
assign SoCBlockRAMModule_L38F13L40T109_Lookup2 = internalWriteValueBits;
assign SoCBlockRAMModule_L38F13L40T109_LookupMultiplexerAddress = SoCBlockRAMModule_L38F13T38_Expr;
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
