using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using COMPILATAR_V1._0.AST;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;


namespace COMPILATAR_V1._0
{

	public class Program
	{
		static void Main(string[] args)
		{
			AntlrFileStream antlrInputStream = new AntlrFileStream("programm_text.txt", Encoding.UTF8);
			Console.WriteLine($"programm_text:\n{antlrInputStream} \n \n");
			My_grammarLexer lexer = new My_grammarLexer(antlrInputStream);

			IToken token1;
			token1 = lexer.NextToken();
			while (token1.Type != My_grammarLexer.Eof)
			{
				Console.WriteLine($"pos:{token1.StartIndex}, type:{token1.Type} - {token1.Text}");
				token1 = lexer.NextToken(); // Читаем следующий токен, то тех пор, пока не дойдем до конца файла.
			}
			// устанавливаю на позицию 0
			lexer.Reset();

			CommonTokenStream commonToken = new CommonTokenStream(lexer);

			My_grammarParser parser = new My_grammarParser(commonToken);

			IParseTree tree = parser.prog();
			ASTBuilder visitor = new ASTBuilder();
			ASTNode ast = visitor.Visit(tree);
			var json = JsonConvert.SerializeObject(ast, Formatting.Indented);
			File.WriteAllText(@"C:\Users\Дмитрий\source\repos\COMPILATAR_V1.0\ast.json", json);
			var generator = new ILGenerator();
			generator.Visit(tree);

			using (StreamWriter sw = new StreamWriter(@"C:\Users\Дмитрий\source\repos\COMPILATAR_V1.0\il_instructions.txt"))
			{
				foreach (var instruction in generator.Instructions)
				{
					sw.WriteLine(instruction);
					Console.WriteLine(instruction);
				}
			}

			// Создание и сохранение DLL
			var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MyDynamicAssembly"), AssemblyBuilderAccess.Save);
			var moduleBuilder = assemblyBuilder.DefineDynamicModule("MyDynamicModule", "MyDynamicAssembly.dll");
			var typeBuilder = moduleBuilder.DefineType("MyDynamicType", TypeAttributes.Public);
			var methodBuilder = typeBuilder.DefineMethod("MyDynamicMethod", MethodAttributes.Public | MethodAttributes.Static, typeof(void), null);
			var ilGenerator = methodBuilder.GetILGenerator();

			Dictionary<string, Label> labels = new Dictionary<string, Label>();

			// Генерация IL кода на основе инструкций AST
			foreach (var instruction in generator.Instructions)
			{
				switch (instruction.OpCode)
				{
					case OpCode.Halt:
						ilGenerator.Emit(OpCodes.Ret);
						break;

					case OpCode.StoreVar:
						ilGenerator.Emit(OpCodes.Stloc, instruction.Operand);
						break;

					case OpCode.Input:
						ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine", Type.EmptyTypes));
						ilGenerator.Emit(OpCodes.Call, typeof(int).GetMethod("Parse", new Type[] { typeof(string) }));
						ilGenerator.Emit(OpCodes.Stloc, int.Parse(instruction.Operand));
						break;

					case OpCode.Print:
						ilGenerator.Emit(OpCodes.Ldloc, int.Parse(instruction.Operand));
						ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }));
						break;

					case OpCode.Add:
						ilGenerator.Emit(OpCodes.Add);
						break;

					case OpCode.Sub:
						ilGenerator.Emit(OpCodes.Sub);
						break;

					case OpCode.Mul:
						ilGenerator.Emit(OpCodes.Mul);
						break;

					case OpCode.Div:
						ilGenerator.Emit(OpCodes.Div);
						break;

					case OpCode.Mod:
						ilGenerator.Emit(OpCodes.Rem);
						break;

					case OpCode.LoadConst:
						if (int.TryParse(instruction.Operand, out int constValue))
						{
							ilGenerator.Emit(OpCodes.Ldc_I4, constValue);
						}
						else
						{
							Console.WriteLine($"Такого нет {instruction.Operand}");
						}
						break;

					case OpCode.JmpIf:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Brtrue, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.JmpIfLess:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Blt, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.JmpIfLessOrEqual:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Ble, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.JmpIfGreater:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Bgt, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.JmpIfGreaterOrEqual:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Bge, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.JmpIfEqual:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Beq, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.JmpIfNotEqual:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Bne_Un, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.JmpIfFalse:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Brfalse, labels[instruction.Operand]);
						}
						else
						{
							Console.WriteLine($"Метка {instruction.Operand} не найдена.");
						}
						break;

					case OpCode.Jmp:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.Emit(OpCodes.Br, labels[instruction.Operand]);
						}
						else
						{
							var label = ilGenerator.DefineLabel();
							labels[instruction.Operand] = label;
							ilGenerator.Emit(OpCodes.Br, label);
						}
						break;

					case OpCode.Label:
						if (labels.ContainsKey(instruction.Operand))
						{
							ilGenerator.MarkLabel(labels[instruction.Operand]);
						}
						else
						{
							var newLabel = ilGenerator.DefineLabel();
							labels[instruction.Operand] = newLabel;
							ilGenerator.MarkLabel(newLabel);
						}
						break;

					case OpCode.Call:
						ilGenerator.Emit(OpCodes.Call, methodBuilder);
						break;

					case OpCode.Ret:
						ilGenerator.Emit(OpCodes.Ret);
						break;

					default:
						Console.WriteLine($"Неизвестная инструкция: {instruction.OpCode}");
						break;
				}
			}


			// Завершение создания типа, метода и сборки
			var dynamicType = typeBuilder.CreateType();
			assemblyBuilder.Save("MyDynamicAssembly.dll");
		}
	}
}
