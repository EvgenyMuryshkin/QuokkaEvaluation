li x1, 0x80000000 # mem blink storage
addi x3, x0, 0 # loop iterator
addi x4, x0, 0 # blink counter
li x20, 10 # timer iteration in blink toggle
li x21, 20 # number of toggles to run in simulation (half is the number of full blinks)

blink:
lw x31, 0(x1)
addi x31, x31, 1
sw x31, 0(x1)
addi x4, x4, 1
beq x4, x21, loop

addi x5, x0, 0 # timer iterator
counter:
addi x5, x5, 1
beq x5, x20, blink
j counter


# infinite loop is the end of simulation
loop:
	j loop