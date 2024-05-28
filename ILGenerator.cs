using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using COMPILATAR_V1._0.AST;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0
{
	internal class ILGenerator : My_grammarBaseVisitor<ASTNode>
	{
		private List<ILInstruction> _instructions = new List<ILInstruction>();
		private Dictionary<string, string> _labels = new Dictionary<string, string>();
		private int _labelCounter = 0;

		public IReadOnlyList<ILInstruction> Instructions => _instructions;

		private string GetNewLabel()
		{
			return "Label" + _labelCounter++;
		}

		public override ASTNode VisitProg([NotNull] My_grammarParser.ProgContext context)
		{
			foreach (var child in context.children)
			{
				Visit(child);
			}
			_instructions.Add(new ILInstruction(OpCode.Halt));
			return null; // Или возвращайте другой тип, если требуется
		}

		public override ASTNode VisitBeginstmt([NotNull] My_grammarParser.BeginstmtContext context)
		{
			foreach (var child in context.children)
			{
				Visit(child);
			}
			return null;
		}

		public override ASTNode VisitVars_([NotNull] My_grammarParser.Vars_Context context)
		{
			// Do nothing for variable declaration
			return null;
		}

		public override ASTNode VisitAssignstmt([NotNull] My_grammarParser.AssignstmtContext context)
		{
			Visit(context.expression());
			_instructions.Add(new ILInstruction(OpCode.StoreVar, context.ident().GetText()));
			return null;
		}

		public override ASTNode VisitQstmt([NotNull] My_grammarParser.QstmtContext context)
		{
			_instructions.Add(new ILInstruction(OpCode.Input, context.ident().GetText()));
			return null;
		}

		public override ASTNode VisitPrintstmt([NotNull] My_grammarParser.PrintstmtContext context)
		{
			Visit(context.expression());
			_instructions.Add(new ILInstruction(OpCode.Print));
			return null;
		}

		public override ASTNode VisitExpression([NotNull] My_grammarParser.ExpressionContext context)
		{
			// Сначала посещаем первый терм
			Visit(context.term(0));

			// Проходим по остальным термам и операторам
			for (int i = 1; i < context.term().Length; i++)
			{
				var operatorToken = context.GetChild(2 * i - 1).GetText();
				Visit(context.term(i));

				// Генерация инструкции для оператора
				switch (operatorToken)
				{
					case "+":
						_instructions.Add(new ILInstruction(OpCode.Add));
						break;
					case "-":
						_instructions.Add(new ILInstruction(OpCode.Sub));
						break;
					case "*":
						_instructions.Add(new ILInstruction(OpCode.Mul));
						break;
					case "/":
						_instructions.Add(new ILInstruction(OpCode.Div));
						break;
					case "%":
						_instructions.Add(new ILInstruction(OpCode.Mod));
						break;
					default:
						throw new InvalidOperationException($"Неизвестный оператор: {operatorToken}");
				}
			}

			// Обработка унарного минуса
			if (context.GetChild(0).GetText() == "-" && context.term().Length == 1)
			{
				_instructions.Add(new ILInstruction(OpCode.LoadConst, "0"));
				_instructions.Add(new ILInstruction(OpCode.Sub));
			}

			return null;
		}

		public override ASTNode VisitCondition([NotNull] My_grammarParser.ConditionContext context)
		{
			Visit(context.children[0]);
			Visit(context.children[2]);
			var op = context.children[1].GetText();
			switch (op)
			{
				case "<":
					_instructions.Add(new ILInstruction(OpCode.JmpIfLess));
					break;
				case "<=":
					_instructions.Add(new ILInstruction(OpCode.JmpIfLessOrEqual));
					break;
				case ">":
					_instructions.Add(new ILInstruction(OpCode.JmpIfGreater));
					break;
				case ">=":
					_instructions.Add(new ILInstruction(OpCode.JmpIfGreaterOrEqual));
					break;
				case "==":
					_instructions.Add(new ILInstruction(OpCode.JmpIfEqual));
					break;
				case "!=":
					_instructions.Add(new ILInstruction(OpCode.JmpIfNotEqual));
					break;
				case "=":
					_instructions.Add(new ILInstruction(OpCode.StoreVar)); // Обработка оператора присваивания
					break;
				default:
					throw new InvalidOperationException($"Неизвестный оператор: {op}");
			}

			return null;
		}



		public override ASTNode VisitWhilestmt([NotNull] My_grammarParser.WhilestmtContext context)
		{
			var startLabel = GetNewLabel();
			var endLabel = GetNewLabel();

			_instructions.Add(new ILInstruction(OpCode.Label, startLabel));
			Visit(context.condition());
			_instructions.Add(new ILInstruction(OpCode.JmpIfFalse, endLabel));
			Visit(context.stat());
			_instructions.Add(new ILInstruction(OpCode.Jmp, startLabel));
			_instructions.Add(new ILInstruction(OpCode.Label, endLabel));

			return null;
		}

		public override ASTNode VisitIfstmt([NotNull] My_grammarParser.IfstmtContext context)
		{
			var endLabel = GetNewLabel();
			Visit(context.condition());
			_instructions.Add(new ILInstruction(OpCode.JmpIfFalse, endLabel));
			Visit(context.stat());
			_instructions.Add(new ILInstruction(OpCode.Label, endLabel));
			return null;
		}

		public override ASTNode VisitCallstmt([NotNull] My_grammarParser.CallstmtContext context)
		{
			_instructions.Add(new ILInstruction(OpCode.Call, context.ident().GetText()));
			return null;
		}

		public override ASTNode VisitProcedure([NotNull] My_grammarParser.ProcedureContext context)
		{
			var procedureName = context.ident().GetText();
			var startLabel = GetNewLabel();
			_labels[procedureName] = startLabel;

			_instructions.Add(new ILInstruction(OpCode.Label, startLabel));
			Visit(context.block());
			_instructions.Add(new ILInstruction(OpCode.Ret));

			return null;
		}



	}
}
