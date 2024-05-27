using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class OddConditionNode : ASTNode
	{
		public ASTNode Expression { get; set; }

		public override string ToString()
		{
			return $"ODD {Expression}";
		}
	}
}
