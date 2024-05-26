using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class AssignStmtNode : ASTNode
	{
		public string VarName { get; set; }
		public ASTNode Expression { get; set; }

		public AssignStmtNode()
		{
			Type = "AssignStmt";
		}
	}
}
