using Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controllers
{
    public class TryControllers_Blocks
    {
        public static void Bootstrap(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD,
            Action<byte, FPGA.SyncStream<byte>> testAction
            )
        {
            const int baud = 115200;

            var stream = new FPGA.SyncStream<byte>();
            Action<byte> streamHandler = (data) =>
            {
                UART.Write(baud, data, TXD);
            };
            FPGA.Config.OnStream<byte>(stream, streamHandler);

            Action handler = () =>
            {
                byte data = 0;
                UART.Read(baud, RXD, out data);
                testAction(data, stream);
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }

        public static void TryCatchAll(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 10)
                    throw new Exception1();

                stream.Write(data);
            }
            catch (Exception)
            {
                stream.Write(255);
            }

            stream.Write(254);
        }


        public static void TryCatchAll_TryCatchAll(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 10)
                    throw new Exception1();

                TryCatchAll(data, stream);

                stream.Write(data);
            }
            catch (Exception)
            {
                stream.Write(254);
            }

            stream.Write(253);
        }

        public static void ReturnRethrow(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 200 || data == 201)
                    throw new Exception1();

                stream.Write(data);
            }
            catch (Exception1)
            {
                stream.Write(50);
                return;
            }
            catch (Exception2)
            {
                if (data == 201)
                    throw new Exception3();
            }
            finally
            {
                if (data == 100)
                    throw new Exception4();

                stream.Write(241);
            }

            stream.Write(240);
        }

        public static void TryCatchAll_ReturnRethrow(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 10)
                    throw new Exception1();

                ReturnRethrow(data, stream);

                stream.Write(data);
            }
            catch(Exception3)
            {
                stream.Write(199);
            }
            catch (Exception4)
            {
                stream.Write(205);
            }
            catch (Exception)
            {
                stream.Write(254);
            }

            stream.Write(253);
        }

        public static void TryCatchAll_TryCatchSome(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 10)
                    throw new Exception1();

                TryCatchSome(data, stream);

                stream.Write(data);
            }
            catch (Exception)
            {
                stream.Write(254);
            }

            stream.Write(253);
        }

        public static void TryCatchBase_TryCatchSome(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 10)
                    throw new DerivedException1();

                TryCatchSome(data, stream);

                stream.Write(data);
            }
            catch (BaseException)
            {
                stream.Write(254);
            }

            stream.Write(253);
        }

        public static void TryCatchAllFinally_TryCatchAll(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 10)
                    throw new Exception1();

                TryCatchAllFinally(data, stream);

                stream.Write(data);
            }
            catch (Exception)
            {
                stream.Write(254);
            }

            stream.Write(253);
        }

        public static void TryCatchAll_TryCatchAllFinally(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data == 10)
                    throw new Exception1();

                TryCatchAllFinally(data, stream);

                stream.Write(data);
            }
            catch (Exception)
            {
                stream.Write(254);
            }

            stream.Write(253);
        }

        public static void TryFinally(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                stream.Write(data);
            }
            finally
            {
                stream.Write(255);
            }

            stream.Write(255);
        }

        public static void TryCatchAllFinally(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                switch (data)
                {
                    case 1:
                        throw new Exception1();
                    case 2:
                        throw new Exception2();
                    default:
                        stream.Write(data);
                        break;
                }
            }
            catch (Exception)
            {
                stream.Write(10);
            }
            finally
            {
                stream.Write(254);
            }

            stream.Write(255);
        }

        public static void TryCatchExplicitFinally(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                switch (data)
                {
                    case 1:
                        throw new Exception1();
                    case 2:
                        throw new Exception2();
                    default:
                        stream.Write(data);
                        break;
                }
            }
            catch (Exception1)
            {
                stream.Write(10);
            }
            catch (Exception2)
            {
                stream.Write(20);
            }
            finally
            {
                stream.Write(254);
            }

            stream.Write(255);
        }

        public static void TryCatchExplicit(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                switch (data)
                {
                    case 1:
                        throw new Exception1();
                    case 2:
                        throw new Exception2();
                    default:
                        stream.Write(data);
                        break;
                }
            }
            catch (Exception1)
            {
                stream.Write(10);
            }
            catch (Exception2)
            {
                stream.Write(20);
            }

            stream.Write(255);
        }

        public static void TryCatchSome(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                switch (data)
                {
                    case 1:
                        throw new DerivedException1();
                    case 2:
                        throw new DerivedException2();
                    default:
                        stream.Write(data);
                        break;
                }
            }
            catch (DerivedException1)
            {
                stream.Write(10);
            }
            finally
            {
                stream.Write(154);
            }

            stream.Write(155);
        }

        public static void SmokeTest(byte data, FPGA.SyncStream<byte> stream)
        {
            try
            {
                if (data > 10)
                    TryCatchAll_ReturnRethrow(data, stream);
                else
                    TryCatchAllFinally(data, stream);
            }
            catch(Exception)
            {
                stream.Write(253);
            }
            finally
            {
                stream.Write(254);
            }

            stream.Write(255);
        }
    }
}
