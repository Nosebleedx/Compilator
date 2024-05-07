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
		//static public List<Dictionary<>>

		static void Main(string[] args)
		{
			AntlrFileStream antlrInputStream = new AntlrFileStream("programm_text.txt", Encoding.UTF8);
            Console.WriteLine($"programm_text: \n {antlrInputStream} \n \n");
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

			Visitor visitor = new Visitor();
            
            visitor.Visit(tree);

		}
	}
}
