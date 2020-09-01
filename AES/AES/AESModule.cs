/*
    This is a port of AES implementation to low-level Quokka
    https://github.com/kokke/tiny-AES-c/blob/master/aes.c
    
    Work in progress.
*/
using Quokka.RTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AES
{
    public class AESModuleInputs
    {

    }

    public class AESModuleState
    {
        public RTLBitArray Value = new RTLBitArray().Resized(128);
    }

    /*
    #define Nb 4        // The number of columns comprising a state in AES. This is a constant in AES. Value=4
    #define Nk 4        // The number of 32 bit words in a key.
    #define Nr 10       // The number of rounds in AES Cipher.
     */

    public class AESModule : RTLSynchronousModule<AESModuleInputs, AESModuleState>
    {
        SBoxModule sbox = new SBoxModule();
        BoxModule box = new BoxModule();
        byte SBoxAddress => 0;
        byte RBoxAddress => 0;

        protected override void OnSchedule(Func<AESModuleInputs> inputsFactory)
        {
            base.OnSchedule(inputsFactory);

            box.Schedule(() => new BoxModuleInputs() { SBoxAddress = SBoxAddress, RBoxAddress = RBoxAddress });
            sbox.Schedule(() => new SBoxModuleInputs() { Value = State.Value });
        }

        protected override void OnStage()
        {
        }
    }
}
