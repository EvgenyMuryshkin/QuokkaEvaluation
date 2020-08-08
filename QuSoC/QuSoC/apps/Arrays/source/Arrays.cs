namespace Arrays
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual uint Counter { get; set; }
    }

    public static class Firmware
    {
        static sbyte[] S8Buff = new sbyte[16];
        static short[] S16Buff = new short[16];
        static uint[] U32Buff = new uint[16];

        static void Fill(uint size)
        {
            uint offset = 3 + (size >> 1);
            for (uint i = 0; i < size; i++)
            {
                var value = i - offset;
                S8Buff[i] = (sbyte)value;
                S16Buff[i] = (short)value;
                U32Buff[i] = value;
            }
        }

        static void Sum(uint size)
        {
            uint result = 42;
            for (uint i = 0; i < size; i++)
            {
                result += (uint)S8Buff[i];
                result += (uint)S16Buff[i];
                result += U32Buff[i];
            }

            SOC.Instance.Counter = result;
        }

        public static void EntryPoint()
        {
            uint size = 6;
            Fill(size);
            Sum(size);
        }
    }
}
