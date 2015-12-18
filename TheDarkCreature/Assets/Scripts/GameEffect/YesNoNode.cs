using System;
using System.Collections;
using System.Collections.Generic;

namespace Effect
{
	public class YesNoNode: BaseEffectNode
	{
        private EffectParam m_EffectParam;
		private List<BaseEffectNode> m_YesMethods;
		private List<BaseEffectNode> m_NoMethods;

		public List<BaseEffectNode> YesMethods {
			get { return m_YesMethods; }
			set { m_YesMethods = value; }
		}

		public List<BaseEffectNode> NoMethods {
			get { return m_NoMethods; }
			set { m_NoMethods = value; }
		}

        public EffectParam EffectParam
        {
            get { return m_EffectParam; }
            set { m_EffectParam = value; }
        }

		public YesNoNode () : base ()
		{
            EffectParam = new EffectParam();
			m_YesMethods = new List<BaseEffectNode> ();
			m_NoMethods = new List<BaseEffectNode> ();                                     
		}
	}
}

