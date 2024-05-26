using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class ExpressionNode : ASTNode
	{
		public ASTNode Left { get; set; }
		public string Operator { get; set; }
		public ASTNode Right { get; set; }

		public ExpressionNode()
		{
			Type = "Expression";
		}
	}
}
