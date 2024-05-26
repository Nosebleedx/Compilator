﻿using Antlr4.Runtime;
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

		public override ASTNode VisitPrintstmt([NotNull] My_grammarParser.PrintstmtContext context)
		{
			var expression = Visit(context.expression());
			var printNode = new PrintStmtNode { Expression = expression };
			return printNode;
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
					Left = new NumberNode { Value = "0", Type = "Number" },
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
			return new NumberNode { Value = context.GetText() };
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
				return new NumberNode
				{
					Value = context.number().GetText(),
					Type = "Number"
				};
			}
			else if (context.expression() != null)
			{
				return Visit(context.expression());
			}

			throw new Exception("Unsupported factor type");
		}
		public override ASTNode VisitProcedure([NotNull] My_grammarParser.ProcedureContext context)
		{	
			// Создаем узел для процедуры
			var procedureNode = new ProcedureNode(context.ident().GetText()) {Body = Visit(context.block()) };

			return procedureNode;
		}


	}




}
