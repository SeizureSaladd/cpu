using System.Text;

namespace CPU;

public class Cpu
{
    public ushort ProgramCounter;
    public ushort Instruction;
    public ushort Addr;
    public ushort Data;

    public ushort[] Registers =
    {
        0, // AX
        0, // BX
        0, // CX
        0  // DX
    };
    
    public readonly Flags Flags = new();

    private readonly Instructions _instructions;

    public Cpu()
    {
        _instructions = new Instructions(this);
    }
    
    public void Start()
    {
        for (var i = 0; i < Memory.GetLength(); i++)
        {
            Log();
            ExecuteNext();
            Console.ReadKey();
        }
    }

    private static string GroupBits(string bits)
    {
        var sb = new StringBuilder();
        var groups =  Enumerable.Range(0, bits.Length / 4)
            .Select(i => bits.Substring(i * 4, 4));
        
        foreach (var chunk in groups)
        {
            sb.Append(chunk);
            sb.Append(' ');
        }

        sb.Remove(0, 5);

        return sb.ToString();
    }

    private void Log()
    {
        Console.WriteLine("Program counter: " + ProgramCounter);
        Console.WriteLine("Instruction: " + GroupBits(Convert.ToString(Instruction, 2).PadLeft(16, '0')) + new Instruction(Instruction).Opcode);
        Console.WriteLine("Address: " + GroupBits(Convert.ToString(Addr, 2).PadLeft(16, '0')));
        Console.WriteLine("Data: " + GroupBits(Convert.ToString(Data, 2).PadLeft(16, '0')));

        for (var i = 0; i < Registers.Length; i++)
        {
            Console.WriteLine("Register " + i + ": " + GroupBits(Convert.ToString(Registers[i], 2).PadLeft(16, '0')));
        }
        Console.WriteLine();
    }

    private void ExecuteNext()
    {
        Fetch();
        Decode();
        Execute();
    }

    private void Fetch()
    {
        Addr = ProgramCounter;
        Data = Memory.Read(Addr);
        ProgramCounter++;
    }

    private void Decode()
    {
        Instruction = Data;
    }

    private void Execute()
    {
        var instruction = new Instruction(Instruction);
        _instructions.ExecuteOpcode(instruction);
    }
}