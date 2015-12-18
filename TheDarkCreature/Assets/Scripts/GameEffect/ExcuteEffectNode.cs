using System;
using System.Collections;
using System.Collections.Generic;

namespace Effect
{
	public class ExcuteEffectNode : BaseEffectNode
	{
        private List<EffectParam> m_ExcuteMethods;

        public List<EffectParam> ExcuteMethods {
			get { return m_ExcuteMethods; }
			set { m_ExcuteMethods = value; }
		}

		public ExcuteEffectNode () : base ()
		{
            m_ExcuteMethods = new List<EffectParam> ();
		}
	}
}

