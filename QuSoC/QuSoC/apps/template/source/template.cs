namespace template
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual uint Counter { get; set; }
    }

    public static class Firmware
    {
        public static void EntryPoint()
        {
        }
    }
}
