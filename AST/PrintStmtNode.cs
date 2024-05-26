using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class PrintStmtNode : ASTNode
	{
		public ASTNode Expression { get; set; }
		public PrintStmtNode()
		{
			Type = "PrintStmt";
		}
	}
}
