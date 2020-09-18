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
-- System configuration name is CounterModule_TopLevel, clock frequency is 1Hz, Top-level
library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

use work.Quokka.all;

entity CounterModule_TopLevel is
    port
    (
-- [BEGIN USER PORTS]
-- [END USER PORTS]

Clock : in  std_logic;
Reset : in  std_logic;
Enabled : in  std_logic;
Value : out  unsigned(7 downto 0)
    );
end entity;

-- FSM summary
-- Packages
architecture rtl of CounterModule_TopLevel is
-- [BEGIN USER SIGNALS]
-- [END USER SIGNALS]
constant HiSignal : std_logic := '1';
constant LoSignal : std_logic := '0';
constant Zero : std_logic := '0';
constant One : std_logic := '1';
constant true : std_logic := '1';
constant false : std_logic := '0';
constant CounterModule_L19F65T66_Expr : std_logic := '1';
signal Inputs_Enabled : std_logic := '0';
signal NextState_Value : unsigned(7 downto 0)  := "00000000";
signal NextValue : unsigned(7 downto 0)  := "00000000";
signal CounterModule_L19F27T81_Cast : unsigned(7 downto 0)  := "00000000";
signal State_Value : unsigned(7 downto 0)  := "00000000";
constant State_ValueDefault : unsigned(7 downto 0)  := "00000000";
signal CounterModule_L19F51T66_Expr : unsigned(9 downto 0)  := "0000000000";
signal CounterModule_L19F51T66_Expr_1 : signed(9 downto 0)  := "0000000000";
signal CounterModule_L19F51T66_Expr_2 : signed(9 downto 0)  := "0000000000";
signal CounterModule_L19F34T80_Lookup : unsigned(7 downto 0)  := "00000000";
signal CounterModule_L19F34T80_LookupMultiplexerAddress : std_logic := '0';
signal CounterModule_L19F34T80_Lookup1 : unsigned(7 downto 0)  := "00000000";
signal CounterModule_L19F34T80_Lookup2 : unsigned(7 downto 0)  := "00000000";
begin
process (Clock, NextState_Value, Reset)
begin
if rising_edge(Clock) then
if ( Reset = '1' ) then
State_Value <= State_ValueDefault;
else
State_Value <= NextState_Value;
end if;
end if;
end process;

process(CounterModule_L19F51T66_Expr_1, CounterModule_L19F51T66_Expr_2)
begin
    CounterModule_L19F51T66_Expr <= resize(unsigned(signed(resize(CounterModule_L19F51T66_Expr_1, CounterModule_L19F51T66_Expr_1'length + 1)) + signed(resize(CounterModule_L19F51T66_Expr_2, CounterModule_L19F51T66_Expr_2'length + 1))), CounterModule_L19F51T66_Expr'length);

end process;
process(CounterModule_L19F34T80_Lookup1, CounterModule_L19F34T80_Lookup2, CounterModule_L19F34T80_LookupMultiplexerAddress)
begin
case CounterModule_L19F34T80_LookupMultiplexerAddress is
  when '0' => 
CounterModule_L19F34T80_Lookup <= CounterModule_L19F34T80_Lookup1;
  when '1' => 
CounterModule_L19F34T80_Lookup <= CounterModule_L19F34T80_Lookup2;
  when others => 
CounterModule_L19F34T80_Lookup <= "00000000";
end case;

end process;
process(NextValue, State_Value)
begin
NextState_Value <= State_Value;
NextState_Value <= NextValue;
end process;
process(CounterModule_L19F27T81_Cast, CounterModule_L19F34T80_Lookup, CounterModule_L19F51T66_Expr, Enabled, Inputs_Enabled, State_Value)
begin
CounterModule_L19F51T66_Expr_1 <= signed(resize(unsigned(State_Value), CounterModule_L19F51T66_Expr_1'length));
CounterModule_L19F51T66_Expr_2 <= (0 => CounterModule_L19F65T66_Expr, others => '0');
Inputs_Enabled <= Enabled;
CounterModule_L19F27T81_Cast <= CounterModule_L19F34T80_Lookup;
NextValue <= CounterModule_L19F27T81_Cast;
Value <= State_Value;
CounterModule_L19F34T80_Lookup1 <= State_Value;
CounterModule_L19F34T80_Lookup2 <= CounterModule_L19F51T66_Expr(7 downto 0);
CounterModule_L19F34T80_LookupMultiplexerAddress <= Inputs_Enabled;
end process;
-- [BEGIN USER ARCHITECTURE]
-- [END USER ARCHITECTURE]
end architecture;
