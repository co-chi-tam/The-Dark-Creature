using System;
using System.Collections;
using System.Collections.Generic;

namespace Effect
{
	public class YesNoNode: BaseEffectNode
	{
		private string m_Method;
		private List<BaseEffectNode> m_YesMethods;
		private List<BaseEffectNode> m_NoMethods;

		public string Method {
			get { return m_Method; }
			set { m_Method = value; }
		}

		public List<BaseEffectNode> YesMethods {
			get { return m_YesMethods; }
			set { m_YesMethods = value; }
		}

		public List<BaseEffectNode> NoMethods {
			get { return m_NoMethods; }
			set { m_NoMethods = value; }
		}

		public YesNoNode () : base ()
		{
			m_YesMethods = new List<BaseEffectNode> ();
			m_NoMethods = new List<BaseEffectNode> ();                                     
		}
	}
}

