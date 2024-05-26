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
			if(context.stat() != null)
			{
				beginNode.Children.Add(Visit(context.stat()));
			}
			return beginNode;
		}

		public override ASTNode VisitAssignstmt([NotNull] My_grammarParser.AssignstmtContext context)
		{
			var varName = context.ident().GetText();
			var expression = Visit(context.expression());
			var assignNode = new AssignStmtNode(varName, expression);
			return assignNode;
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
			if (context.children.Count == 1)
			{
				return Visit(context.children[0]);
			}
			else if (context.children.Count == 3)
			{
				var left = Visit(context.children[0]);
				var op = context.children[1].GetText();
				var right = Visit(context.children[2]);

				var expressionNode = new ExpressionNode
				{
					Left = left,
					Operator = op,
					Right = right
				};
				return expressionNode;
			}
			throw new InvalidOperationException("Неизвестное выражение");
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

		
	}




}
