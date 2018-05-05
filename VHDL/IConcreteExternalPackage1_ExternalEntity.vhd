-- first implementation
library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity IConcreteExternalPackage1_ExternalEntity is
	port( 
		Clock: in std_logic;
		Reset: in std_logic;
		InSignal: in unsigned(8 downto 1);
		OutSignal: out unsigned(8 downto 1);
		InTrigger: in std_logic;
		OutReady: out std_logic
	);
end entity IConcreteExternalPackage1_ExternalEntity;

architecture rtl of IConcreteExternalPackage1_ExternalEntity is

begin
	-- forward incoming data to output, always ready
	OutSignal <= InSignal + 1;
	OutReady <= '1';
end architecture rtl;
