
using System;

namespace Effect
{
	public class BaseEffectNode
	{
		protected int m_ID;
		protected EffectNodeType m_NodeType;

		public EffectNodeType NodeType {
			get { return m_NodeType; }
			set { m_NodeType = value; }
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public BaseEffectNode ()
		{
			m_ID = 0;
			m_NodeType = EffectNodeType.None;
		}
	}
}

