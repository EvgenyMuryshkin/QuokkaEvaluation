addi x1, x0, 10
// x2 should be 1
slti x2, x1, 11
// x3 should be 0 after compare
addi x3, x0, 1
slti x3, x1, 10
// test with negative numbers
addi x4, x0, -10
// x4 should be 1
slti x5, x4, -9
// x6 should be 0
addi x6, x0, 1
slti x6, x4, -10
