using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class NumberNode : ASTNode
	{
		public string Value { get; set; }
		public NumberNode(string value)
		{
			Type = "Number";
			Value = value;
		}
	}
}
