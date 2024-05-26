﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILATAR_V1._0.AST
{
	public class CallStmtNode : ASTNode
	{
		public string ProcedureName { get; set; }
		public CallStmtNode(string procedureName)
		{
			Type = "CallStmt";
			ProcedureName = procedureName;
		}
	}
}
