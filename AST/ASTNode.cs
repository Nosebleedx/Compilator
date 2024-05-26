using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class ASTNode
	{
		public string Type { get; set; }
		public List<ASTNode> Children { get; set; } = new List<ASTNode>();
	}
}
