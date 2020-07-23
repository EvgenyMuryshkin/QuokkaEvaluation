module CounterTopLevel(
// [BEGIN USER PORTS]
// [END USER PORTS]

	input  Clock,
	input  Reset,
	output AggregatorBank1,
	output LED1,
	output LED2,
	output LED3,
	output LED4
);

wire [31:0] counter;
assign AggregatorBank1 = 1'b1;
assign LED1 = counter[18];
assign LED2 = counter[19];
assign LED3 = counter[20];
assign LED4 = counter[21];

Counter_TopLevel Counter_TopLevel
(
	.Clock (Clock),
	.Reset (!Reset),
	.CSCounter (counter)
);

endmodule