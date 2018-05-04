using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public struct DividerRequest
    {
        public ulong Numerator;
        public ulong Denominator;
    }

    public struct DividerResponse
    {
        public ulong Result;
        public ulong Remainder;
    }

    public struct RoundTrip
    {
        public byte b;
        public ushort us;
        public uint ui;
        public ulong ul;
    }

    public struct IsPrimeRequest
    {
        public uint value;
    }

    public struct IsPrimeResponse
    {
        public uint value;
        public byte result;
    }

    public struct BidirResponse
    {
        public byte dir;
        public byte value;
    }         
}
