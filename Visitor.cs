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
		public override IParseTree VisitInitialization([NotNull] My_grammarParser.InitializationContext context)
		{
			
			Console.WriteLine("ID: " + context.ID().GetText());
			Console.WriteLine("TYPE: " + context.TYPE().GetText());
			return base.VisitInitialization(context);
		}
	}
}
