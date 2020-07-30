namespace Recursion
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual uint Counter { get; set; }
    }

    public static class Firmware
    {
        static void RecursiveCounter(uint counter)
        {
            SOC.Instance.Counter += counter;

            if (counter < 10)
                RecursiveCounter(counter + 1);
        }

        public static void EntryPoint()
        {
            RecursiveCounter(0);
        }
    }
}
