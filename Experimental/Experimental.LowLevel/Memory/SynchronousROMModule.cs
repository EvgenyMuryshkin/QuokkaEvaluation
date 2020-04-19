using System;
using System.Collections.Generic;
using System.Linq;
using Quokka.RTL;

namespace QuokkaTests.Experimental
{
    public class SynchronousROMModuleInputs
    {
        public byte Addr1;
        public byte Addr2;
    }

    public class SynchronousROMModuleState
    {
        public byte Data1;
        public byte Data2;
        public byte[] Buff;
    }

    public class SynchronousROMModule : RTLSynchronousModule<SynchronousROMModuleInputs, SynchronousROMModuleState>
    {
        public SynchronousROMModule()
        {
            State.Buff = GetBuffer();
        }

        static byte[] GetBuffer()
        {
            var rnd = new Random(Environment.TickCount);

            return Enumerable
                .Range(0, 256)
                .Select(i => (byte)i)
                .OrderBy(r => rnd.Next())
                .ToArray();
        }

        public byte Data1 => State.Data1;
        public byte Data2 => State.Data2;

        protected override void OnStage()
        {
            NextState.Data1 = State.Buff[Inputs.Addr1];
            NextState.Data2 = State.Buff[Inputs.Addr2];
        }
    }
}
