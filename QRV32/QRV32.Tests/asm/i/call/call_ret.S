		nop					#04
		nop					#08
		call proc			#C0
		addi x13, x0, 4		#10
		addi x14, x0, 5		#14
		j end				#18
proc:	addi x10, x0, 1		#1C
		addi x11, x0, 2		#20
		addi x12, x0, 3		#24
		addi x13, x0, 0xFF	#28
		ret					#2C
		addi x14, x0, 0xFF	#30
end:	nop					#34