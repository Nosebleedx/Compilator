using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0
{
	internal class Program
	{
		static void Main(string[] args)
		{
			AntlrFileStream antlrInputStream = new AntlrFileStream("programm_text.txt", Encoding.UTF8);

			My_grammarLexer lexer = new My_grammarLexer(antlrInputStream);

			CommonTokenStream commonToken = new CommonTokenStream(lexer);

			My_grammarParser parser = new My_grammarParser(commonToken);			

			IParseTree tree = parser.prog();

			Visitor visitor = new Visitor();
			visitor.Visit(tree);
		}
	}
}
