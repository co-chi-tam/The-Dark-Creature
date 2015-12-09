using System;
using System.Collections;
using System.Collections.Generic;

namespace Effect
{
	public class ExcuteEffectNode : BaseEffectNode
	{
		private List<string> m_PlayerMethods;
		private List<string> m_GameMethods;

		public List<string> PlayerMethods {
			get { return m_PlayerMethods; }
			set { m_PlayerMethods = value; }
		}

		public List<string> GameMethods {
			get { return m_GameMethods; }
			set { m_GameMethods = value; }
		}

		public ExcuteEffectNode () : base ()
		{
			m_PlayerMethods = new List<string> ();
			m_GameMethods = new List<string> ();
		}
	}
}

