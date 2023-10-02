using System.Text;

namespace CPU;

public class Instruction
{
    public Opcode Opcode { get; }
    public ushort[] Arguments { get; }
    public ushort Data { get; }

    public Instruction(ushort instruction)
    {
        Opcode = (Opcode)(ushort)(instruction >> 8);
        var arg = (ushort)((instruction >> 4) & 0xF);
        Arguments = new []
        {
            (ushort)(arg >> 2),
            (ushort)(arg & 0x3)
        };
        Data = (ushort)(instruction & 0xF);
    }
}

public class Instructions
{
    private readonly Cpu _cpu;
    
    private delegate void OpcodeFunction(ushort[] args, ushort data);
    private readonly IDictionary<Opcode, OpcodeFunction?> _opcodeToFunctionMap;

    public Instructions(Cpu cp)
    {
        _cpu = cp;
        
        _opcodeToFunctionMap = new Dictionary<Opcode, OpcodeFunction?>
        {
            { Opcode.Read, Read },
            { Opcode.Write, Write },
            { Opcode.Cmp, Compare},
            { Opcode.CopyToDx, CopyToDx },
            { Opcode.CopyFromAddress, CopyFromAddress },
            { Opcode.CxToDx, CxToDx },
            { Opcode.DxToAddress, DxToAddress },
            { Opcode.DxToCx, DxToCx },
            { Opcode.Jmp, Jmp },
            { Opcode.Add, Add },
            { Opcode.Inc, Increment },
            { Opcode.Not, Not },
            { Opcode.Mov, Mov },
            { Opcode.Exit, Exit }
        };
    }

    public void ExecuteOpcode(Instruction instruction)
    {
        if (_opcodeToFunctionMap.TryGetValue(instruction.Opcode, out var function))
        {
            function?.Invoke(instruction.Arguments, instruction.Data);
        }
        else
        {
            Console.WriteLine("Opcode not supported: " + instruction.Opcode);
        }
    }

    private void Read(ushort[] args, ushort data)
    {
        var isNumber = args[0];
        var register = args[1];

        _cpu.Registers[register] = Convert.ToUInt16("3"); //Console.ReadLine()
    }

    private void Write(ushort[] args, ushort data)
    {
        Console.WriteLine(_cpu.Registers[args[1]]);
    }

    private void Compare(ushort[] args, ushort data)
    {
        _cpu.Flags.EqualFlag = _cpu.Registers[args[0]] == _cpu.Registers[args[1]];
    }

    private void CopyToDx(ushort[] args, ushort data)
    {
        _cpu.Registers[3] = data;
    }

    private void CopyFromAddress(ushort[] args, ushort data)
    {
        var arg = (ushort)((args[0] << 2) | args[1]);

        _cpu.Registers[3] = new Instruction(Memory.Read(arg)).Data;
    }

    private void CxToDx(ushort[] args, ushort data)
    {
        _cpu.Registers[3] = Memory.Read(_cpu.Registers[2]);
    }

    private void DxToAddress(ushort[] args, ushort data)
    {
        var arg = (ushort)((args[0] << 2) | args[1]);
        Memory.Write(arg, _cpu.Registers[3]);
    }

    private void DxToCx(ushort[] args, ushort data)
    {
        Memory.Write(_cpu.Registers[2], _cpu.Registers[3]);
    }

    private void Jmp(ushort[] args, ushort data)
    {
        var arg = (ushort)((args[0] << 2) | args[1]);
        if (!_cpu.Flags.EqualFlag)
        {
            _cpu.ProgramCounter = arg;
        }
    }

    private void Add(ushort[] args, ushort data)
    {
        _cpu.Registers[0] = (ushort)(_cpu.Registers[0] + _cpu.Registers[args[1]]);
    }

    private void Exit(ushort[] args, ushort data)
    {
        Environment.Exit(0);
    }

    private void Increment(ushort[] args, ushort data)
    {
        _cpu.Registers[args[1]]++;
    }

    private void Not(ushort[] args, ushort data)
    {
        _cpu.Registers[args[1]] = (ushort)(_cpu.Registers[args[1]] ^ ushort.MaxValue);
    }

    private void Mov(ushort[] args, ushort data)
    {
        _cpu.Registers[args[1]] = _cpu.Registers[3];
    }
}
