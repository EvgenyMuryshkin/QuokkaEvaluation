-- second implementation
library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity IConcreteExternalPackage2_ExternalEntity is
	port( 
		Clock: in std_logic;
		Reset: in std_logic;
		InSignal: in unsigned(8 downto 1);
		OutSignal: out unsigned(8 downto 1);
		InTrigger: in std_logic;
		OutReady: out std_logic
	);
end entity IConcreteExternalPackage2_ExternalEntity;

architecture rtl of IConcreteExternalPackage2_ExternalEntity is
	signal counter: unsigned(8 downto 1) := (others => '0');
	type FSM is (Ready,Counting);
	signal currentState: FSM := Ready;
begin
	
	process(Clock,Reset,InSignal,InTrigger) is
	begin
		if( rising_edge(Clock) ) then
			if Reset = '1' then 
				currentState <= Ready;
				counter <= (others => '0');
			else
				case currentState is
					when Ready =>
						if( InTrigger = '1') then
							counter <= (others => '0');
							currentState <= Counting;
						end if;
					when Counting =>
						if counter = 10 then 
							currentState <= Ready;
						else
							counter <= counter + 1;
						end if;
					when others =>
						currentState <= Ready;
				end case;
			end if;
		end if;
	end process;
	
	OutSignal <= InSignal + counter;
	OutReady <= '1' when currentState = Ready else '0';
	
end architecture rtl;