addi x1, x0, 10
addi x2, x0, 11
li x3, 0xBAD
bne x1, x2, btg
j end
btg:
li x3, 0xC0FFEE
end:
nop