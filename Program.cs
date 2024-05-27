using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using COMPILATAR_V1._0.AST;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;


namespace COMPILATAR_V1._0
{
	public class Program
	{
		//static public List<Dictionary<>>

		static void Main(string[] args)
		{
			AntlrFileStream antlrInputStream = new AntlrFileStream("third_example.txt", Encoding.UTF8);
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
		}
	}
}
