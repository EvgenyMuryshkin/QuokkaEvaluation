addi x1, x0, -10
addi x2, x0, -10
addi x3, x0, -11
li x4, 0xBAD
li x5, 0xBAD
bgeu x1, x2, b1
j end
b1:
li x4, 0xC0FFEE
bgeu x1, x3, b2
j end
b2:
li x5, 0xC0FFEE
end:
nop