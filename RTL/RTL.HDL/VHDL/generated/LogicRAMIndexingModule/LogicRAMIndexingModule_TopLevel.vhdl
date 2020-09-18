-- PLEASE READ THIS, IT MAY SAVE YOU SOME TIME AND MONEY, THANK YOU!
-- * This file was generated by Quokka FPGA Toolkit.
-- * Generated code is your property, do whatever you want with it
-- * Place custom code between [BEGIN USER ***] and [END USER ***].
-- * CAUTION: All code outside of [USER] scope is subject to regeneration.
-- * Bad things happen sometimes in developer's life,
--   it is recommended to use source control management software (e.g. git, bzr etc) to keep your custom code safe'n'sound.
-- * Internal structure of code is subject to change.
--   You can use some of signals in custom code, but most likely they will not exist in future (e.g. will get shorter or gone completely)
-- * Please send your feedback, comments, improvement ideas etc. to evmuryshkin@gmail.com
-- * Visit https://github.com/EvgenyMuryshkin/QuokkaEvaluation to access latest version of playground
-- 
-- DISCLAIMER:
--   Code comes AS-IS, it is your responsibility to make sure it is working as expected
--   no responsibility will be taken for any loss or damage caused by use of Quokka toolkit.
-- 
-- System configuration name is LogicRAMIndexingModule_TopLevel, clock frequency is 1Hz, Top-level
library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

use work.Quokka.all;

entity LogicRAMIndexingModule_TopLevel is
    port
    (
-- [BEGIN USER PORTS]
-- [END USER PORTS]

Clock : in  std_logic;
Reset : in  std_logic;
WE : in  std_logic;
WriteAddr : in  unsigned(1 downto 0);
WriteData : in  unsigned(7 downto 0);
ReadAddr : in  unsigned(1 downto 0);
OpData : in  unsigned(7 downto 0);
MemLhsRhs : out  unsigned(7 downto 0);
MathMemLhs : out  unsigned(7 downto 0);
MathMemRhs : out  unsigned(7 downto 0);
LogicMemLhs : out  unsigned(7 downto 0);
LogicMemRhs : out  unsigned(7 downto 0);
CmpMemLhs : out  std_logic;
CmpMemRhs : out  std_logic
    );
end entity;

-- FSM summary
-- Packages
architecture rtl of LogicRAMIndexingModule_TopLevel is
-- [BEGIN USER SIGNALS]
-- [END USER SIGNALS]
constant HiSignal : std_logic := '1';
constant LoSignal : std_logic := '0';
constant Zero : std_logic := '0';
constant One : std_logic := '1';
constant true : std_logic := '1';
constant false : std_logic := '0';
constant LogicRAMIndexingModule_L26F52T53_Expr : std_logic := '1';
signal Inputs_WE : std_logic := '0';
signal Inputs_WriteAddr : unsigned(1 downto 0)  := "00";
signal Inputs_WriteData : unsigned(7 downto 0)  := "00000000";
signal Inputs_ReadAddr : unsigned(1 downto 0)  := "00";
signal Inputs_OpData : unsigned(7 downto 0)  := "00000000";
signal State_BuffDefault : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L26F41T54_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L26F57T84_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L26F34T85_Cast : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L27F42T69_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L27F35T86_Cast : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L28F58T85_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L28F35T86_Cast : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L30F43T70_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L30F36T87_Cast : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L31F59T86_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L31F36T87_Cast : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L33F34T61_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L34F50T77_Index : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L30F43T86_Expr : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L30F43T86_Expr_1 : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L30F43T86_Expr_2 : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L31F43T86_Expr : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L31F43T86_Expr_1 : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L31F43T86_Expr_2 : unsigned(7 downto 0)  := "00000000";
signal LogicRAMIndexingModule_L26F41T84_Expr : unsigned(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L26F41T84_Expr_1 : signed(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L26F41T84_Expr_2 : signed(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L27F42T85_Expr : signed(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L27F42T85_Expr_1 : signed(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L27F42T85_Expr_2 : signed(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L28F42T85_Expr : unsigned(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L28F42T85_Expr_1 : signed(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L28F42T85_Expr_2 : signed(9 downto 0)  := "0000000000";
signal LogicRAMIndexingModule_L33F34T77_Expr : std_logic := '0';
signal LogicRAMIndexingModule_L33F34T77_ExprLhs : signed(8 downto 0)  := "000000000";
signal LogicRAMIndexingModule_L33F34T77_ExprRhs : signed(8 downto 0)  := "000000000";
signal LogicRAMIndexingModule_L34F34T77_Expr : std_logic := '0';
signal LogicRAMIndexingModule_L34F34T77_ExprLhs : signed(8 downto 0)  := "000000000";
signal LogicRAMIndexingModule_L34F34T77_ExprRhs : signed(8 downto 0)  := "000000000";
type State_BuffArray is array(0 to 3) of unsigned(7 downto 0);
signal State_Buff : State_BuffArray := (others => (others => '0'));
type NextState_BuffArray is array(0 to 3) of unsigned(7 downto 0);
signal NextState_Buff : NextState_BuffArray := (others => (others => '0'));
begin
process (Clock, NextState_Buff, Reset, State_BuffDefault)
begin
if rising_edge(Clock) then
if ( Reset = '1' ) then
for State_Buff_Iterator in 0 to 3 loop
State_Buff(State_Buff_Iterator) <= State_BuffDefault;
end loop;
else
for State_Buff_Iterator in 0 to 3 loop
State_Buff(State_Buff_Iterator) <= NextState_Buff(State_Buff_Iterator);
end loop;
end if;
end if;
end process;
    LogicRAMIndexingModule_L33F34T77_Expr <= '1' when (signed(resize(LogicRAMIndexingModule_L33F34T77_ExprLhs, LogicRAMIndexingModule_L33F34T77_ExprLhs'length + 1)) > signed(resize(LogicRAMIndexingModule_L33F34T77_ExprRhs, LogicRAMIndexingModule_L33F34T77_ExprRhs'length + 1))) else '0';
    LogicRAMIndexingModule_L34F34T77_Expr <= '1' when (signed(resize(LogicRAMIndexingModule_L34F34T77_ExprLhs, LogicRAMIndexingModule_L34F34T77_ExprLhs'length + 1)) > signed(resize(LogicRAMIndexingModule_L34F34T77_ExprRhs, LogicRAMIndexingModule_L34F34T77_ExprRhs'length + 1))) else '0';

process(LogicRAMIndexingModule_L30F43T86_Expr_1, LogicRAMIndexingModule_L30F43T86_Expr_2)
begin

    for i in 7 downto 0 loop
        LogicRAMIndexingModule_L30F43T86_Expr(i) <= LogicRAMIndexingModule_L30F43T86_Expr_1(i)  OR LogicRAMIndexingModule_L30F43T86_Expr_2(i)    ;
    end loop;

    end process;

process(LogicRAMIndexingModule_L31F43T86_Expr_1, LogicRAMIndexingModule_L31F43T86_Expr_2)
begin

    for i in 7 downto 0 loop
        LogicRAMIndexingModule_L31F43T86_Expr(i) <= LogicRAMIndexingModule_L31F43T86_Expr_1(i)  AND LogicRAMIndexingModule_L31F43T86_Expr_2(i)    ;
    end loop;

    end process;

process(LogicRAMIndexingModule_L26F41T84_Expr_1, LogicRAMIndexingModule_L26F41T84_Expr_2)
begin
    LogicRAMIndexingModule_L26F41T84_Expr <= resize(unsigned(signed(resize(LogicRAMIndexingModule_L26F41T84_Expr_1, LogicRAMIndexingModule_L26F41T84_Expr_1'length + 1)) + signed(resize(LogicRAMIndexingModule_L26F41T84_Expr_2, LogicRAMIndexingModule_L26F41T84_Expr_2'length + 1))), LogicRAMIndexingModule_L26F41T84_Expr'length);

end process;

process(LogicRAMIndexingModule_L27F42T85_Expr_1, LogicRAMIndexingModule_L27F42T85_Expr_2)
begin
    LogicRAMIndexingModule_L27F42T85_Expr <= resize(signed(signed(resize(LogicRAMIndexingModule_L27F42T85_Expr_1, LogicRAMIndexingModule_L27F42T85_Expr_1'length + 1)) - signed(resize(LogicRAMIndexingModule_L27F42T85_Expr_2, LogicRAMIndexingModule_L27F42T85_Expr_2'length + 1))), LogicRAMIndexingModule_L27F42T85_Expr'length);

end process;

process(LogicRAMIndexingModule_L28F42T85_Expr_1, LogicRAMIndexingModule_L28F42T85_Expr_2)
begin
    LogicRAMIndexingModule_L28F42T85_Expr <= resize(unsigned(signed(resize(LogicRAMIndexingModule_L28F42T85_Expr_1, LogicRAMIndexingModule_L28F42T85_Expr_1'length + 1)) + signed(resize(LogicRAMIndexingModule_L28F42T85_Expr_2, LogicRAMIndexingModule_L28F42T85_Expr_2'length + 1))), LogicRAMIndexingModule_L28F42T85_Expr'length);

end process;
process(Inputs_WE, Inputs_WriteData, State_Buff)
begin
for NextState_Buff_Iterator in 0 to 3 loop
NextState_Buff(NextState_Buff_Iterator) <= State_Buff(NextState_Buff_Iterator);
end loop;
if ( Inputs_WE = '1' ) then
NextState_Buff(TO_INTEGER(UNSIGNED(Inputs_WriteAddr))) <= Inputs_WriteData;
end if;
end process;
process(Inputs_OpData, LogicRAMIndexingModule_L26F34T85_Cast, LogicRAMIndexingModule_L26F41T54_Index, LogicRAMIndexingModule_L26F41T84_Expr, LogicRAMIndexingModule_L26F57T84_Index, LogicRAMIndexingModule_L27F35T86_Cast, LogicRAMIndexingModule_L27F42T69_Index, LogicRAMIndexingModule_L27F42T85_Expr, LogicRAMIndexingModule_L28F35T86_Cast, LogicRAMIndexingModule_L28F42T85_Expr, LogicRAMIndexingModule_L28F58T85_Index, LogicRAMIndexingModule_L30F36T87_Cast, LogicRAMIndexingModule_L30F43T70_Index, LogicRAMIndexingModule_L30F43T86_Expr, LogicRAMIndexingModule_L31F36T87_Cast, LogicRAMIndexingModule_L31F43T86_Expr, LogicRAMIndexingModule_L31F59T86_Index, LogicRAMIndexingModule_L33F34T61_Index, LogicRAMIndexingModule_L33F34T77_Expr, LogicRAMIndexingModule_L34F34T77_Expr, LogicRAMIndexingModule_L34F50T77_Index, OpData, ReadAddr, State_Buff, WE, WriteAddr, WriteData)
begin
LogicRAMIndexingModule_L33F34T77_ExprLhs <= signed(resize(unsigned(LogicRAMIndexingModule_L33F34T61_Index), LogicRAMIndexingModule_L33F34T77_ExprLhs'length));
LogicRAMIndexingModule_L33F34T77_ExprRhs <= signed(resize(unsigned(Inputs_OpData), LogicRAMIndexingModule_L33F34T77_ExprRhs'length));
LogicRAMIndexingModule_L34F34T77_ExprLhs <= signed(resize(unsigned(Inputs_OpData), LogicRAMIndexingModule_L34F34T77_ExprLhs'length));
LogicRAMIndexingModule_L34F34T77_ExprRhs <= signed(resize(unsigned(LogicRAMIndexingModule_L34F50T77_Index), LogicRAMIndexingModule_L34F34T77_ExprRhs'length));
LogicRAMIndexingModule_L30F43T86_Expr_1 <= LogicRAMIndexingModule_L30F43T70_Index;
LogicRAMIndexingModule_L30F43T86_Expr_2 <= Inputs_OpData;
LogicRAMIndexingModule_L31F43T86_Expr_1 <= Inputs_OpData;
LogicRAMIndexingModule_L31F43T86_Expr_2 <= LogicRAMIndexingModule_L31F59T86_Index;
LogicRAMIndexingModule_L26F41T84_Expr_1 <= signed(resize(unsigned(LogicRAMIndexingModule_L26F41T54_Index), LogicRAMIndexingModule_L26F41T84_Expr_1'length));
LogicRAMIndexingModule_L26F41T84_Expr_2 <= signed(resize(unsigned(LogicRAMIndexingModule_L26F57T84_Index), LogicRAMIndexingModule_L26F41T84_Expr_2'length));
LogicRAMIndexingModule_L27F42T85_Expr_1 <= signed(resize(unsigned(LogicRAMIndexingModule_L27F42T69_Index), LogicRAMIndexingModule_L27F42T85_Expr_1'length));
LogicRAMIndexingModule_L27F42T85_Expr_2 <= signed(resize(unsigned(Inputs_OpData), LogicRAMIndexingModule_L27F42T85_Expr_2'length));
LogicRAMIndexingModule_L28F42T85_Expr_1 <= signed(resize(unsigned(Inputs_OpData), LogicRAMIndexingModule_L28F42T85_Expr_1'length));
LogicRAMIndexingModule_L28F42T85_Expr_2 <= signed(resize(unsigned(LogicRAMIndexingModule_L28F58T85_Index), LogicRAMIndexingModule_L28F42T85_Expr_2'length));
Inputs_WE <= WE;
Inputs_WriteAddr <= WriteAddr;
Inputs_WriteData <= WriteData;
Inputs_ReadAddr <= ReadAddr;
Inputs_OpData <= OpData;
LogicRAMIndexingModule_L26F34T85_Cast <= LogicRAMIndexingModule_L26F41T84_Expr(7 downto 0);
MemLhsRhs <= LogicRAMIndexingModule_L26F34T85_Cast;
LogicRAMIndexingModule_L27F35T86_Cast <= unsigned(LogicRAMIndexingModule_L27F42T85_Expr(7 downto 0));
MathMemLhs <= LogicRAMIndexingModule_L27F35T86_Cast;
LogicRAMIndexingModule_L28F35T86_Cast <= LogicRAMIndexingModule_L28F42T85_Expr(7 downto 0);
MathMemRhs <= LogicRAMIndexingModule_L28F35T86_Cast;
LogicRAMIndexingModule_L30F36T87_Cast <= LogicRAMIndexingModule_L30F43T86_Expr;
LogicMemLhs <= LogicRAMIndexingModule_L30F36T87_Cast;
LogicRAMIndexingModule_L31F36T87_Cast <= LogicRAMIndexingModule_L31F43T86_Expr;
LogicMemRhs <= LogicRAMIndexingModule_L31F36T87_Cast;
CmpMemLhs <= LogicRAMIndexingModule_L33F34T77_Expr;
CmpMemRhs <= LogicRAMIndexingModule_L34F34T77_Expr;
LogicRAMIndexingModule_L26F41T54_Index <= State_Buff(bit_to_integer(LogicRAMIndexingModule_L26F52T53_Expr));
LogicRAMIndexingModule_L26F57T84_Index <= State_Buff(TO_INTEGER(UNSIGNED(Inputs_ReadAddr)));
LogicRAMIndexingModule_L27F42T69_Index <= State_Buff(TO_INTEGER(UNSIGNED(Inputs_ReadAddr)));
LogicRAMIndexingModule_L28F58T85_Index <= State_Buff(TO_INTEGER(UNSIGNED(Inputs_ReadAddr)));
LogicRAMIndexingModule_L30F43T70_Index <= State_Buff(TO_INTEGER(UNSIGNED(Inputs_ReadAddr)));
LogicRAMIndexingModule_L31F59T86_Index <= State_Buff(TO_INTEGER(UNSIGNED(Inputs_ReadAddr)));
LogicRAMIndexingModule_L33F34T61_Index <= State_Buff(TO_INTEGER(UNSIGNED(Inputs_ReadAddr)));
LogicRAMIndexingModule_L34F50T77_Index <= State_Buff(TO_INTEGER(UNSIGNED(Inputs_ReadAddr)));
end process;
-- [BEGIN USER ARCHITECTURE]
-- [END USER ARCHITECTURE]
end architecture;
