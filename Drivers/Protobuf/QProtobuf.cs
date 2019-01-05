using System;
using System.Collections.Generic;
using System.Text;

namespace Drivers.Protobuf
{
	/// <summary>
	/// Indicates the encoding used to represent an individual value in a protobuf stream
	/// </summary>
	public enum QWireType
	{
		/// <summary>
		/// Represents an error condition
		/// </summary>
		None = -1,

		/// <summary>
		/// Base-128 variant-length encoding
		/// </summary>
		Variant = 0,

		/// <summary>
		/// Fixed-length 8-byte encoding
		/// </summary>
		Fixed64 = 1,

		/// <summary>
		/// Length-variant-prefixed encoding
		/// </summary>
		String = 2,

		/// <summary>
		/// Indicates the start of a group
		/// </summary>
		StartGroup = 3,

		/// <summary>
		/// Indicates the end of a group
		/// </summary>
		EndGroup = 4,

		/// <summary>
		/// Fixed-length 4-byte encoding
		/// </summary>10
		Fixed32 = 5,

		/// <summary>
		/// This is not a formal wire-type in the "protocol buffers" spec, but
		/// denotes a variant integer that should be interpreted using
		/// zig-zag semantics (so -ve numbers aren't a significant overhead)
		/// </summary>
		SignedVariant = QWireType.Variant | (1 << 3),
	}

	public static class QProtobuf
	{
		public static void WriteUInt32Variant(UInt32 value, FPGA.SyncStream<byte> output)
		{
			// reserve 10 bytes
			byte[] buff = new byte[10];

			int count = 0;

			do
			{
				buff[count] = (byte)((value & 0x7F) | 0x80);
				count++;
			} while ((value >>= 7) != 0);

			byte tmp;
			tmp = buff[count - 1];
			buff[count - 1] = (byte)(tmp & 0x7F);

			for (int i = 0; i < count; i++)
			{
				tmp = buff[i];
				output.Write(tmp);
			}
		}

		public static void WriteHeaderCore(int fieldNumber, QWireType wireType, FPGA.SyncStream<byte> output)
		{
			uint header = (((uint)fieldNumber) << 3) | (((uint)wireType) & 7);

			WriteUInt32Variant(header, output);
		}
	}
}
