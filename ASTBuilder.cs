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
				var childNode = Visit(child);
				if (childNode != null)
				{
					node.Children.Add(childNode);
				}
			}
			return node;
		}

		public override ASTNode VisitVars_([NotNull] My_grammarParser.Vars_Context context)
		{
			var varNode = new ASTNode { Type = "Vars" };
			foreach (var varDecl in context.ident())
			{
				varNode.Children.Add(new VarNode(varDecl.GetText()));
			}
			return varNode;
		}

		public override ASTNode VisitAssignstmt([NotNull] My_grammarParser.AssignstmtContext context)
		{
			var varName = context.ident().GetText();
			var expression = Visit(context.expression());
			return new AssignStmtNode(varName, expression);
		}

		public override ASTNode VisitBeginstmt([NotNull] My_grammarParser.BeginstmtContext context)
		{
			var beginNode = new BeginStmtNode();
			foreach (var stat in context.stat())
			{
				var statNode = Visit(stat);
				if (statNode != null)
				{
					beginNode.Children.Add(statNode);
				}
			}
			return beginNode;
		}

		public override ASTNode VisitStat([NotNull] My_grammarParser.StatContext context)
		{
			return VisitChildren(context);
		}

		public override ASTNode VisitNumber([NotNull] My_grammarParser.NumberContext context)
		{
			return new NumberNode(context.NUMBER().GetText());
		}

		public override ASTNode VisitExpression([NotNull] My_grammarParser.ExpressionContext context)
		{
			var exprNode = new ExpressionNode();
			foreach (var child in context.children)
			{
				var childNode = Visit(child);
				if (childNode != null)
				{
					exprNode.Children.Add(childNode);
				}
			}
			return exprNode;
		}
	}


}
