using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    /*[BoardConfig(Name = "NEB")]*/
    [BoardConfig(Name = "BIG")]
    public static class Misc_KeccakController
    {
        public static void Keccak1600(
            FPGA.Register<ulong> a00, FPGA.Register<ulong> a01, FPGA.Register<ulong> a02, FPGA.Register<ulong> a03, FPGA.Register<ulong> a04,
            FPGA.Register<ulong> a05, FPGA.Register<ulong> a06, FPGA.Register<ulong> a07, FPGA.Register<ulong> a08, FPGA.Register<ulong> a09,
            FPGA.Register<ulong> a10, FPGA.Register<ulong> a11, FPGA.Register<ulong> a12, FPGA.Register<ulong> a13, FPGA.Register<ulong> a14,
            FPGA.Register<ulong> a15, FPGA.Register<ulong> a16, FPGA.Register<ulong> a17, FPGA.Register<ulong> a18, FPGA.Register<ulong> a19,
            FPGA.Register<ulong> a20, FPGA.Register<ulong> a21, FPGA.Register<ulong> a22, FPGA.Register<ulong> a23, FPGA.Register<ulong> a24
        )
        {
            // TODO: check that map key is unsigned or allow key to be signed
            FPGA.Collections.ReadOnlyDictionary<uint, ulong> constants = new FPGA.Collections.ReadOnlyDictionary<uint, ulong>()
            {
                { 0, 0x0000000000000001 },
                { 1, 0x0000000000008082 },
                { 2, 0x800000000000808A },
                { 3, 0x8000000080008000 },
                { 4, 0x000000000000808B },
                { 5, 0x0000000080000001 },
                { 6, 0x8000000080008081 },
                { 7, 0x8000000000008009 },
                { 8, 0x000000000000008A },
                { 9, 0x0000000000000088 },
                { 10, 0x0000000080008009 },
                { 11, 0x000000008000000A },
                { 12, 0x000000008000808B },
                { 13, 0x800000000000008B },
                { 14, 0x8000000000008089 },
                { 15, 0x8000000000008003 },
                { 16, 0x8000000000008002 },
                { 17, 0x8000000000000080 },
                { 18, 0x000000000000800A },
                { 19, 0x800000008000000A },
                { 20, 0x8000000080008081 },
                { 21, 0x8000000000008080 },
                { 22, 0x0000000080000001 },
                { 23, 0x8000000080008008 },
            };

            ulong ulongMax = ulong.MaxValue;

            for (uint round = 0; round < 24; round += 4)
            {
                ulong rc0, rc1, rc2, rc3;
                rc0 = constants[round];
                rc1 = constants[round+1];
                rc2 = constants[round+2];
                rc3 = constants[round+3];

                ulong t = 0, bc0 = 0, bc1 = 0, bc2 = 0, bc3 = 0, bc4 = 0, d0 = 0, d1 = 0, d2 = 0, d3 = 0, d4 = 0;
                FPGA.Config.Suppress("W0003", t, bc0, bc1, bc2, bc3, bc4, d0, d1, d2, d3, d4);

                FPGA.Signal<bool> round1Trigger = false, round2Trigger = false, round3Trigger = false, round4Trigger = false;
                FPGA.Signal<bool> round1Completed = false, round2Completed = false, round3Completed = false, round4Completed = false;

                Func<ulong> x0 = () => a00 ^ a05 ^ a10 ^ a15 ^ a20;
                Func<ulong> x1 = () => a01 ^ a06 ^ a11 ^ a16 ^ a21;
                Func<ulong> x2 = () => a02 ^ a07 ^ a12 ^ a17 ^ a22;
                Func<ulong> x3 = () => a03 ^ a08 ^ a13 ^ a18 ^ a23;
                Func<ulong> x4 = () => a04 ^ a09 ^ a14 ^ a19 ^ a24;

                Func<ulong> xd0 = () => bc4 ^ (bc1 << 1 | bc1 >> 63);
                Func<ulong> xd1 = () => bc0 ^ (bc2 << 1 | bc2 >> 63);
                Func<ulong> xd2 = () => bc1 ^ (bc3 << 1 | bc3 >> 63);
                Func<ulong> xd3 = () => bc2 ^ (bc4 << 1 | bc4 >> 63);
                Func<ulong> xd4 = () => bc3 ^ (bc0 << 1 | bc0 >> 63);

                Func<ulong> bc021 = () => bc0 ^ (bc2 & (bc1 ^ ulongMax));
                Func<ulong> bc132 = () => bc1 ^ (bc3 & (bc2 ^ ulongMax));
                Func<ulong> bc243 = () => bc2 ^ (bc4 & (bc3 ^ ulongMax));
                Func<ulong> bc304 = () => bc3 ^ (bc0 & (bc4 ^ ulongMax));
                Func<ulong> bc410 = () => bc4 ^ (bc1 & (bc0 ^ ulongMax));

                Sequential round1 = () =>
                {
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(x0, (v) => bc0 = v),
                        FPGA.Expressions.Assign(x1, (v) => bc1 = v),
                        FPGA.Expressions.Assign(x2, (v) => bc2 = v),
                        FPGA.Expressions.Assign(x3, (v) => bc3 = v),
                        FPGA.Expressions.Assign(x4, (v) => bc4 = v)
                        );

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(xd0, (v) => d0 = v),
                        FPGA.Expressions.Assign(xd1, (v) => d1 = v),
                        FPGA.Expressions.Assign(xd2, (v) => d2 = v),
                        FPGA.Expressions.Assign(xd3, (v) => d3 = v),
                        FPGA.Expressions.Assign(xd4, (v) => d4 = v)
                        );

                    bc0 = a00 ^ d0;
                    t = a06 ^ d1;
                    bc1 = FPGA.Runtime.Rol(44, t);
                    t = a12 ^ d2;
                    bc2 = FPGA.Runtime.Rol(43, t);
                    t = a18 ^ d3;
                    bc3 = FPGA.Runtime.Rol(21, t);
                    t = a24 ^ d4;
                    bc4 = FPGA.Runtime.Rol(14, t);

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021() ^ rc0, a00),
                        FPGA.Expressions.AssignRegister(bc132(), a06),
                        FPGA.Expressions.AssignRegister(bc243(), a12),
                        FPGA.Expressions.AssignRegister(bc304(), a18),
                        FPGA.Expressions.AssignRegister(bc410(), a24)
                        );

                    t = a10 ^ d0;
                    bc2 = FPGA.Runtime.Rol(3, t);
                    t = a16 ^ d1;
                    bc3 = FPGA.Runtime.Rol(45, t);
                    t = a22 ^ d2;
                    bc4 = FPGA.Runtime.Rol(61, t);
                    t = a03 ^ d3;
                    bc0 = FPGA.Runtime.Rol(28, t);
                    t = a09 ^ d4;
                    bc1 = FPGA.Runtime.Rol(20, t);

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a10),
                        FPGA.Expressions.AssignRegister(bc132(), a16),
                        FPGA.Expressions.AssignRegister(bc243(), a22),
                        FPGA.Expressions.AssignRegister(bc304(), a03),
                        FPGA.Expressions.AssignRegister(bc410(), a09)
                        );

                    t = a20 ^ d0;
                    bc4 = FPGA.Runtime.Rol(18, t);
                    t = a01 ^ d1;
                    bc0 = FPGA.Runtime.Rol(1, t);
                    t = a07 ^ d2;
                    bc1 = FPGA.Runtime.Rol(6, t);
                    t = a13 ^ d3;
                    bc2 = FPGA.Runtime.Rol(25, t);
                    t = a19 ^ d4;
                    bc3 = FPGA.Runtime.Rol(8, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a20),
                        FPGA.Expressions.AssignRegister(bc132(), a01),
                        FPGA.Expressions.AssignRegister(bc243(), a07),
                        FPGA.Expressions.AssignRegister(bc304(), a13),
                        FPGA.Expressions.AssignRegister(bc410(), a19)
                        );

                    t = a05 ^ d0;
                    bc1 = FPGA.Runtime.Rol(36, t);
                    t = a11 ^ d1;
                    bc2 = FPGA.Runtime.Rol(10, t);
                    t = a17 ^ d2;
                    bc3 = FPGA.Runtime.Rol(15, t);
                    t = a23 ^ d3;
                    bc4 = FPGA.Runtime.Rol(56, t);
                    t = a04 ^ d4;
                    bc0 = FPGA.Runtime.Rol(27, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a05),
                        FPGA.Expressions.AssignRegister(bc132(), a11),
                        FPGA.Expressions.AssignRegister(bc243(), a17),
                        FPGA.Expressions.AssignRegister(bc304(), a23),
                        FPGA.Expressions.AssignRegister(bc410(), a04)
                        );

                    t = a15 ^ d0;
                    bc3 = FPGA.Runtime.Rol(41, t);
                    t = a21 ^ d1;
                    bc4 = FPGA.Runtime.Rol(2, t);
                    t = a02 ^ d2;
                    bc0 = FPGA.Runtime.Rol(62, t);
                    t = a08 ^ d3;
                    bc1 = FPGA.Runtime.Rol(55, t);
                    t = a14 ^ d4;
                    bc2 = FPGA.Runtime.Rol(39, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a15),
                        FPGA.Expressions.AssignRegister(bc132(), a21),
                        FPGA.Expressions.AssignRegister(bc243(), a02),
                        FPGA.Expressions.AssignRegister(bc304(), a08),
                        FPGA.Expressions.AssignRegister(bc410(), a14)
                        );

                    FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round1Completed));
                };
                FPGA.Config.OnSignal(round1Trigger, round1);

                FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round1Trigger));
                FPGA.Runtime.WaitForAllConditions(round1Completed);

                // Round 2
                Sequential round2 = () =>
                {
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(x0, (v) => bc0 = v),
                        FPGA.Expressions.Assign(x1, (v) => bc1 = v),
                        FPGA.Expressions.Assign(x2, (v) => bc2 = v),
                        FPGA.Expressions.Assign(x3, (v) => bc3 = v),
                        FPGA.Expressions.Assign(x4, (v) => bc4 = v)
                        );

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(xd0, (v) => d0 = v),
                        FPGA.Expressions.Assign(xd1, (v) => d1 = v),
                        FPGA.Expressions.Assign(xd2, (v) => d2 = v),
                        FPGA.Expressions.Assign(xd3, (v) => d3 = v),
                        FPGA.Expressions.Assign(xd4, (v) => d4 = v)
                        );

                    bc0 = a00 ^ d0;
                    t = a16 ^ d1;
                    bc1 = FPGA.Runtime.Rol(44, t);
                    t = a07 ^ d2;
                    bc2 = FPGA.Runtime.Rol(43, t);
                    t = a23 ^ d3;
                    bc3 = FPGA.Runtime.Rol(21, t);
                    t = a14 ^ d4;
                    bc4 = FPGA.Runtime.Rol(14, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021() ^ rc1, a00),
                        FPGA.Expressions.AssignRegister(bc132(), a16),
                        FPGA.Expressions.AssignRegister(bc243(), a07),
                        FPGA.Expressions.AssignRegister(bc304(), a23),
                        FPGA.Expressions.AssignRegister(bc410(), a14)
                        );

                    t = a20 ^ d0;
                    bc2 = FPGA.Runtime.Rol(3, t);
                    t = a11 ^ d1;
                    bc3 = FPGA.Runtime.Rol(45, t);
                    t = a02 ^ d2;
                    bc4 = FPGA.Runtime.Rol(61, t);
                    t = a18 ^ d3;
                    bc0 = FPGA.Runtime.Rol(28, t);
                    t = a09 ^ d4;
                    bc1 = FPGA.Runtime.Rol(20, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a20),
                        FPGA.Expressions.AssignRegister(bc132(), a11),
                        FPGA.Expressions.AssignRegister(bc243(), a02),
                        FPGA.Expressions.AssignRegister(bc304(), a18),
                        FPGA.Expressions.AssignRegister(bc410(), a09)
                        );

                    t = a15 ^ d0;
                    bc4 = FPGA.Runtime.Rol(18, t);
                    t = a06 ^ d1;
                    bc0 = FPGA.Runtime.Rol(1, t);
                    t = a22 ^ d2;
                    bc1 = FPGA.Runtime.Rol(6, t);
                    t = a13 ^ d3;
                    bc2 = FPGA.Runtime.Rol(25, t);
                    t = a04 ^ d4;
                    bc3 = FPGA.Runtime.Rol(8, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a15),
                        FPGA.Expressions.AssignRegister(bc132(), a06),
                        FPGA.Expressions.AssignRegister(bc243(), a22),
                        FPGA.Expressions.AssignRegister(bc304(), a13),
                        FPGA.Expressions.AssignRegister(bc410(), a04)
                        );

                    t = a10 ^ d0;
                    bc1 = FPGA.Runtime.Rol(36, t);
                    t = a01 ^ d1;
                    bc2 = FPGA.Runtime.Rol(10, t);
                    t = a17 ^ d2;
                    bc3 = FPGA.Runtime.Rol(15, t);
                    t = a08 ^ d3;
                    bc4 = FPGA.Runtime.Rol(56, t);
                    t = a24 ^ d4;
                    bc0 = FPGA.Runtime.Rol(27, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a10),
                        FPGA.Expressions.AssignRegister(bc132(), a01),
                        FPGA.Expressions.AssignRegister(bc243(), a17),
                        FPGA.Expressions.AssignRegister(bc304(), a08),
                        FPGA.Expressions.AssignRegister(bc410(), a24)
                        );

                    t = a05 ^ d0;
                    bc3 = FPGA.Runtime.Rol(41, t);
                    t = a21 ^ d1;
                    bc4 = FPGA.Runtime.Rol(2, t);
                    t = a12 ^ d2;
                    bc0 = FPGA.Runtime.Rol(62, t);
                    t = a03 ^ d3;
                    bc1 = FPGA.Runtime.Rol(55, t);
                    t = a19 ^ d4;
                    bc2 = FPGA.Runtime.Rol(39, t);

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a05),
                        FPGA.Expressions.AssignRegister(bc132(), a21),
                        FPGA.Expressions.AssignRegister(bc243(), a12),
                        FPGA.Expressions.AssignRegister(bc304(), a03),
                        FPGA.Expressions.AssignRegister(bc410(), a19)
                        );

                    FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round2Completed));
                };
                FPGA.Config.OnSignal(round2Trigger, round2);

                FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round2Trigger));
                FPGA.Runtime.WaitForAllConditions(round2Completed);

                // Round 3
                Sequential round3 = () =>
                {
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(x0, (v) => bc0 = v),
                        FPGA.Expressions.Assign(x1, (v) => bc1 = v),
                        FPGA.Expressions.Assign(x2, (v) => bc2 = v),
                        FPGA.Expressions.Assign(x3, (v) => bc3 = v),
                        FPGA.Expressions.Assign(x4, (v) => bc4 = v)
                        );

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(xd0, (v) => d0 = v),
                        FPGA.Expressions.Assign(xd1, (v) => d1 = v),
                        FPGA.Expressions.Assign(xd2, (v) => d2 = v),
                        FPGA.Expressions.Assign(xd3, (v) => d3 = v),
                        FPGA.Expressions.Assign(xd4, (v) => d4 = v)
                        );

                    bc0 = a00 ^ d0;
                    t = a11 ^ d1;
                    bc1 = FPGA.Runtime.Rol(44, t);
                    t = a22 ^ d2;
                    bc2 = FPGA.Runtime.Rol(43, t);
                    t = a08 ^ d3;
                    bc3 = FPGA.Runtime.Rol(21, t);
                    t = a19 ^ d4;
                    bc4 = FPGA.Runtime.Rol(14, t);

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021() ^ rc2, a00),
                        FPGA.Expressions.AssignRegister(bc132(), a11),
                        FPGA.Expressions.AssignRegister(bc243(), a22),
                        FPGA.Expressions.AssignRegister(bc304(), a08),
                        FPGA.Expressions.AssignRegister(bc410(), a19)
                        );

                    t = a15 ^ d0;
                    bc2 = FPGA.Runtime.Rol(3, t);
                    t = a01 ^ d1;
                    bc3 = FPGA.Runtime.Rol(45, t);
                    t = a12 ^ d2;
                    bc4 = FPGA.Runtime.Rol(61, t);
                    t = a23 ^ d3;
                    bc0 = FPGA.Runtime.Rol(28, t);
                    t = a09 ^ d4;
                    bc1 = FPGA.Runtime.Rol(20, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a15),
                        FPGA.Expressions.AssignRegister(bc132(), a01),
                        FPGA.Expressions.AssignRegister(bc243(), a12),
                        FPGA.Expressions.AssignRegister(bc304(), a23),
                        FPGA.Expressions.AssignRegister(bc410(), a09)
                        );

                    t = a05 ^ d0;
                    bc4 = FPGA.Runtime.Rol(18, t);
                    t = a16 ^ d1;
                    bc0 = FPGA.Runtime.Rol(1, t);
                    t = a02 ^ d2;
                    bc1 = FPGA.Runtime.Rol(6, t);
                    t = a13 ^ d3;
                    bc2 = FPGA.Runtime.Rol(25, t);
                    t = a24 ^ d4;
                    bc3 = FPGA.Runtime.Rol(8, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a05),
                        FPGA.Expressions.AssignRegister(bc132(), a16),
                        FPGA.Expressions.AssignRegister(bc243(), a02),
                        FPGA.Expressions.AssignRegister(bc304(), a13),
                        FPGA.Expressions.AssignRegister(bc410(), a24)
                        );

                    t = a20 ^ d0;
                    bc1 = FPGA.Runtime.Rol(36, t);
                    t = a06 ^ d1;
                    bc2 = FPGA.Runtime.Rol(10, t);
                    t = a17 ^ d2;
                    bc3 = FPGA.Runtime.Rol(15, t);
                    t = a03 ^ d3;
                    bc4 = FPGA.Runtime.Rol(56, t);
                    t = a14 ^ d4;
                    bc0 = FPGA.Runtime.Rol(27, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a20),
                        FPGA.Expressions.AssignRegister(bc132(), a06),
                        FPGA.Expressions.AssignRegister(bc243(), a17),
                        FPGA.Expressions.AssignRegister(bc304(), a03),
                        FPGA.Expressions.AssignRegister(bc410(), a14)
                        );

                    t = a10 ^ d0;
                    bc3 = FPGA.Runtime.Rol(41, t);
                    t = a21 ^ d1;
                    bc4 = FPGA.Runtime.Rol(2, t);
                    t = a07 ^ d2;
                    bc0 = FPGA.Runtime.Rol(62, t);
                    t = a18 ^ d3;
                    bc1 = FPGA.Runtime.Rol(55, t);
                    t = a04 ^ d4;
                    bc2 = FPGA.Runtime.Rol(39, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a10),
                        FPGA.Expressions.AssignRegister(bc132(), a21),
                        FPGA.Expressions.AssignRegister(bc243(), a07),
                        FPGA.Expressions.AssignRegister(bc304(), a18),
                        FPGA.Expressions.AssignRegister(bc410(), a04)
                        );

                    FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round3Completed));
                };

                FPGA.Config.OnSignal(round3Trigger, round3);

                FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round3Trigger));
                FPGA.Runtime.WaitForAllConditions(round3Completed);

                // Round 4
                Sequential round4 = () =>
                {
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(x0, (v) => bc0 = v),
                        FPGA.Expressions.Assign(x1, (v) => bc1 = v),
                        FPGA.Expressions.Assign(x2, (v) => bc2 = v),
                        FPGA.Expressions.Assign(x3, (v) => bc3 = v),
                        FPGA.Expressions.Assign(x4, (v) => bc4 = v)
                        );

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.Assign(xd0, (v) => d0 = v),
                        FPGA.Expressions.Assign(xd1, (v) => d1 = v),
                        FPGA.Expressions.Assign(xd2, (v) => d2 = v),
                        FPGA.Expressions.Assign(xd3, (v) => d3 = v),
                        FPGA.Expressions.Assign(xd4, (v) => d4 = v)
                        );

                    bc0 = a00 ^ d0;
                    t = a01 ^ d1;
                    bc1 = FPGA.Runtime.Rol(44, t);
                    t = a02 ^ d2;
                    bc2 = FPGA.Runtime.Rol(43, t);
                    t = a03 ^ d3;
                    bc3 = FPGA.Runtime.Rol(21, t);
                    t = a04 ^ d4;
                    bc4 = FPGA.Runtime.Rol(14, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021() ^ rc3, a00),
                        FPGA.Expressions.AssignRegister(bc132(), a01),
                        FPGA.Expressions.AssignRegister(bc243(), a02),
                        FPGA.Expressions.AssignRegister(bc304(), a03),
                        FPGA.Expressions.AssignRegister(bc410(), a04)
                        );

                    t = a05 ^ d0;
                    bc2 = FPGA.Runtime.Rol(3, t);
                    t = a06 ^ d1;
                    bc3 = FPGA.Runtime.Rol(45, t);
                    t = a07 ^ d2;
                    bc4 = FPGA.Runtime.Rol(61, t);
                    t = a08 ^ d3;
                    bc0 = FPGA.Runtime.Rol(28, t);
                    t = a09 ^ d4;
                    bc1 = FPGA.Runtime.Rol(20, t);
                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a05),
                        FPGA.Expressions.AssignRegister(bc132(), a06),
                        FPGA.Expressions.AssignRegister(bc243(), a07),
                        FPGA.Expressions.AssignRegister(bc304(), a08),
                        FPGA.Expressions.AssignRegister(bc410(), a09)
                        );

                    t = a10 ^ d0;
                    bc4 = FPGA.Runtime.Rol(18, t);
                    t = a11 ^ d1;
                    bc0 = FPGA.Runtime.Rol(1, t);
                    t = a12 ^ d2;
                    bc1 = FPGA.Runtime.Rol(6, t);
                    t = a13 ^ d3;
                    bc2 = FPGA.Runtime.Rol(25, t);
                    t = a14 ^ d4;
                    bc3 = FPGA.Runtime.Rol(8, t);

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a10),
                        FPGA.Expressions.AssignRegister(bc132(), a11),
                        FPGA.Expressions.AssignRegister(bc243(), a12),
                        FPGA.Expressions.AssignRegister(bc304(), a13),
                        FPGA.Expressions.AssignRegister(bc410(), a14)
                        );

                    t = a15 ^ d0;
                    bc1 = FPGA.Runtime.Rol(36, t);
                    t = a16 ^ d1;
                    bc2 = FPGA.Runtime.Rol(10, t);
                    t = a17 ^ d2;
                    bc3 = FPGA.Runtime.Rol(15, t);
                    t = a18 ^ d3;
                    bc4 = FPGA.Runtime.Rol(56, t);
                    t = a19 ^ d4;
                    bc0 = FPGA.Runtime.Rol(27, t);

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a15),
                        FPGA.Expressions.AssignRegister(bc132(), a16),
                        FPGA.Expressions.AssignRegister(bc243(), a17),
                        FPGA.Expressions.AssignRegister(bc304(), a18),
                        FPGA.Expressions.AssignRegister(bc410(), a19)
                        );

                    t = a20 ^ d0;
                    bc3 = FPGA.Runtime.Rol(41, t);
                    t = a21 ^ d1;
                    bc4 = FPGA.Runtime.Rol(2, t);
                    t = a22 ^ d2;
                    bc0 = FPGA.Runtime.Rol(62, t);
                    t = a23 ^ d3;
                    bc1 = FPGA.Runtime.Rol(55, t);
                    t = a24 ^ d4;
                    bc2 = FPGA.Runtime.Rol(39, t);

                    FPGA.Runtime.Assign(
                        FPGA.Expressions.AssignRegister(bc021(), a20),
                        FPGA.Expressions.AssignRegister(bc132(), a21),
                        FPGA.Expressions.AssignRegister(bc243(), a22),
                        FPGA.Expressions.AssignRegister(bc304(), a23),
                        FPGA.Expressions.AssignRegister(bc410(), a24)
                        );

                    FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round4Completed));
                };

                FPGA.Config.OnSignal(round4Trigger, round4);

                FPGA.Runtime.Assign(FPGA.Expressions.AssignSignal(true, round4Trigger));
                FPGA.Runtime.WaitForAllConditions(round4Completed);
            }
        }

        public static void ReadData(
            ref FPGA.Register<ulong> a00, ref FPGA.Register<ulong> a01, ref FPGA.Register<ulong> a02, ref FPGA.Register<ulong> a03, ref FPGA.Register<ulong> a04,
            ref FPGA.Register<ulong> a05, ref FPGA.Register<ulong> a06, ref FPGA.Register<ulong> a07, ref FPGA.Register<ulong> a08, ref FPGA.Register<ulong> a09,
            ref FPGA.Register<ulong> a10, ref FPGA.Register<ulong> a11, ref FPGA.Register<ulong> a12, ref FPGA.Register<ulong> a13, ref FPGA.Register<ulong> a14,
            ref FPGA.Register<ulong> a15, ref FPGA.Register<ulong> a16, ref FPGA.Register<ulong> a17, ref FPGA.Register<ulong> a18, ref FPGA.Register<ulong> a19,
            ref FPGA.Register<ulong> a20, ref FPGA.Register<ulong> a21, ref FPGA.Register<ulong> a22, ref FPGA.Register<ulong> a23, ref FPGA.Register<ulong> a24,
            FPGA.InputSignal<bool> RXD)
        {
            for (byte index = 0; index < 25; index++)
            {
                ulong data = 0;
                for (byte i = 0; i < 8; i++)
                {
                    byte part = UART.Read(115200, RXD);
                    data = ((ulong)part << 56) | (data >> 8);
                }

                FPGA.Runtime.Assign(
                    FPGA.Expressions.AssignRegister(index == 0 ? data : (ulong)a00, a00),
                    FPGA.Expressions.AssignRegister(index == 1 ? data : (ulong)a01, a01),
                    FPGA.Expressions.AssignRegister(index == 2 ? data : (ulong)a02, a02),
                    FPGA.Expressions.AssignRegister(index == 3 ? data : (ulong)a03, a03),
                    FPGA.Expressions.AssignRegister(index == 4 ? data : (ulong)a04, a04),
                    FPGA.Expressions.AssignRegister(index == 5 ? data : (ulong)a05, a05),
                    FPGA.Expressions.AssignRegister(index == 6 ? data : (ulong)a06, a06),
                    FPGA.Expressions.AssignRegister(index == 7 ? data : (ulong)a07, a07),
                    FPGA.Expressions.AssignRegister(index == 8 ? data : (ulong)a08, a08),
                    FPGA.Expressions.AssignRegister(index == 9 ? data : (ulong)a09, a09),
                    FPGA.Expressions.AssignRegister(index == 10 ? data : (ulong)a10, a10),
                    FPGA.Expressions.AssignRegister(index == 11 ? data : (ulong)a11, a11),
                    FPGA.Expressions.AssignRegister(index == 12 ? data : (ulong)a12, a12),
                    FPGA.Expressions.AssignRegister(index == 13 ? data : (ulong)a13, a13),
                    FPGA.Expressions.AssignRegister(index == 14 ? data : (ulong)a14, a14),
                    FPGA.Expressions.AssignRegister(index == 15 ? data : (ulong)a15, a15),
                    FPGA.Expressions.AssignRegister(index == 16 ? data : (ulong)a16, a16),
                    FPGA.Expressions.AssignRegister(index == 17 ? data : (ulong)a17, a17),
                    FPGA.Expressions.AssignRegister(index == 18 ? data : (ulong)a18, a18),
                    FPGA.Expressions.AssignRegister(index == 19 ? data : (ulong)a19, a19),
                    FPGA.Expressions.AssignRegister(index == 20 ? data : (ulong)a20, a20),
                    FPGA.Expressions.AssignRegister(index == 21 ? data : (ulong)a21, a21),
                    FPGA.Expressions.AssignRegister(index == 22 ? data : (ulong)a22, a22),
                    FPGA.Expressions.AssignRegister(index == 23 ? data : (ulong)a23, a23),
                    FPGA.Expressions.AssignRegister(index == 24 ? data : (ulong)a24, a24));
            }
        }

        public static void WriteData(
            FPGA.Register<ulong> a00, FPGA.Register<ulong> a01, FPGA.Register<ulong> a02, FPGA.Register<ulong> a03, FPGA.Register<ulong> a04,
            FPGA.Register<ulong> a05, FPGA.Register<ulong> a06, FPGA.Register<ulong> a07, FPGA.Register<ulong> a08, FPGA.Register<ulong> a09,
            FPGA.Register<ulong> a10, FPGA.Register<ulong> a11, FPGA.Register<ulong> a12, FPGA.Register<ulong> a13, FPGA.Register<ulong> a14,
            FPGA.Register<ulong> a15, FPGA.Register<ulong> a16, FPGA.Register<ulong> a17, FPGA.Register<ulong> a18, FPGA.Register<ulong> a19,
            FPGA.Register<ulong> a20, FPGA.Register<ulong> a21, FPGA.Register<ulong> a22, FPGA.Register<ulong> a23, FPGA.Register<ulong> a24,
            out bool TXD)
        {
            TXD = true;

            FPGA.Collections.ReadOnlyDictionary<byte, ulong> mapItems = new FPGA.Collections.ReadOnlyDictionary<byte, ulong>()
            {
                { 0, a00 }, { 1, a01 }, { 2, a02 }, { 3, a03 }, { 4, a04 },
                { 5, a05 }, { 6, a06 }, { 7, a07 }, { 8, a08 }, { 9, a09 },
                { 10, a10 }, { 11, a11 },{ 12, a12 }, { 13, a13 },  { 14, a14 },
                { 15, a15 }, { 16, a16 }, { 17, a17 }, { 18, a18 }, { 19, a19 },
                { 20, a20 }, { 21, a21 }, { 22, a22 }, { 23, a23 }, { 24, a24 },
            };

            for (byte pos = 0; pos < 25; pos++)
            {
                ulong tmp = 0;

                tmp = mapItems[pos];

                for (byte j = 0; j < 8; j++)
                {
                    UART.RegisteredWrite(115200, (byte)tmp, out TXD);
                    tmp = tmp >> 8;
                }
            }
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD
            )
        {
            bool internalTXD = true;
            FPGA.Config.Link(internalTXD, TXD);

            FPGA.Register<ulong> a00 = 0, a01 = 0, a02 = 0, a03 = 0, a04 = 0;
            FPGA.Register<ulong> a05 = 0, a06 = 0, a07 = 0, a08 = 0, a09 = 0;
            FPGA.Register<ulong> a10 = 0, a11 = 0, a12 = 0, a13 = 0, a14 = 0;
            FPGA.Register<ulong> a15 = 0, a16 = 0, a17 = 0, a18 = 0, a19 = 0;
            FPGA.Register<ulong> a20 = 0, a21 = 0, a22 = 0, a23 = 0, a24 = 0;

            FPGA.Config.Suppress("W0003", 
                a00, a01, a02, a03, a04, 
                a05, a06, a07, a08, a09, 
                a10, a11, a12, a13, a14, 
                a15, a16, a17, a18, a19, 
                a20, a21, a22, a23, a24);

            FPGA.Config.Suppress("W0005", 
                a00, a01, a02, a03, a04, 
                a05, a06, a07, a08, a09, 
                a10, a11, a12, a13, a14, 
                a15, a16, a17, a18, a19, 
                a20, a21, a22, a23, a24);

            Sequential handler = () =>
            {
                while (true)
                {
                    FPGA.Signal<bool> readTrigger = false;
                    FPGA.Signal<bool> readCompleted = false;

                    Sequential readHandler = () =>
                    {
                        ReadData(
                            ref a00, ref a01, ref a02, ref a03, ref a04,
                            ref a05, ref a06, ref a07, ref a08, ref a09,
                            ref a10, ref a11, ref a12, ref a13, ref a14,
                            ref a15, ref a16, ref a17, ref a18, ref a19,
                            ref a20, ref a21, ref a22, ref a23, ref a24,
                            RXD);

                        readCompleted = true;
                    };
                    FPGA.Config.OnSignal(readTrigger, readHandler);

                    readTrigger = true;
                    FPGA.Runtime.WaitForAllConditions(readCompleted);

                    Keccak1600(
                        a00, a01, a02, a03, a04,
                        a05, a06, a07, a08, a09,
                        a10, a11, a12, a13, a14,
                        a15, a16, a17, a18, a19,
                        a20, a21, a22, a23, a24);

                    FPGA.Signal<bool> writeTrigger = false;
                    FPGA.Signal<bool> writeCompleted = false;

                    Sequential writeHandler = () =>
                    {
                        WriteData(
                            a00, a01, a02, a03, a04,
                            a05, a06, a07, a08, a09,
                            a10, a11, a12, a13, a14,
                            a15, a16, a17, a18, a19,
                            a20, a21, a22, a23, a24,
                            out internalTXD);

                        writeCompleted = true;
                    };
                    FPGA.Config.OnSignal(writeTrigger, writeHandler);

                    writeTrigger = true;
                    FPGA.Runtime.WaitForAllConditions(writeCompleted);
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}