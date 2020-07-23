		la x10, irq
		la x11, end
		csrrw zero, mtvec, x10

		li x20, 50 # iteration counter, expecting 0
		li x21, 0	# non-interrupted counter, expecting 50
		li x22, 0	# interruts counter, expecting 49
		
loop:	addi x20, x20, -1	# decrement iteration
		addi x21, x21, 1	# inctement non-interrupted counter
		beq x20, zero, end	# check if run out of iterations
		# enable interrupts
		csrrs zero, mstatus, 0x8	# expecting interrupt handling here
		csrrc zero, mstatus, 0x8	# disable interrupts
		j loop						

irq:
		addi x22, x22, 1
		mret

end:	j end
