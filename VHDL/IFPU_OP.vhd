library ieee;
use ieee.std_logic_1164.all;
use ieee.numeric_std.all;

entity IFPU_OP is
	port( 
		Clock: in std_logic;
		Reset: in std_logic;
		Trigger: in std_logic;
		Completed: out std_logic;
		Lhs: in signed(32 downto 1);
		Rhs: in signed(32 downto 1);
		Op: in unsigned(8 downto 1);
		Result: out signed(32 downto 1)
	);
end entity IFPU_OP;

architecture rtl of IFPU_OP is
	signal opa : std_logic_vector(32 downto 1);
	signal opb : std_logic_vector(32 downto 1);
	signal res : std_logic_vector(32 downto 1);
	signal fpuOp: std_logic_vector(3 downto 1);
begin
	opa <= std_logic_vector(Lhs);
	opb <= std_logic_vector(Rhs);
	fpuOp <= std_logic_vector(Op(3 downto 1));
	Result <= signed(res);
	
	--Existing FPU integration is done for FPU100, please clone and include FPU related files into your project
	--Repository is available here: https://github.com/freecores/fpu100.git
	--Integration commit: bab960a0e8c98eb217664d187208d7ede277248d

	fpu: entity work.fpu port map
	(
		clk_i => Clock,
		opa_i => opa,
		opb_i => opb,
		fpu_op_i => fpuOp,
		rmode_i => "00",
		output_o => res,
		start_i => Trigger,
		ready_o => Completed
	);

end architecture rtl;