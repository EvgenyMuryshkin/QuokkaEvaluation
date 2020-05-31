addi x1, x0, 10
addi x2, x0, 10
addi x3, x0, 11
li x4, 0xBAD
beq x1, x3, end
beq x1, x2, btg
j end
btg:
li x4, 0xC0FFEE
end:
nop