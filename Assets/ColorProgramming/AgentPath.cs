using System.Collections.Generic;
using ColorProgramming;
using ColorProgramming.Core;
using UnityEngine;

namespace ColorProgramming { 

	public class AgentPath 
	{
		public List<BaseNodeController> RootPath;
		public Dictionary<CapsuleNode, List<BaseNodeController>> SubPaths;
	}
}


