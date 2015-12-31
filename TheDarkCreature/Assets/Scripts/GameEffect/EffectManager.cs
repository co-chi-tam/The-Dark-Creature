using System;
using System.Collections;
using System.Collections.Generic;

namespace Effect
{
	public class EffectManager
	{
		private Dictionary<string, Func<Dictionary<string, object>, bool>> m_ConditionMethods;
		private Dictionary<string, Action<Dictionary<string, object>>> m_ExcuteMethods;
		private EffectReader m_Reader;

		public EffectManager ()
		{
			m_ConditionMethods = new Dictionary<string, Func<Dictionary<string, object>, bool>> ();
			m_ExcuteMethods = new Dictionary<string, Action<Dictionary<string, object>>> ();
			m_Reader = new EffectReader ();
		}

		public void LoadEffect(string path) {
			m_Reader.LoadJson (path);
		}

		public void RegisterCondition(string methodName, Func<Dictionary<string, object>, bool> func) {
			m_ConditionMethods [methodName] = func;
		}

		public void RegisterExcuteMethod(string methodName, Action<Dictionary<string, object>> func) {
			m_ExcuteMethods [methodName] = func;
		}

		public void ExcuteEffect() {
			if (m_Reader == null)
				return;
			ExcuteEffectNode (m_Reader.CurrentNode);
		}

		private void ExcuteEffectNode(BaseEffectNode parent) {
			var type = parent.NodeType;
			switch (type) {
    			case EffectNodeType.YesNoCondition: 
                    {
        				var yesNoNode = parent as YesNoNode;
                        var methodNode = m_ConditionMethods [yesNoNode.EffectParam.Method](yesNoNode.EffectParam.Params);
        				if (methodNode) {
        					var yesMethods = yesNoNode.YesMethods;
        					for (int i = 0; i < yesMethods.Count; i++) {
        						ExcuteEffectNode (yesMethods [i]);
        					}
        				} else {
        					var noMethods = yesNoNode.NoMethods;
        					for (int i = 0; i < noMethods.Count; i++) {
        						ExcuteEffectNode (noMethods [i]);
        					}
        				}
        			} 
                    break;
    			case EffectNodeType.ExcuteEffectNode:
                    {
        				var excuteNode = parent as ExcuteEffectNode;
        				var playerMethods = excuteNode.ExcuteMethods;
        				for (int i = 0; i < playerMethods.Count; i++) {
                            m_ExcuteMethods [playerMethods[i].Method](playerMethods[i].Params);
        				}
    			    } 
                    break;
			} 
		}

	}
}

