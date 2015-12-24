
using System;

namespace Effect
{
	public class BaseEffectNode : TDCInfo
	{
		protected EffectNodeType m_NodeType;

		public EffectNodeType NodeType {
			get { return m_NodeType; }
			set { m_NodeType = value; }
		}

		public BaseEffectNode () : base ()
		{
			m_NodeType = EffectNodeType.None;
		}
	}
}

