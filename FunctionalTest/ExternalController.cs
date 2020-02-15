using Drivers;
using FPGA;
using FPGA.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public interface IAbstractExternalPackage
    {
        void ExternalEntity(
            [ModuleInput]
            FPGA.Signal<byte> InSignal, 
            [ModuleOutput]
            FPGA.Signal<byte> OutSignal,
            [ModuleInput]
            FPGA.Signal<bool> InTrigger,
            [ModuleOutput]
            FPGA.Signal<bool> OutReady);
    }

    public interface IConcreteExternalPackage1 : Controllers.IAbstractExternalPackage { }

    public interface IConcreteExternalPackage2 : Controllers.IAbstractExternalPackage { }

    /*[BoardConfig(Name = "NEB")]*/[BoardConfig(Name = "Quokka")]
    public static class External_ExistingEntity
    {
        // Here are examples of two implementations of some functinality
        /*
        -- IConcreteExternalPackage1_ExternalEntity.vhdl
        -- first implementation
        library ieee;
        use ieee.std_logic_1164.all;
        use ieee.numeric_std.all;

        entity IConcreteExternalPackage1_ExternalEntity is
            port( 
                Clock: in std_logic;
                Reset: in std_logic;
                InSignal: in unsigned(8 downto 1);
                OutSignal: out unsigned(8 downto 1);
                InTrigger: in std_logic;
                OutReady: out std_logic
            );
        end entity IConcreteExternalPackage1_ExternalEntity;

        architecture rtl of IConcreteExternalPackage1_ExternalEntity is

        begin
            -- forward incoming data to output, always ready
            OutSignal <= InSignal + 1;
            OutReady <= '1';
        end architecture rtl;




        /////////////////////////////////////////////////////////
        -- IConcreteExternalPackage2_ExternalEntity.vhdl
        -- second implementation
        library ieee;
        use ieee.std_logic_1164.all;
        use ieee.numeric_std.all;

        entity IConcreteExternalPackage2_ExternalEntity is
	        port( 
		        Clock: in std_logic;
		        Reset: in std_logic;
		        InSignal: in unsigned(8 downto 1);
		        OutSignal: out unsigned(8 downto 1);
		        InTrigger: in std_logic;
		        OutReady: out std_logic
	        );
        end entity IConcreteExternalPackage2_ExternalEntity;

        architecture rtl of IConcreteExternalPackage2_ExternalEntity is
	        signal counter: unsigned(8 downto 1) := (others => '0');
	        type FSM is (Ready,Counting);
	        signal currentState: FSM := Ready;
        begin
	
	        process(Clock,Reset,InSignal,InTrigger) is
	        begin
		        if( rising_edge(Clock) ) then
			        if Reset = '1' then 
				        currentState <= Ready;
				        counter <= (others => '0');
			        else
				        case currentState is
					        when Ready =>
						        if( InTrigger = '1') then
							        counter <= (others => '0');
							        currentState <= Counting;
						        end if;
					        when Counting =>
						        if counter = 10 then 
							        currentState <= Ready;
						        else
							        counter <= counter + 1;
						        end if;
					        when others =>
						        currentState <= Ready;
				        end case;
			        end if;
		        end if;
	        end process;
	
	        OutSignal <= InSignal + counter;
	        OutReady <= '1' when currentState = Ready else '0';
	
        end architecture rtl;
        */

        static void NestedLevel1<T>(byte dataIn, out byte dataOut) where T : IAbstractExternalPackage
        {
            FPGA.Signal<byte> externalInput = new FPGA.Signal<byte>();
            FPGA.Config.Link(dataIn, out externalInput);

            FPGA.Signal<byte> externalOutput = new FPGA.Signal<byte>();
            FPGA.Signal<bool> externalTrigger = new FPGA.Signal<bool>();
            FPGA.Signal<bool> externalReady = new FPGA.Signal<bool>();

            FPGA.Config.Entity<T>().ExternalEntity(externalInput, externalOutput, externalTrigger, externalReady);
            externalTrigger = true;

            FPGA.Runtime.WaitForAllConditions(externalReady);

            dataOut = externalOutput;
        }

        static void NestedLevel2<T>(byte dataIn, out byte dataOut) where T : IAbstractExternalPackage
        {
            NestedLevel1<T>(dataIn, out dataOut);
        }

        public static async Task Aggregator(
            FPGA.InputSignal<bool> RXD,
            FPGA.OutputSignal<bool> TXD)
        {
            byte data = 0;
            FPGA.Signal<byte> externalInput = new FPGA.Signal<byte>();
            FPGA.Config.Link(data, out externalInput);

            FPGA.Signal<byte> externalOutput = new FPGA.Signal<byte>();
            FPGA.Signal<bool> externalTrigger = new FPGA.Signal<bool>();
            FPGA.Signal<bool> externalReady = new FPGA.Signal<bool>();

            Sequential handler = () =>
            {
                data = UART.Read(115200, RXD);

                for(var i = 0; i < 3; i++)
                {
                    byte result = 0;

                    switch (i)
                    {
                        case 0:
                            FPGA.Config.Entity<IConcreteExternalPackage1>().ExternalEntity(externalInput, externalOutput, externalTrigger, externalReady);

                            externalTrigger = true;

                            FPGA.Runtime.WaitForAllConditions(externalReady);

                            result = externalOutput;
                            break;
                        case 1:
                            NestedLevel1<IConcreteExternalPackage1>(data, out result);
                            break;
                        case 2:
                            NestedLevel2<IConcreteExternalPackage2>(40, out result);
                            break;
                    }

                    UART.Write(115200, result, TXD);
                }
            };

            const bool trigger = true;
            FPGA.Config.OnSignal(trigger, handler);
        }
    }
}
