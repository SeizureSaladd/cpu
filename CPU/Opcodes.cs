namespace CPU;

public class Flags
{
    public bool ControlFlag { get; set; }
    public bool EqualFlag { get; set; }
}

public enum Opcode: ushort
{
    Read = 0b0001,
    Write = 0b0010,
    Cmp = 0b0011,
    CopyToDx = 0b0100,
    CopyFromAddress = 0b0101,
    CxToDx = 0b0110,
    DxToAddress = 0b0111,
    DxToCx = 0b1000,
    Jmp = 0b1001,
    Add = 0b1010,
    Inc = 0b1011,
    Not = 0b1100,
    Mov = 0b1101,
    Exit = 0b1111
}
