		la x10, irq
		la x11, end
		csrrw zero, mtvec, x10
		# enable interrupts
		csrrs zero, mstatus, 0x8
loop:	nop
		j loop

irq:
		li x12, 0xFF
		nop
		li x13, 0x1FF
		# do not return from irq in this test, hang
end:	j end