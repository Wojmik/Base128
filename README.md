# Base128 variable length integer serializer

This is library for binary serializing and deserializing integer types as variable length integers.

## What is it?

Let's consider Int64 type (long). Normally this type is binary serialized always as 8 bytes, no matter what value is in that type. In contrast, variable length binary serializer consumes the number of bytes depending on the actual value in that type. For small values (close to zero) it consumes only one byte, for very large values (far from zero) it consumes up to 10 bytes (for long type). Profit is that usually small values are much more common than large ones, so usually variable length binary serializer is few times more efficient (in size of produced output) than standard binary serializer.

## How it works?

Base128 serialization uses only 7 bits of every byte to store content of value. The most significant bit in every byte is used to indicate if more bytes are necessary to store whole value. Most important is that only significant bits are stored.

### Unsigned integer examples

#### ulong 55 example

Let's consider value of 55. If it is stored in ulong variable, hex representation is as follows:

```c#
0x0000000000000037
```

Binary representation of last byte is:

![Example unsigned 1 input](/Documentation/Example%20unsigned%2055%20input.png)

Only green bits are significant (before green bits there can be any number of leading zero bits and value will be the same). As it ware told, only significant bits are serialized so serialized value will be:

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
