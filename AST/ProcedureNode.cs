using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class ProcedureNode : ASTNode
	{
		public string Name { get; set; }
		public ProcedureNode(string name)
		{
			Type = "Procedure";
			Name = name;
		}
	}
}
