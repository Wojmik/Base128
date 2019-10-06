﻿# Base128 variable length integer serializer

This is library for binary serializing and deserializing integer types as variable length integers.

## What is it?

Let's consider Int64 type (long). Normally this type is binary serialized always as 8 bytes, no matter what value is in that type. In contrast, variable length binary serializer consumes the number of bytes depending on the actual value in that type. For small values (close to zero) it consumes only one byte, for very large values (far from zero) it consumes up to 10 bytes (for long type). Profit is that usually small values are much more common than large ones, so usually variable length binary serializer is few times more efficient (in size of produced output) than standard binary serializer.

## What are the methods?

In a static namespace WojciechMikołajewicz.Base128 are those static methods.

### Serialize integer values

```c#
bool TryWriteUInt64(Span<byte> destination, ulong value, out int written);
bool TryWriteUInt32(Span<byte> destination, uint value, out int written);
bool TryWriteInt64(Span<byte> destination, long value, out int written);
bool TryWriteInt32(Span<byte> destination, int value, out int written);
bool TryWriteInt64ZigZag(Span<byte> destination, long value, out int written);
bool TryWriteInt32ZigZag(Span<byte> destination, int value, out int written);
```

Where destination is place to serialize value to, value is the value to serialize, written is out variable indicating how many bytes was written to destination and return value indicates if operation succeed (it can be false if there was not enough room in the destination to serialize whole value).

To write ushort, short, byte or sbyte use one of TryWrite32 method.

### Deserialize integer values

```c#
bool TryReadUInt64(ReadOnlySpan<byte> source, out ulong value, out int read);
bool TryReadUInt32(ReadOnlySpan<byte> source, out uint value, out int read);
bool TryReadUInt16(ReadOnlySpan<byte> source, out ushort value, out int read);
bool TryReadUInt8(ReadOnlySpan<byte> source, out byte value, out int read);
bool TryReadInt64(ReadOnlySpan<byte> source, out long value, out int read);
bool TryReadInt32(ReadOnlySpan<byte> source, out int value, out int read);
bool TryReadInt16(ReadOnlySpan<byte> source, out short value, out int read);
bool TryReadInt8(ReadOnlySpan<byte> source, out sbyte value, out int read);
bool TryReadInt64ZigZag(ReadOnlySpan<byte> source, out long value, out int read);
bool TryReadInt32ZigZag(ReadOnlySpan<byte> source, out int value, out int read);
bool TryReadInt16ZigZag(ReadOnlySpan<byte> source, out short value, out int read);
bool TryReadInt8ZigZag(ReadOnlySpan<byte> source, out sbyte value, out int read);
```

Where source is place to deserialize value from, value is out variable witch deserialized value, read is out variable indicating how many bytes was read from source and return value indicates if operation succeed (it can be false if end of source was reached before whole value was read).

Those TryRead methods can throw an OverflowException if read value is too big or to small for destination integer type.

All those above methods are defined also without Try prefix. In those cases they throw an ArgumentOutOfRangeException if there was not sufficient space to write a value or end of source was reached before whole value was read.

### How many bytes are required to store particular value

```c#
int GetRequiredBytesUInt64(ulong value);
int GetRequiredBytesUInt32(uint value);
int GetRequiredBytesInt64(long value);
int GetRequiredBytesInt32(int value);
```

Where value is the value to check and return value is the number of required bytes to store argument value.

Try not to use those methods. Better assume there is enough space in destination and check return value of TryWrite methods to make sure.

### Skip value

Sometimes we only want to skip a value, not reading it. For this purpose there is:

```c#
bool TrySkip(ReadOnlySpan<byte> source, out int read);
```

Where source is a place with serialized value, read is out variable indicates how many bytes was skipped and return value indicates if operation succeed (it can be false if end of source was reached before whole value was skipped).

There is also Skip method which throws an ArgumentOutOfRangeException instead of returning false.

## How it works?

Base128 serialization uses only 7 bits of every byte to store content of value. The most significant bit in every byte is used to indicate if more bytes are necessary to store whole value. Most important is that only significant bits are stored.

### Unsigned integers

#### ulong 55 example

Let's consider value of 55. If it is stored in ulong variable, hex representation is as follows:

```c#
0x0000000000000037
```

Binary representation of last byte is:

![Example unsigned 1 input](/Documentation/Example%20unsigned%2055%20input.png)

Only green bits are significant (before green bits there can be any number of leading zero bits and value will be the same). As it were told, only significant bits are serialized so serialized value will be:

![Example unsigned output](/Documentation/Example%20unsigned%2055%20output.png)

Zero on red bit informs that this is the last byte used to store original value.

#### ulong 283 example

Value 283 stored in ulong variable has hex representation:

```c#
0x000000000000011B
```

Binary representation of last two bytes is:

![Example unsigned 283 input](/Documentation/Example%20unsigned%20283%20input.png)

And serialized value is:

![Example unsigned 283 output](/Documentation/Example%20unsigned%20283%20output.png)

Bytes are encoded as little endian – less significant byte is encoded first and so on. Seven bits from less significant byte are written first with red bit set to one – indicating that this byte wasn't last. Next seven bits are stored in next byte with red bit set to zero – indicating that this byte is last.

### Signed integers

In computer memory, signed integers are stored in U2 code. The weight of bits are like usual – less significant has weight 1 (2⁰), second 2 (2¹), third 4 (2²), fourth 8 (2³) and so one. Exception is only on the most significant bit. It has weight -2³¹ instead of 2³¹. So minimal value for int type has hex representation:

```c#
0x80000000
```

Only the most significant bit is set (with negative weight) and no other bits tries to compensate this massive negative value with its positive weights.

If all positive bits are also set to 1 (maximum compensation of massive negative value):

```c#
0xFFFFFFFF
```

The value of the whole number is -1. Sum of all positive weights is always one less then weight of negative (most significant) bit.

So – negative values always have most significant bit set and positive values (and zero) never have most significant bit set (sum of all positive weight is insufficient to compensate negative weight).

Positive values have the same binary representation as unsigned integers but they cannot take most significant bit so the range for positive values are twice less.

To expand type we have to copy most significant bit all the way to the wider type. For example for long type:

```c#
0xFFFFFFFFFFFFFFFF
```

This is also -1. And for the short type:

```c#
0xFFFF
```

This is also -1.

#### long -283 example

Value -283 stored in long variable has hex representation:

```c#
0xFFFFFFFFFFFFFEE5
```

Binary representation of last two bytes is:

![Example signed -283 input](/Documentation/Example%20signed%20-283%20input.png)

Significant bits are green. First set bit (from right) before series of all set bit is the last significant bit.

After serialization the value looks like:

![Example signed -283 output](/Documentation/Example%20signed%20-283%20output.png)

As before, less significant byte is serialized first. Seven bits are stored to the output and red bit is set – indicates this is not the last byte. Then next tree significant bits are stored, rest space is filed with last significant bit (as were said before – this is technique to expand type without changing value) and red bit is set to zero – there will be no other bytes.

#### long 83 example

Value 83 stored in long variable has hex representation:

```c#
0x0000000000000053
```

Binary representation of last two bytes is:

![Example signed 83 input](/Documentation/Example%20signed%2083%20input.png)

One leading zero bit is significant to distinguish from negative values.

After serialization the value looks like:

![Example signed 83 output](/Documentation/Example%20signed%2083%20output.png)

Seven less significant bits are stored together with red – set bit (indication this is not the last byte). Next, the last significant bit is stored, rest space is field with last significant bit, and red – zero bit (marks there will be no other bytes).

### Signed integers with ZigZag encoding

Signed integers can also be serialized in different way called ZigZag. In this approach negative numbers are pre-encoded to positive and then stored like unsigned integers.

In pre-encoding phase values are encoded as follows: 0 changes to 0, -1 changes to 1, 1 changes to 2, -2 changes to 3 and so on. All positive values and zero encodes to even numbers, all negative to odd.

For long type the pre-encoding phase does:

```c#
(value << 1) ^ (value >> 63)
```

Value coded this way is next serialized as unsigned integer.

## How many bytes are required to store particular value?

|     Type      | Required bytes | Max unsigned value | Min negative value | Max positive value |
| :-----------: | -------------: | -----------------: | -----------------: | -----------------: |
|  byte, sbyte  |              1 |                127 |                -64 |                 63 |
|  byte, sbyte  |              2 |              16383 |              -8192 |               8191 |
| ushort, short |              3 |            2097151 |           -1048576 |            1048575 |
|   uint, int   |              4 |          268435455 |         -134217728 |          134217727 |
|   uint, int   |              5 |        34359738367 |       -17179869184 |        17179869183 |
|  ulong, long  |              6 |              2⁴²-1 |               -2⁴¹ |              2⁴¹-1 |
|  ulong, long  |              7 |              2⁴⁹-1 |               -2⁴⁸ |              2⁴⁸-1 |
|  ulong, long  |              8 |              2⁵⁶-1 |               -2⁵⁵ |              2⁵⁵-1 |
|  ulong, long  |              9 |              2⁶³-1 |               -2⁶² |              2⁶²-1 |
|  ulong, long  |             10 |              2⁶⁴-1 |               -2⁶³ |              2⁶³-1 |

## BinaryReader / BinaryWriter

From version 1.1.0 in namespace WojciechMikołajewicz there are also BinaryReaderBase128 (derived from System.IO.BinaryReader) and BinaryWriterBase128 (derived from System.IO.BinaryWriter) for reading and writing variable length integers to and from Streams.