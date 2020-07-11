		la x10, irq
		la x11, end
		csrrw zero, mtvec, x10
		# enable interrupts
		csrr zero, mstatus, 0x8

loop:	nop
		j loop

irq:
		li x12, 0xFF
		nop
		li x13, 0x1FF
		mret

end:	j end