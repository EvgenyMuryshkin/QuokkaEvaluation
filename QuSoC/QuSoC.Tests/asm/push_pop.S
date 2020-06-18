			li sp, 0x100
			li x10, 1
			li x11, 2
			call push_x10
			call push_x11
			call pop_x10
			call pop_x11
end:		j end

push_x10:	
			addi sp, sp, -4
			sw x10, 0(sp)
			ret
push_x11:	
			addi sp, sp, -4
			sw x11, 0(sp)
			ret
pop_x10:	
			lw x10, 0(sp)
			addi sp, sp, 4
			ret
pop_x11:	
			lw x11, 0(sp)
			addi sp, sp, 4
			ret