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
-- System configuration name is CombinationalL1Override_TopLevel, clock frequency is 1Hz, Top-level
library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

use work.Quokka.all;

entity CombinationalL1Override_TopLevel is
    port
    (
-- [BEGIN USER PORTS]
-- [END USER PORTS]

InOverride : in  std_logic;
InValue : in  unsigned(7 downto 0);
OutValue : out  unsigned(7 downto 0)
    );
end entity;

-- FSM summary
-- Packages
architecture rtl of CombinationalL1Override_TopLevel is
-- [BEGIN USER SIGNALS]
-- [END USER SIGNALS]
constant HiSignal : std_logic := '1';
constant LoSignal : std_logic := '0';
constant Zero : std_logic := '0';
constant One : std_logic := '1';
constant true : std_logic := '1';
constant false : std_logic := '0';
constant CombinationalOverridesBase_L16F50T51_Expr : std_logic := '1';
constant CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F44T45_Expr : unsigned(2 downto 1)  := "10";
constant CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F44T45_Expr : unsigned(2 downto 1)  := "10";
signal Inputs_InOverride : std_logic := '0';
signal Inputs_InValue : unsigned(8 downto 1)  := "00000000";
signal InternalOffset : unsigned(8 downto 1)  := "00000000";
signal CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F20T46_Cast : unsigned(8 downto 1)  := "00000000";
signal CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F20T46_Cast : unsigned(8 downto 1)  := "00000000";
signal CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr : unsigned(10 downto 1)  := "0000000000";
signal CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_1 : signed(10 downto 1)  := "0000000000";
signal CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_2 : signed(10 downto 1)  := "0000000000";
signal CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr : signed(10 downto 1)  := "0000000000";
signal CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_1 : signed(10 downto 1)  := "0000000000";
signal CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_2 : signed(10 downto 1)  := "0000000000";
signal CombinationalOverridesBase_L14F41T96_Lookup : unsigned(8 downto 1)  := "00000000";
signal CombinationalOverridesBase_L14F41T96_LookupMultiplexerAddress : std_logic := '0';
signal CombinationalOverridesBase_L14F41T96_Lookup1 : unsigned(8 downto 1)  := "00000000";
signal CombinationalOverridesBase_L14F41T96_Lookup2 : unsigned(8 downto 1)  := "00000000";
begin

process(CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_1, CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_2)
begin
    CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr <= resize(unsigned(signed(resize(CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_1, CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_1'length + 1)) + signed(resize(CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_2, CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_2'length + 1))), CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr'length);

end process;

process(CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_1, CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_2)
begin
    CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr <= resize(signed(signed(resize(CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_1, CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_1'length + 1)) - signed(resize(CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_2, CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_2'length + 1))), CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr'length);

end process;
process(CombinationalOverridesBase_L14F41T96_Lookup1, CombinationalOverridesBase_L14F41T96_Lookup2, CombinationalOverridesBase_L14F41T96_LookupMultiplexerAddress)
begin
case CombinationalOverridesBase_L14F41T96_LookupMultiplexerAddress is
  when '0' => 
CombinationalOverridesBase_L14F41T96_Lookup <= CombinationalOverridesBase_L14F41T96_Lookup1;
  when '1' => 
CombinationalOverridesBase_L14F41T96_Lookup <= CombinationalOverridesBase_L14F41T96_Lookup2;
  when others => 
CombinationalOverridesBase_L14F41T96_Lookup <= "00000000";
end case;

end process;
process(CombinationalOverridesBase_L14F41T96_Lookup, CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F20T46_Cast, CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr, CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F20T46_Cast, CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr, InOverride, Inputs_InOverride, Inputs_InValue, InValue)
begin
CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_1 <= signed(resize(unsigned(Inputs_InValue), CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_1'length));
CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_2 <= signed(resize(unsigned(CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F44T45_Expr), CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr_2'length));
CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_1 <= signed(resize(unsigned(Inputs_InValue), CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_1'length));
CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_2 <= signed(resize(unsigned(CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F44T45_Expr), CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr_2'length));
Inputs_InOverride <= InOverride;
Inputs_InValue <= InValue;
InternalOffset <= (1 => CombinationalOverridesBase_L16F50T51_Expr, others => '0');
CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F20T46_Cast <= CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F27T45_Expr(8 downto 1);
CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F20T46_Cast <= unsigned(CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F27T45_Expr(8 downto 1));
OutValue <= CombinationalOverridesBase_L14F41T96_Lookup;
CombinationalOverridesBase_L14F41T96_Lookup1 <= CombinationalOverridesBase_L14F80T96_CombinationalL1Override_L10F9L12T10_CombinationalL1Override_L11F20T46_Cast;
CombinationalOverridesBase_L14F41T96_Lookup2 <= CombinationalOverridesBase_L14F61T77_CombinationalL1Override_L15F9L17T10_CombinationalL1Override_L16F20T46_Cast;
CombinationalOverridesBase_L14F41T96_LookupMultiplexerAddress <= Inputs_InOverride;
end process;
-- [BEGIN USER ARCHITECTURE]
-- [END USER ARCHITECTURE]
end architecture;
