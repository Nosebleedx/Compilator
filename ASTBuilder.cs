using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using COMPILATAR_V1._0.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0
{
	internal class ASTBuilder : My_grammarBaseVisitor<ASTNode>
	{
		public override ASTNode VisitProg([NotNull] My_grammarParser.ProgContext context)
		{
			var node = new ProgramNode();
			foreach (var child in context.children)
			{
				node.Children.Add(Visit(child));
			}
			return node;
		}

		public override ASTNode VisitBlock([NotNull] My_grammarParser.BlockContext context)
		{
			var beginNode = new BeginStmtNode();

			// Обрабатываем константы, если они есть
			if (context.consts() != null)
			{
				beginNode.Children.Add(Visit(context.consts()));
			}

			// Обрабатываем переменные, если они есть
			if (context.vars_() != null)
			{
				beginNode.Children.Add(Visit(context.vars_()));
			}

			// Обрабатываем процедуры, если они есть
			foreach (var proc in context.procedure())
			{
				beginNode.Children.Add(Visit(proc));
			}

			// Обрабатываем операторы, если они есть
			if (context.stat() != null)
			{
				beginNode.Children.Add(Visit(context.stat()));
			}

			return beginNode;
		}


		public override ASTNode VisitConsts(My_grammarParser.ConstsContext context)
		{
			var constBlockNode = new ConstBlockNode();

			foreach (var constDecl in context.const_decl())
			{
				var constNode = Visit(constDecl) as ConstNode;
				if (constNode != null)
				{
					constBlockNode.Children.Add(constNode);
				}
			}

			return constBlockNode;
		}

		public override ASTNode VisitConst_decl(My_grammarParser.Const_declContext context)
		{
			var constNode = new ConstNode
			{
				VarName = context.ident().GetText(),
				Value = context.number().GetText()
			};

			return constNode;
		}
		public override ASTNode VisitWritestmt([NotNull] My_grammarParser.WritestmtContext context)
		{
			var writeStmtNode = new WriteStmtNode
			{
				Expression = Visit(context.ident()) // Assuming the write statement writes an identifier
			};

			return writeStmtNode;
		}

		

		public override ASTNode VisitAssignstmt([NotNull] My_grammarParser.AssignstmtContext context)
		{
			var varName = context.ident().GetText();
			var expressionNode = Visit(context.expression());

			return new AssignStmtNode
			{
				VarName = varName,
				Expression = expressionNode,
				Type = "AssignStmt"
			};
		}
		public override ASTNode VisitCallstmt([NotNull] My_grammarParser.CallstmtContext context)
		{
			var procedureName = context.ident().GetText();
			var callNode = new CallStmtNode(procedureName);
			return callNode;
		}
		public override ASTNode VisitQstmt([NotNull] My_grammarParser.QstmtContext context)
		{
			var varName = Visit(context.ident());
			return new InputStmtNode { Expression = varName };
		}
		public override ASTNode VisitPrintstmt([NotNull] My_grammarParser.PrintstmtContext context)
		{
			var expression = Visit(context.expression());
			var printNode = new PrintStmtNode { Expression = expression };
			return printNode;
		}

		public override ASTNode VisitIfstmt([NotNull] My_grammarParser.IfstmtContext context)
		{
			var ifStmtNode = new IfStmtNode();

			// Visit the condition and add it to the IfStmtNode
			ifStmtNode.Condition = Visit(context.condition());

			// Visit the then statement and add it to the IfStmtNode
			ifStmtNode.ThenStatement = Visit(context.stat());

			return ifStmtNode;
		}
		
		public override ASTNode VisitWhilestmt([NotNull] My_grammarParser.WhilestmtContext context)
		{
			var condition = Visit(context.condition());
			var body = Visit(context.stat());

			var whileNode = new WhileStmtNode
			{
				Condition = condition,
				Body = body
			};
			return whileNode;
		}

		public override ASTNode VisitCondition([NotNull] My_grammarParser.ConditionContext context)
		{

			if (context.children.Count == 3)
			{
				var left = Visit(context.children[0]);
				var op = context.children[1].GetText();
				var right = Visit(context.children[2]);

				var conditionNode = new ExpressionNode
				{
					Left = left,
					Operator = op,
					Right = right
				};
				return conditionNode;
			}
			throw new InvalidOperationException("Неизвестное условие");
		}
		
		public override ASTNode VisitTerm([NotNull] My_grammarParser.TermContext context)
		{
			ASTNode node = Visit(context.factor(0));

			for (int i = 1; i < context.factor().Length; i++)
			{
				var operatorToken = context.GetChild(2 * i - 1).GetText();
				var rightNode = Visit(context.factor(i));

				node = new ExpressionNode
				{
					Left = node,
					Operator = operatorToken,
					Right = rightNode,
					Type = "Expression"
				};
			}

			return node;
		}

		public override ASTNode VisitFactor([NotNull] My_grammarParser.FactorContext context)
		{
			if (context.ident() != null)
			{
				return new VariableNode
				{
					Value = context.ident().GetText(),
					Type = "Variable"
				};
			}
			else if (context.number() != null)
			{
				// Преобразование текста числа в числовое значение
				if (int.TryParse(context.number().GetText(), out int numberValue))
				{
					return new NumberNode
					{
						Value = numberValue,
						Type = "Number"
					};
				}
				else
				{
					// Обработка ошибки в случае невозможности преобразования текста в число
					throw new Exception("Invalid number format");
				}
			}
			else if (context.expression() != null)
			{
				return Visit(context.expression());
			}

			throw new Exception("Unsupported factor type");
		}


		public override ASTNode VisitExpression([NotNull] My_grammarParser.ExpressionContext context)
		{
			ASTNode node = Visit(context.term(0));

			for (int i = 1; i < context.term().Length; i++)
			{
				var operatorToken = context.GetChild(2 * i - 1).GetText();
				var rightNode = Visit(context.term(i));

				node = new ExpressionNode
				{
					Left = node,
					Operator = operatorToken,
					Right = rightNode,
					Type = "Expression"
				};
			}

			if (context.GetChild(0).GetText() == "-" && context.term().Length == 1)
			{
				// Handle unary minus
				node = new ExpressionNode
				{
					Left = new NumberNode { Value = 0},
					Operator = "-",
					Right = node,
					Type = "Expression"
				};
			}

			return node;
		}

		public override ASTNode VisitBeginstmt([NotNull] My_grammarParser.BeginstmtContext context)
		{
			var beginNode = new BeginStmtNode();
			foreach (var child in context.stat())
			{
				beginNode.Children.Add(Visit(child));
			}
			return beginNode;
		}

		public override ASTNode VisitNumber([NotNull] My_grammarParser.NumberContext context)
		{
			// Проверяем, является ли текст контекста числом
			if (int.TryParse(context.GetText(), out int number))
			{
				// Если текст контекста можно преобразовать в число, создаем узел для числа
				return new NumberNode { Value = number };
			}
			else
			{
				// Если текст контекста не является числом, выбрасываем исключение или выполняем другую обработку по вашему усмотрению
				throw new InvalidOperationException("Invalid number format");
			}
		}



		public override ASTNode VisitIdent([NotNull] My_grammarParser.IdentContext context)
		{
			return new VariableNode { Value = context.GetText() };
		}
		public override ASTNode VisitVars_([NotNull] My_grammarParser.Vars_Context context)
		{
			// Создаем узел для блока переменных
			var varBlockNode = new VarBlockNode();

			// Проходим по всем идентификаторам в блоке VAR
			foreach (var ident in context.ident())
			{
				// Добавляем каждый идентификатор как дочерний узел к блоку переменных
				var varNode = new VariableNode
				{
					Value = ident.GetText(),
					Type = "Variable"
				};
				varBlockNode.Children.Add(varNode);
			}

			return varBlockNode;
		}
		
		public override ASTNode VisitProcedure([NotNull] My_grammarParser.ProcedureContext context)
		{	
			// Создаем узел для процедуры
			var procedureNode = new ProcedureNode(context.ident().GetText()) {Body = Visit(context.block()) };

			return procedureNode;
		}


	}




}
