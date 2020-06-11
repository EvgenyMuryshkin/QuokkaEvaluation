addi x1, x0, 10
addi x2, x0, 20

li x3, 0x100
li x4, 0xBAD0C0DE
li x5, 0xC0DE0BAD
li x6, 0xC00DC0DE

sb x4, 0x8(x3)
sb x4, 0x9(x3)
sb x5, 0xA(x3)
sb x5, 0xB(x3)

sh x4, 0x4(x3)
sh x5, 0x6(x3)

sw x6, 0(x3)

# infinite loop is the end of simulation
loop:
	j loop