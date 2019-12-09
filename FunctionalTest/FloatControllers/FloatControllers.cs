using System.Collections.Generic;
using System.Text;

namespace FloatControllers
{
    public static class FloatControllersOps
    {
        public static float BinaryExpressions(float op1, float op2, byte command)
        {
            switch (command)
            {
                case 0: return op1 + op2;
                case 1: return op1 - op2;
                case 2: return op1 * op2;
                case 3: return op1 / op2;
                case 4: return (op1 + op2) * 1.5f;
                case 5: return (op1 - op2) * -50.6f;
                default:
                    return 0;
            }
        }

        public static float UnaryExpressions(float op1, float op2, byte command)
        {
            float res = 0;
            switch (command)
            {
                case 6:
                    res = op1;
                    res += op2;
                    break;
                case 7:
                    res = op1;
                    res -= op2;
                    break;
                case 8:
                    res = op1;
                    res *= op2 + 1.0f;
                    break;
                case 9:
                    res = op1++;
                    break;
                case 10:
                    res = op2--;
                    break;
                default: 
                    res = 1.2f * 345.7f; break;
            }

            return res;
        }

        public static void TestHandler(float op1, float op2, byte command, out float res)
        {
            if (command < 6)
            {
                res = BinaryExpressions(op1, op2, command);
            }
            else
            {
                res = UnaryExpressions(op1, op2, command);
            }
        }
    }
}
