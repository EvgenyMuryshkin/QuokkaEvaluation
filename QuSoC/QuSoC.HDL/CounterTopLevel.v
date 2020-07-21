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

wire [7:0] counter;
assign AggregatorBank1 = 1'b1;
assign LED1 = counter[0];
assign LED2 = counter[1];
assign LED3 = counter[2];
assign LED4 = counter[3];

Counter_TopLevel Counter_TopLevel
(
	.Clock (Clock),
	.Reset (!Reset),
	.Counter (counter)
);

endmodule