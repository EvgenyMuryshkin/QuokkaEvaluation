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
// System configuration name is Counter_TopLevel_QuSoCModule_UARTSim, clock frequency is 1Hz, Embedded
// FSM summary
// -- Packages
module Counter_TopLevel_QuSoCModule_UARTSim (
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
	output IsActive,
	output IsReady,
	output [32: 1] ReadValue
    );

// [BEGIN USER SIGNALS]
// [END USER SIGNALS]
localparam HiSignal = 1'b1;
localparam LoSignal = 1'b0;
wire  Zero = 1'b0;
wire  One = 1'b1;
wire  true = 1'b1;
wire  false = 1'b0;
wire  [2:0] addressSpan = 3'b100;
wire  [1:0] SoCComponentModule_L46F83T84_Expr = 2'b11;
wire  [1:0] SoCUARTSimModule_L27F44T45_Expr = 2'b10;
wire  SoCUARTSimModule_L27F50T51_Expr = 1'b0;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L36F33T38_Expr = 1'b0;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L41F32T33_Expr = 1'b0;
wire  [1:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L42F32T33_Expr = 2'b10;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L42F37T38_Expr = 1'b0;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L43F37T41_Expr = 1'b1;
wire  [6:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L46F42T45_Expr = 7'b1100100;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F43T44_Expr = 1'b0;
wire  [1:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L52F36T37_Expr = 2'b10;
wire  [1:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L52F41T42_Expr = 2'b10;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F74T75_Expr = 1'b1;
wire  [1:0] SoCUARTSimModule_L32F24T25_Expr = 2'b11;
wire  [1:0] SoCUARTSimModule_L32F39T40_Expr = 2'b10;
wire  SoCUARTSimModule_L32F54T55_Expr = 1'b1;
wire  SoCUARTSimModule_L32F69T70_Expr = 1'b0;
wire  [31:0] Inputs_Common_Address;
wire  [31:0] Inputs_Common_WriteValue;
wire  Inputs_Common_WE;
wire  Inputs_Common_RE;
wire  [31:0] Inputs_DeviceAddress;
reg  NextState_UART_TX = 1'b0;
reg  [7:0] NextState_txSimCounter = 8'b00000000;
wire  addressMatch;
wire  [31:0] internalAddressBits;
wire  [4:0] internalByteAddress;
wire  internalIsActive;
wire  internalIsReady;
wire  [31:0] SoCComponentModule_L45F54T92_Source;
wire  [1:0] SoCComponentModule_L46F54T79_Index;
wire  [7:0] SoCUARTSimModule_L27F33T46_Index;
wire  [7:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L41F37T67_Cast;
wire  [7:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F46T76_Cast;
reg  [7:0] State_UARTDefault = 8'b00000000;
wire  [31:0] SoCUARTSimModule_L31F43L32T72_Source;
wire  [7:0] SoCUARTSimModule_L32F13T26_Index;
wire  [7:0] SoCUARTSimModule_L32F28T41_Index;
wire  [7:0] SoCUARTSimModule_L32F43T56_Index;
wire  [7:0] SoCUARTSimModule_L32F58T71_Index;
reg  State_UART_TX = 1'b0;
wire  State_UART_TXDefault = 1'b0;
reg  [7:0] State_txSimCounter = 8'b00000000;
wire  [7:0] State_txSimCounterDefault = 8'b00000000;
wire  SoCComponentModule_L44F48T157_Expr;
wire  SoCComponentModule_L44F48T157_Expr_1;
wire  SoCComponentModule_L44F48T157_Expr_2;
wire  [7:0] SoCComponentModule_L46F54T84_Expr;
wire  [7:0] SoCComponentModule_L46F54T84_Expr_1;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr_1;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr_2;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr_1;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr_2;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L49F17T33_Expr;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L49F17T33_Expr_1;
wire  [31:0] SoCUARTSimModule_L31F43L32T95_Expr;
wire  [31:0] SoCUARTSimModule_L31F43L32T95_Expr_1;
wire  [33:0] SoCComponentModule_L44F122T156_Expr;
wire signed  [33:0] SoCComponentModule_L44F122T156_Expr_1;
wire signed  [33:0] SoCComponentModule_L44F122T156_Expr_2;
wire signed  [9:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr;
wire signed  [9:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr_1;
wire signed  [9:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr_2;
wire  SoCComponentModule_L44F48T93_Expr;
wire signed  [32:0] SoCComponentModule_L44F48T93_ExprLhs;
wire signed  [32:0] SoCComponentModule_L44F48T93_ExprRhs;
wire  SoCComponentModule_L44F97T157_Expr;
wire signed  [34:0] SoCComponentModule_L44F97T157_ExprLhs;
wire signed  [34:0] SoCComponentModule_L44F97T157_ExprRhs;
wire  SoCUARTSimModule_L27F33T51_Expr;
wire signed  [8:0] SoCUARTSimModule_L27F33T51_ExprLhs;
wire signed  [8:0] SoCUARTSimModule_L27F33T51_ExprRhs;
wire  SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_Expr;
wire signed  [8:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_ExprLhs;
wire signed  [8:0] SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_ExprRhs;
integer State_UART_Iterator;
reg [7:0] State_UART [0 : 3];
initial
begin
	$readmemh("Counter_TopLevel_QuSoCModule_UARTSim_State_UART.hex", State_UART);
end
integer NextState_UART_Iterator;
reg [7:0] NextState_UART [0 : 3];
initial
begin
	for (NextState_UART_Iterator = 0; NextState_UART_Iterator < 4; NextState_UART_Iterator = NextState_UART_Iterator + 1)
		NextState_UART[NextState_UART_Iterator] = 0;
end
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
State_UART_TX <= State_UART_TXDefault;
State_txSimCounter <= State_txSimCounterDefault;
end
else begin
State_UART_TX <= NextState_UART_TX;
State_txSimCounter <= NextState_txSimCounter;
end
end
always @(posedge BoardSignals_Clock)
begin
if ( BoardSignals_Reset == 1 ) begin
for (State_UART_Iterator = 0; State_UART_Iterator < 4; State_UART_Iterator = State_UART_Iterator + 1)
begin
State_UART[State_UART_Iterator] <= State_UARTDefault;
end
end
else begin
for (State_UART_Iterator = 0; State_UART_Iterator < 4; State_UART_Iterator = State_UART_Iterator + 1)
begin
State_UART[State_UART_Iterator] <= NextState_UART[State_UART_Iterator];
end
end
end
assign SoCComponentModule_L44F48T93_Expr = SoCComponentModule_L44F48T93_ExprLhs >= SoCComponentModule_L44F48T93_ExprRhs ? 1'b1 : 1'b0;
assign SoCComponentModule_L44F97T157_Expr = SoCComponentModule_L44F97T157_ExprLhs < SoCComponentModule_L44F97T157_ExprRhs ? 1'b1 : 1'b0;
assign SoCUARTSimModule_L27F33T51_Expr = SoCUARTSimModule_L27F33T51_ExprLhs != SoCUARTSimModule_L27F33T51_ExprRhs ? 1'b1 : 1'b0;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_Expr = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_ExprLhs == SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_ExprRhs ? 1'b1 : 1'b0;
assign SoCComponentModule_L44F48T157_Expr = SoCComponentModule_L44F48T157_Expr_1 & SoCComponentModule_L44F48T157_Expr_2;
assign SoCComponentModule_L46F54T84_Expr[0] = 0;
assign SoCComponentModule_L46F54T84_Expr[1] = 0;
assign SoCComponentModule_L46F54T84_Expr[2] = 0;
assign SoCComponentModule_L46F54T84_Expr[3] = SoCComponentModule_L46F54T84_Expr_1[0];
assign SoCComponentModule_L46F54T84_Expr[4] = SoCComponentModule_L46F54T84_Expr_1[1];
assign SoCComponentModule_L46F54T84_Expr[5] = 0;
assign SoCComponentModule_L46F54T84_Expr[6] = 0;
assign SoCComponentModule_L46F54T84_Expr[7] = 0;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr_1 & SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr_2;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr_1 & SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr_2;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L49F17T33_Expr = ~SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L49F17T33_Expr_1;
assign SoCUARTSimModule_L31F43L32T95_Expr = SoCUARTSimModule_L31F43L32T95_Expr_1 >> internalByteAddress;
assign SoCComponentModule_L44F122T156_Expr = SoCComponentModule_L44F122T156_Expr_1 + SoCComponentModule_L44F122T156_Expr_2;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr_1 - SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr_2;
always @*
begin
for (NextState_UART_Iterator = 0; NextState_UART_Iterator < 4; NextState_UART_Iterator = NextState_UART_Iterator + 1)
begin
NextState_UART[NextState_UART_Iterator] = State_UART[NextState_UART_Iterator];
end
NextState_UART_TX = State_UART_TX;
NextState_txSimCounter = State_txSimCounter;
NextState_UART_TX = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L36F33T38_Expr;
if ( SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr == 1 ) begin
NextState_UART[SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L41F32T33_Expr] = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L41F37T67_Cast;
NextState_UART[SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L42F32T33_Expr] = { {7{1'b0}}, SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L42F37T38_Expr }/*expand*/;
NextState_UART_TX = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L43F37T41_Expr;
NextState_txSimCounter = { {1{1'b0}}, SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L46F42T45_Expr }/*expand*/;
end
if ( SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L49F17T33_Expr == 1 ) begin
if ( SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_Expr == 1 ) begin
NextState_UART[SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L52F36T37_Expr] = { {6{1'b0}}, SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L52F41T42_Expr }/*expand*/;
end
else begin
NextState_txSimCounter = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F46T76_Cast;
end
end

end
assign SoCComponentModule_L44F48T93_ExprLhs = { {1{1'b0}}, Inputs_Common_Address }/*expand*/;
assign SoCComponentModule_L44F48T93_ExprRhs = { {1{1'b0}}, Inputs_DeviceAddress }/*expand*/;
assign SoCComponentModule_L44F97T157_ExprLhs = { {3{1'b0}}, Inputs_Common_Address }/*expand*/;
assign SoCComponentModule_L44F97T157_ExprRhs = { {1{1'b0}}, SoCComponentModule_L44F122T156_Expr }/*expand*/;
assign SoCUARTSimModule_L27F33T51_ExprLhs = { {1{1'b0}}, SoCUARTSimModule_L27F33T46_Index }/*expand*/;
assign SoCUARTSimModule_L27F33T51_ExprRhs = { {8{1'b0}}, SoCUARTSimModule_L27F50T51_Expr }/*expand*/;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_ExprLhs = { {1{1'b0}}, State_txSimCounter }/*expand*/;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F21T44_ExprRhs = { {8{1'b0}}, SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L51F43T44_Expr }/*expand*/;
assign SoCComponentModule_L44F48T157_Expr_1 = SoCComponentModule_L44F48T93_Expr;
assign SoCComponentModule_L44F48T157_Expr_2 = SoCComponentModule_L44F97T157_Expr;
assign SoCComponentModule_L46F54T84_Expr_1 = { {6{1'b0}}, SoCComponentModule_L46F54T79_Index }/*expand*/;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr_1 = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T72_Expr_2 = Inputs_Common_WE;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr_1 = internalIsReady;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L38F17T52_Expr_2 = internalIsActive;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L49F17T33_Expr_1 = internalIsReady;
assign SoCUARTSimModule_L31F43L32T95_Expr_1 = SoCUARTSimModule_L31F43L32T72_Source;
assign SoCComponentModule_L44F122T156_Expr_1 = { {2{1'b0}}, Inputs_DeviceAddress }/*expand*/;
assign SoCComponentModule_L44F122T156_Expr_2 = { {31{1'b0}}, addressSpan }/*expand*/;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr_1 = { {2{1'b0}}, State_txSimCounter }/*expand*/;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr_2 = { {9{1'b0}}, SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F74T75_Expr }/*expand*/;
assign Inputs_Common_Address = Common_Address;
assign Inputs_Common_WriteValue = Common_WriteValue;
assign Inputs_Common_WE = Common_WE;
assign Inputs_Common_RE = Common_RE;
assign Inputs_DeviceAddress = DeviceAddress;
assign addressMatch = SoCComponentModule_L44F48T157_Expr;
assign SoCComponentModule_L45F54T92_Source = Inputs_Common_Address;
assign internalAddressBits = SoCComponentModule_L45F54T92_Source;
assign SoCComponentModule_L46F54T79_Index = internalAddressBits[1:0];
assign internalByteAddress = SoCComponentModule_L46F54T84_Expr[4:0]/*truncate*/;
assign internalIsActive = addressMatch;
assign internalIsReady = SoCUARTSimModule_L27F33T51_Expr;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L39F13L47T14_SoCUARTSimModule_L41F37T67_Cast = Inputs_Common_WriteValue[7:0]/*truncate*/;
assign SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F46T76_Cast = SoCUARTSimModule_L35F9L56T10_SoCUARTSimModule_L50F13L55T14_SoCUARTSimModule_L54F53T75_Expr[7:0]/*truncate*/;
assign IsActive = internalIsActive;
assign IsReady = internalIsReady;
assign SoCUARTSimModule_L31F43L32T72_Source[0] = SoCUARTSimModule_L32F58T71_Index[0];
assign SoCUARTSimModule_L31F43L32T72_Source[1] = SoCUARTSimModule_L32F58T71_Index[1];
assign SoCUARTSimModule_L31F43L32T72_Source[2] = SoCUARTSimModule_L32F58T71_Index[2];
assign SoCUARTSimModule_L31F43L32T72_Source[3] = SoCUARTSimModule_L32F58T71_Index[3];
assign SoCUARTSimModule_L31F43L32T72_Source[4] = SoCUARTSimModule_L32F58T71_Index[4];
assign SoCUARTSimModule_L31F43L32T72_Source[5] = SoCUARTSimModule_L32F58T71_Index[5];
assign SoCUARTSimModule_L31F43L32T72_Source[6] = SoCUARTSimModule_L32F58T71_Index[6];
assign SoCUARTSimModule_L31F43L32T72_Source[7] = SoCUARTSimModule_L32F58T71_Index[7];
assign SoCUARTSimModule_L31F43L32T72_Source[8] = SoCUARTSimModule_L32F43T56_Index[0];
assign SoCUARTSimModule_L31F43L32T72_Source[9] = SoCUARTSimModule_L32F43T56_Index[1];
assign SoCUARTSimModule_L31F43L32T72_Source[10] = SoCUARTSimModule_L32F43T56_Index[2];
assign SoCUARTSimModule_L31F43L32T72_Source[11] = SoCUARTSimModule_L32F43T56_Index[3];
assign SoCUARTSimModule_L31F43L32T72_Source[12] = SoCUARTSimModule_L32F43T56_Index[4];
assign SoCUARTSimModule_L31F43L32T72_Source[13] = SoCUARTSimModule_L32F43T56_Index[5];
assign SoCUARTSimModule_L31F43L32T72_Source[14] = SoCUARTSimModule_L32F43T56_Index[6];
assign SoCUARTSimModule_L31F43L32T72_Source[15] = SoCUARTSimModule_L32F43T56_Index[7];
assign SoCUARTSimModule_L31F43L32T72_Source[16] = SoCUARTSimModule_L32F28T41_Index[0];
assign SoCUARTSimModule_L31F43L32T72_Source[17] = SoCUARTSimModule_L32F28T41_Index[1];
assign SoCUARTSimModule_L31F43L32T72_Source[18] = SoCUARTSimModule_L32F28T41_Index[2];
assign SoCUARTSimModule_L31F43L32T72_Source[19] = SoCUARTSimModule_L32F28T41_Index[3];
assign SoCUARTSimModule_L31F43L32T72_Source[20] = SoCUARTSimModule_L32F28T41_Index[4];
assign SoCUARTSimModule_L31F43L32T72_Source[21] = SoCUARTSimModule_L32F28T41_Index[5];
assign SoCUARTSimModule_L31F43L32T72_Source[22] = SoCUARTSimModule_L32F28T41_Index[6];
assign SoCUARTSimModule_L31F43L32T72_Source[23] = SoCUARTSimModule_L32F28T41_Index[7];
assign SoCUARTSimModule_L31F43L32T72_Source[24] = SoCUARTSimModule_L32F13T26_Index[0];
assign SoCUARTSimModule_L31F43L32T72_Source[25] = SoCUARTSimModule_L32F13T26_Index[1];
assign SoCUARTSimModule_L31F43L32T72_Source[26] = SoCUARTSimModule_L32F13T26_Index[2];
assign SoCUARTSimModule_L31F43L32T72_Source[27] = SoCUARTSimModule_L32F13T26_Index[3];
assign SoCUARTSimModule_L31F43L32T72_Source[28] = SoCUARTSimModule_L32F13T26_Index[4];
assign SoCUARTSimModule_L31F43L32T72_Source[29] = SoCUARTSimModule_L32F13T26_Index[5];
assign SoCUARTSimModule_L31F43L32T72_Source[30] = SoCUARTSimModule_L32F13T26_Index[6];
assign SoCUARTSimModule_L31F43L32T72_Source[31] = SoCUARTSimModule_L32F13T26_Index[7];
assign ReadValue = SoCUARTSimModule_L31F43L32T95_Expr;
assign SoCUARTSimModule_L27F33T46_Index = State_UART[SoCUARTSimModule_L27F44T45_Expr];
assign SoCUARTSimModule_L32F13T26_Index = State_UART[SoCUARTSimModule_L32F24T25_Expr];
assign SoCUARTSimModule_L32F28T41_Index = State_UART[SoCUARTSimModule_L32F39T40_Expr];
assign SoCUARTSimModule_L32F43T56_Index = State_UART[SoCUARTSimModule_L32F54T55_Expr];
assign SoCUARTSimModule_L32F58T71_Index = State_UART[SoCUARTSimModule_L32F69T70_Expr];
// [BEGIN USER ARCHITECTURE]
// [END USER ARCHITECTURE]
endmodule
