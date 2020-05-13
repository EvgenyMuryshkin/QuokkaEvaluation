## Supported data types in low-level RTL generator

### Native types

Basic native integer types can be used for signals declaration

| Type | Bits |Sign |
|--- | --- |--- |
| char | 8 | signed |
| byte | 8 | unsigned |
| short | 16 | signed |
| ushort | 16 | unsigned |
| int | 32 | signed |
| uint | 32 | unsigned |
| long | 64 | signed |
| ulong | 64 | unsigned |


### RTLBitArray

RTLBitArray type can be used to work with raw bit vectors. 

It defines basic math operations, cast, range access and resize capabilities.

It is open-sourced project and can be found in its own [repository](https://github.com/EvgenyMuryshkin/Quokka.RTL)

### Structs

There is not support for structs in RTL translator yet. It is in dev pipeline and will be available in near future.
