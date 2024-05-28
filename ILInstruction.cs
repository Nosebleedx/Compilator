using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0
{
	internal class ILInstruction
	{
		public OpCode OpCode { get; }
		public string Operand { get; }

		public ILInstruction(OpCode opCode, string operand = null)
		{
			OpCode = opCode;
			Operand = operand;
		}

		public override string ToString()
		{
			return Operand == null ? OpCode.ToString() : $"{OpCode} {Operand}";
		}
	}

	internal enum OpCode
	{
		Halt,
		StoreVar,
		Input,
		Print,
		Add,
		Sub,
		Mul,
		Div,
		Mod,
		LoadConst,
		JmpIf,
		JmpIfLess,
		JmpIfLessOrEqual,
		JmpIfGreater,
		JmpIfGreaterOrEqual,
		JmpIfEqual,
		JmpIfNotEqual,
		JmpIfFalse,
		Jmp,
		Label,
		Call,
		Ret
	}

}
