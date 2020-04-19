// Quartus II Verilog Template
// Single port RAM with single read/write address 

module single_port_ram 
#(parameter DATA_WIDTH=8, parameter ADDR_WIDTH=6)
(
	input [(DATA_WIDTH-1):0] data,
	input [(ADDR_WIDTH-1):0] addr,
	input we, re, clk,
	output [(DATA_WIDTH-1):0] q
);

	// Declare the RAM variable
	reg [DATA_WIDTH-1:0] ram[2**ADDR_WIDTH-1:0];

	
	always @ (posedge clk)
	begin
	
		// Write
		if (we) 
			begin
				ram[addr] = data;
			end
		
		readData <= ram[addr];
	end
	
	reg [DATA_WIDTH-1:0] readData;
	
	// Continuous assignment implies read returns NEW data.
	// This is the natural behavior of the TriMatrix memory
	// blocks in Single Port mode.  

	//assign q = ram[addr_reg];
	assign q = readData;

endmodule