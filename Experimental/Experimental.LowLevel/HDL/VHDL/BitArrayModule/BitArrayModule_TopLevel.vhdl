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
-- System configuration name is BitArrayModule_TopLevel, clock frequency is 1Hz, Top-level
library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

use work.Quokka.all;

entity BitArrayModule_TopLevel is
    port
    (
-- [BEGIN USER PORTS]
-- [END USER PORTS]

Value : in  unsigned(7 downto 0);
Direct : out  unsigned(7 downto 0);
High : out  unsigned(3 downto 0);
Low : out  unsigned(3 downto 0);
Reversed : out  unsigned(7 downto 0);
ReversedHigh : out  unsigned(3 downto 0);
ReversedLow : out  unsigned(3 downto 0);
Picks : out  unsigned(3 downto 0)
    );
end entity;

-- FSM summary
-- Packages
architecture rtl of BitArrayModule_TopLevel is
-- [BEGIN USER SIGNALS]
-- [END USER SIGNALS]
constant HiSignal : std_logic := '1';
constant LoSignal : std_logic := '0';
constant Zero : std_logic := '0';
constant One : std_logic := '1';
constant true : std_logic := '1';
constant false : std_logic := '0';
signal Inputs_Value : unsigned(8 downto 1)  := "00000000";
signal Bits : unsigned(8 downto 1)  := "00000000";
signal BitArrayModule_L15F36T46_Index : unsigned(4 downto 1)  := "0000";
signal BitArrayModule_L16F35T45_Index : unsigned(4 downto 1)  := "0000";
signal BitArrayModule_L17F40T50_Index : unsigned(8 downto 1)  := "00000000";
signal BitArrayModule_L18F44T54_Index : unsigned(4 downto 1)  := "0000";
signal BitArrayModule_L19F43T53_Index : unsigned(4 downto 1)  := "0000";
signal BitArrayModule_L20F37T74_Source : unsigned(4 downto 1)  := "0000";
signal BitArrayModule_L20F53T62_Index : unsigned(2 downto 1)  := "00";
signal BitArrayModule_L20F64T73_Index : unsigned(2 downto 1)  := "00";
begin
-- Top-level entity connections
process(BitArrayModule_TopLevel_Direct, BitArrayModule_TopLevel_High, BitArrayModule_TopLevel_Low, BitArrayModule_TopLevel_Picks, BitArrayModule_TopLevel_Reversed, BitArrayModule_TopLevel_ReversedHigh, BitArrayModule_TopLevel_ReversedLow, Value)
begin
	BitArrayModule_TopLevel_Value <= Value;
Direct <= BitArrayModule_TopLevel_Direct;
High <= BitArrayModule_TopLevel_High;
Low <= BitArrayModule_TopLevel_Low;
Reversed <= BitArrayModule_TopLevel_Reversed;
ReversedHigh <= BitArrayModule_TopLevel_ReversedHigh;
ReversedLow <= BitArrayModule_TopLevel_ReversedLow;
Picks <= BitArrayModule_TopLevel_Picks;
end process;
process(BitArrayModule_L15F36T46_Index, BitArrayModule_L16F35T45_Index, BitArrayModule_L17F40T50_Index, BitArrayModule_L18F44T54_Index, BitArrayModule_L19F43T53_Index, BitArrayModule_L20F37T74_Source, BitArrayModule_L20F53T62_Index, BitArrayModule_L20F64T73_Index, Bits, Inputs_Value, Value)
begin
Inputs_Value <= unsigned(Value);
Bits <= unsigned(Inputs_Value);
Direct <= unsigned(Bits);
BitArrayModule_L15F36T46_Index <= unsigned(Bits(8 downto 5));
High <= unsigned(BitArrayModule_L15F36T46_Index);
BitArrayModule_L16F35T45_Index <= unsigned(Bits(4 downto 1));
Low <= unsigned(BitArrayModule_L16F35T45_Index);
BitArrayModule_L17F40T50_Index <= (1 => Bits(8), 2 => Bits(7), 3 => Bits(6), 4 => Bits(5), 5 => Bits(4), 6 => Bits(3), 7 => Bits(2), 8 => Bits(1), others => '0');
Reversed <= unsigned(BitArrayModule_L17F40T50_Index);
BitArrayModule_L18F44T54_Index <= (1 => Bits(8), 2 => Bits(7), 3 => Bits(6), 4 => Bits(5), others => '0');
ReversedHigh <= unsigned(BitArrayModule_L18F44T54_Index);
BitArrayModule_L19F43T53_Index <= (1 => Bits(4), 2 => Bits(3), 3 => Bits(2), 4 => Bits(1), others => '0');
ReversedLow <= unsigned(BitArrayModule_L19F43T53_Index);
BitArrayModule_L20F53T62_Index <= unsigned(Bits(7 downto 6));
BitArrayModule_L20F64T73_Index <= (1 => Bits(3), 2 => Bits(2), others => '0');
BitArrayModule_L20F37T74_Source <= (1 => BitArrayModule_L20F53T62_Index(1), 2 => BitArrayModule_L20F53T62_Index(2), 3 => BitArrayModule_L20F64T73_Index(1), 4 => BitArrayModule_L20F64T73_Index(2), others => '0');
Picks <= unsigned(BitArrayModule_L20F37T74_Source);
end process;
-- [BEGIN USER ARCHITECTURE]
-- [END USER ARCHITECTURE]
end architecture;