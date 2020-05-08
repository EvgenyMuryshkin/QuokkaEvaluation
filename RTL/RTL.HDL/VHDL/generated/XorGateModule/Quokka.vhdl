library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

-- package for Quokka APIs	
package Quokka is
	type BoardSignalsType is record
		-- global signals
		Clock: std_logic;
		Reset: std_logic;
		
		-- states
		Running: std_logic;
		Started: std_logic;
		
		-- events
		Starting: std_logic;
	end record BoardSignalsType;
	
	procedure BoardSignalsProc(
		signal bs: inout BoardSignalsType;
		signal clk: in std_logic;
		signal reset: in std_logic;
		signal internalReset: in std_logic);

	function bit_to_integer( s : std_logic ) return integer;
end package Quokka;

-- package implementation
library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;
package body Quokka is
	procedure BoardSignalsProc(
		signal bs: inout BoardSignalsType;
		signal clk: in std_logic;
		signal reset: in std_logic;
		signal internalReset: in std_logic) is
	begin
		bs.Clock <= clk;
		bs.Reset <= reset OR internalReset;
		
		if rising_edge(bs.Clock) then
			if bs.Reset = '1' then
				bs.Running <= '0';
				bs.Started <= '0';
			else
				if bs.Running = '1' then
					bs.Started <= '1';
				end if;
				
				bs.Running <= '1';
			end if;
		end if;
		
		bs.Starting <= bs.Running AND NOT bs.Started;
	end procedure;

	function bit_to_integer( s : std_logic ) return integer is
		begin
			  if s = '1' then
			  return 1;
		   else
			  return 0;
		   end if;
		end function;

end package body Quokka;

library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity Metastability is
    port
    (
		Clock : in  std_logic;
		Reset : in  std_logic;
		Input : in  std_logic;
		Output : out  std_logic
    );
end entity;

architecture rtl of Metastability is
	signal buff: std_logic_vector(2 downto 1) := "00";
begin
	process(Clock, Reset, Input)
	begin
		if (rising_edge(Clock)) then
			if (Reset = '1') then
				buff <= (others => '0');
			else
				buff <= (buff(1), input);
			end if;
		end if;	
	end process;

	output <= buff(2);
end architecture;
