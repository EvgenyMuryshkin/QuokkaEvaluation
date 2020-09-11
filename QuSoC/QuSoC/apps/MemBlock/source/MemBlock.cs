namespace MemBlock
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual uint Counter { get; set; }
        public virtual uint[] MemBlock { get; set; } = new uint[1024];
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
            SOC.Instance.MemBlock[1023] = 42;

            for (byte i = 0; i < 10; i++)
            {
                SOC.Instance.MemBlock[i] = i;
            }

            for (byte i = 0; i < 10; i++)
            {
                SOC.Instance.MemBlock[1023] += SOC.Instance.MemBlock[i];
            }

        }
    }
}
