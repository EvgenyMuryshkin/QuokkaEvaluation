[00000000]: LUI x2, 4096 (0x1000)            // 0x00001137
[00000004]: JAL x1, 0x000000D8 (0xD4)        // 0x0D4000EF
[00000008]: J 0x00000008                     // 0x0000006F
[0000000C]: ADDI x2, x2, -32 (0xFFFFFFE0)    // 0xFE010113
[00000010]: SW x1, 28(x2)                    // 0x00112E23
[00000014]: SW x8, 24(x2)                    // 0x00812C23
[00000018]: SW x9, 20(x2)                    // 0x00912A23
[0000001C]: ADDI x8, x2, 32 (0x20)           // 0x02010413
[00000020]: SW x10, -20(x8)                  // 0xFEA42623
[00000024]: LW x15, -17(x8)                  // 0xFEC42783
[00000028]: BEQ x15, x0, 20 (0x3C)           // 0x00078A63
[0000002C]: LW x14, -18(x8)                  // 0xFEC42703
[00000030]: ADDI x15, x0, 1 (0x1)            // 0x00100793
[00000034]: BEQ x14, x15, 16 (0x44)          // 0x00F70863
[00000038]: J 0x0000004C                     // 0x0140006F
[0000003C]: ADDI x15, x0, 0 (0x0)            // 0x00000793
[00000040]: J 0x00000078                     // 0x0380006F
[00000044]: ADDI x15, x0, 1 (0x1)            // 0x00100793
[00000048]: J 0x00000078                     // 0x0300006F
[0000004C]: LW x15, -17(x8)                  // 0xFEC42783
[00000050]: ADDI x15, x15, -1 (0xFFFFFFFF)   // 0xFFF78793
[00000054]: ADDI x10, x15, 0 (0x0)           // 0x00078513
[00000058]: JAL x1, 0x0000000C (0xFFFFFFB4)  // 0xFB5FF0EF
[0000005C]: ADDI x9, x10, 0 (0x0)            // 0x00050493
[00000060]: LW x15, -17(x8)                  // 0xFEC42783
[00000064]: ADDI x15, x15, -2 (0xFFFFFFFE)   // 0xFFE78793
[00000068]: ADDI x10, x15, 0 (0x0)           // 0x00078513
[0000006C]: JAL x1, 0x0000000C (0xFFFFFFA0)  // 0xFA1FF0EF
[00000070]: ADDI x15, x10, 0 (0x0)           // 0x00050793
[00000074]: ADD x15, x9, 15                  // 0x00F487B3
[00000078]: ADDI x10, x15, 0 (0x0)           // 0x00078513
[0000007C]: LW x1, 1(x2)                     // 0x01C12083
[00000080]: LW x8, 8(x2)                     // 0x01812403
[00000084]: LW x9, 9(x2)                     // 0x01412483
[00000088]: ADDI x2, x2, 32 (0x20)           // 0x02010113
[0000008C]: JALR x0, x1, 0 (0x0)             // 0x00008067
[00000090]: ADDI x2, x2, -16 (0xFFFFFFF0)    // 0xFF010113
[00000094]: SW x1, 12(x2)                    // 0x00112623
[00000098]: SW x8, 8(x2)                     // 0x00812423
[0000009C]: SW x9, 4(x2)                     // 0x00912223
[000000A0]: ADDI x8, x2, 16 (0x10)           // 0x01010413
[000000A4]: LUI x15, 2147483648 (0x80000000) // 0x800007B7
[000000A8]: LW x15, 15(x15)                  // 0x0007A783
[000000AC]: LUI x9, 2147483648 (0x80000000)  // 0x800004B7
[000000B0]: ADDI x10, x15, 0 (0x0)           // 0x00078513
[000000B4]: JAL x1, 0x0000000C (0xFFFFFF58)  // 0xF59FF0EF
[000000B8]: ADDI x15, x10, 0 (0x0)           // 0x00050793
[000000BC]: SW x15, 0(x9)                    // 0x00F4A023
[000000C0]: ADDI x0, x0, 0 (0x0)             // 0x00000013
[000000C4]: LW x1, 1(x2)                     // 0x00C12083
[000000C8]: LW x8, 8(x2)                     // 0x00812403
[000000CC]: LW x9, 9(x2)                     // 0x00412483
[000000D0]: ADDI x2, x2, 16 (0x10)           // 0x01010113
[000000D4]: JALR x0, x1, 0 (0x0)             // 0x00008067
[000000D8]: ADDI x2, x2, -16 (0xFFFFFFF0)    // 0xFF010113
[000000DC]: SW x1, 12(x2)                    // 0x00112623
[000000E0]: SW x8, 8(x2)                     // 0x00812423
[000000E4]: ADDI x8, x2, 16 (0x10)           // 0x01010413
[000000E8]: JAL x1, 0x00000090 (0xFFFFFFA8)  // 0xFA9FF0EF
[000000EC]: ADDI x0, x0, 0 (0x0)             // 0x00000013
[000000F0]: LW x1, 1(x2)                     // 0x00C12083
[000000F4]: LW x8, 8(x2)                     // 0x00812403
[000000F8]: ADDI x2, x2, 16 (0x10)           // 0x01010113
[000000FC]: JALR x0, x1, 0 (0x0)             // 0x00008067
[00000100]: 16                               // 0x00000010
[00000104]: 0                                // 0x00000000
[00000108]: 1                                // 0x00527A01
[0000010C]: 1                                // 0x01017C01
[00000110]: 27                               // 0x00020D1B
[00000114]: 40                               // 0x00000028
[00000118]: 24                               // 0x00000018
[0000011C]: 112                              // 0xFFFFFEF0
[00000120]: 4                                // 0x00000084
[00000124]: 0                                // 0x200E4400
[00000128]: 76                               // 0x8801814C
[0000012C]: 2                                // 0x44038902
[00000130]: 12                               // 0x0200080C
[00000134]: 96                               // 0xC844C160
[00000138]: 68                               // 0x0D44C944
[0000013C]: 2                                // 0x00000002
[00000140]: 36                               // 0x00000024
[00000144]: 68                               // 0x00000044
[00000148]: 72                               // 0xFFFFFF48
[0000014C]: 72                               // 0x00000048
[00000150]: 0                                // 0x100E4400
[00000154]: 76                               // 0x8801814C
[00000158]: 2                                // 0x44038902
[0000015C]: 12                               // 0x6400080C
[00000160]: 65                               // 0x44C844C1
[00000164]: 73                               // 0x020D44C9
[00000168]: 16                               // 0x00000010
[0000016C]: 0                                // 0x00000000
[00000170]: 1                                // 0x00527A01
[00000174]: 1                                // 0x01017C01
[00000178]: 27                               // 0x00020D1B
[0000017C]: 32                               // 0x00000020
[00000180]: 24                               // 0x00000018
[00000184]: 84                               // 0xFFFFFF54
[00000188]: 40                               // 0x00000028
[0000018C]: 0                                // 0x100E4400
[00000190]: 72                               // 0x88018148
[00000194]: 2                                // 0x080C4402
[00000198]: 0                                // 0x44C14C00
[0000019C]: 72                               // 0x020D44C8