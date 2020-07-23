addi x1, x0, -10
addi x2, x0, -10
addi x3, x0, -9
li x4, 0xBAD
bltu x1, x2, end
bltu x1, x3, btg
j end
btg:
li x4, 0xC0FFEE
end:
nop