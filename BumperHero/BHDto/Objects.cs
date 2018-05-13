using System;

namespace BHDto
{
    public enum ProximityState : byte
    {
        Measuring,
        Safe,
        Warning,
        Alert
    }

    public struct ReportDTO
    {
        public ushort Idx;
        public ushort Cur;
        public ushort Ave;
        public ushort Max;
        public ProximityState Prx;
        public ushort Gone;
    }
}
