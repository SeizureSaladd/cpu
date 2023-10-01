namespace CPU;

public abstract class Memory
{
    private static ushort[] _memory =
    {
        0b0100_0000_0000,
        0b1101_0010_0000,
        0b0001_0111_0011,
        0b1101_0000_0000,
        0b1101_0001_0000,
        0b0101_1111_0000,
        0b1010_0001_0000,
        0b0010_0100_0000,
        0b0011_1110_0000,
        0b1011_0010_0000,
        0b1001_0110_0000,
        0b1100_0001_0000,
        0b1011_0001_0000,
        0b0010_0101_0000,
        0b1111_1111_0000,
        0b0001_0111_0010
    };

    public static ushort GetLength()
    {
        return (ushort)_memory.Length;
    }
    
    public static ushort Read(ushort address)
    {
        return _memory[address];
    }

    public static void Write(ushort address, ushort value)
    {
        _memory[address] = value;
    }
}