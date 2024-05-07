using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0
{
	internal class Visitor : My_grammarBaseVisitor<IParseTree>
	{
		public override IParseTree VisitProg([NotNull] My_grammarParser.ProgContext context)
		{

			return base.VisitChildren(context);
		}
		public override IParseTree VisitBlock([NotNull] My_grammarParser.BlockContext context)
		{
            return base.VisitBlock(context);
		}
		public override IParseTree VisitAssignstmt([NotNull] My_grammarParser.AssignstmtContext context)
		{
            return base.VisitAssignstmt(context);
		}
		public override IParseTree VisitBeginstmt([NotNull] My_grammarParser.BeginstmtContext context)
		{
			return base.VisitBeginstmt(context);
		}
		public override IParseTree VisitStat([NotNull] My_grammarParser.StatContext context)
		{
			return base.VisitStat(context);
		}
		public override IParseTree VisitNumber([NotNull] My_grammarParser.NumberContext context)
		{
			return base.VisitNumber(context);
		}
		public override IParseTree VisitChildren([NotNull] IRuleNode node)
		{
			return base.VisitChildren(node);
		}

		public override IParseTree VisitVars_([NotNull] My_grammarParser.Vars_Context context)
		{
			return base.VisitVars_(context);
		}
		public override IParseTree VisitExpression([NotNull] My_grammarParser.ExpressionContext context)
		{
			return base.VisitExpression(context);
		}


	}
}
