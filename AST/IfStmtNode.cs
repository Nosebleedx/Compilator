using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class IfStmtNode : ASTNode
	{
		public ASTNode Condition { get; set; }
		public ASTNode ThenStatement { get; set; }

		public IfStmtNode()
		{
			Type = "IfStmt";
		}
	}

}
