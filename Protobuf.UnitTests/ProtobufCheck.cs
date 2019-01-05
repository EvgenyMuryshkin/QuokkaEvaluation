using Drivers.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Protobuf.UnitTests
{
	class TestStream : FPGA.SyncStream<byte>
	{
		public Queue<byte> Buff = new Queue<byte>();
		public TestStream()
		{
			Subscribe((data) =>
			{
				Buff.Enqueue(data);	
			});
		}
	}

	[ProtoContract]
	class SimpleFields
	{
		[ProtoMember(1)]
		public bool BooleanField1 { get; set; }

		[ProtoMember(100)]
		public bool BooleanField2 { get; set; }
	}

	[TestClass]
	public class ProtobufCheck
	{
		byte[] PSerialize<T>(T value)
		{
			using (var ms = new MemoryStream())
			{
				Serializer.Serialize(ms, new SimpleFields()
				{
					BooleanField1 = true
				});

				ms.Seek(0, SeekOrigin.Begin);
				var data = ms.ToArray();

				return data;
			}
		}

		byte[] QSerialize<T>(T payload)
		{
			// NOTE: this is emulation of what proto genertor will do
			var stream = new TestStream();

			var protoFields = typeof(T)
				.GetProperties()
				.Where(m => m.GetCustomAttributes(false).OfType<ProtoMemberAttribute>().Any())
				.OrderBy(m => m.GetCustomAttributes(false).OfType<ProtoMemberAttribute>().Single().Tag);

			foreach (var prop in protoFields)
			{
				var order = prop.GetCustomAttributes(false).OfType<ProtoMemberAttribute>().Single().Tag;
				var fieldValue = prop.GetValue(payload);
				switch(fieldValue)
				{
					case bool b:
						if(b)
						{
							QProtobuf.WriteHeaderCore(order, QWireType.Variant, stream);
							QProtobuf.WriteUInt32Variant((uint)(b ? 1 : 0), stream);
						}
						break;
				}
			}


			return stream.Buff.ToArray();
		}

		void Compare<T>(T payload, string message = "")
		{
			var pdata = PSerialize(payload);
			var qdata = QSerialize(payload);

			Assert.AreEqual(pdata.Length, qdata.Length, $"Length failed: {message}");
			for(int i = 0; i < pdata.Length; i++)
			{
				Assert.AreEqual(pdata[i], qdata[i], $"Failed at indes {i}: {message}");
			}
		}

		[TestMethod]
		public void SimpleFields()
		{
			Compare(new SimpleFields() { BooleanField1 = true }, "Boolean");
		}
	}
}
