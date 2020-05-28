addi x1, x0, 10
// x2 should be 1
slti x2, x1, 11
// x2 should be 0
slti x2, x1, 10
// test with negative numbers
addi x3, x0, -10
// x4 should be 1
slti x4, x3, -9
// x4 should be 0
slti x4, x3, -10
