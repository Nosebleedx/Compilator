using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class WhileStmtNode : ASTNode
	{
		public ASTNode Condition { get; set; }
		public ASTNode Body { get; set; }

		public WhileStmtNode()
		{
			Type = "WhileStmt";
		}
	}
}
