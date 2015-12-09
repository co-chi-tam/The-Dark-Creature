using System;
using System.Collections;
using System.Collections.Generic;

namespace Effect
{
	public class EffectManager
	{
		private Dictionary<string, Func<bool>> m_ConditionMethods;
		private Dictionary<string, Action<object[]>> m_ExcuteMethods;
		private EffectReader m_Reader;

		public EffectManager ()
		{
			m_ConditionMethods = new Dictionary<string, Func<bool>> ();
			m_ExcuteMethods = new Dictionary<string, Action<object[]>> ();
			m_Reader = new EffectReader ();
		}

		public void LoadEffect(string path) {
			m_Reader.LoadJson (path);
		}

		public void RegisterCondition(string methodName, Func<bool> func) {
			m_ConditionMethods [methodName] = func;
		}

		public void RegisterExcuteMethod(string methodName, Action<object[]> func) {
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
			case EffectNodeType.YesNoCondition: {
				var yesNoNode = parent as YesNoNode;
				var methodNode = m_ConditionMethods [yesNoNode.Method];
				if (methodNode ()) {
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
			} break;
			case EffectNodeType.ExcuteEffectNode: {
				var excuteNode = parent as ExcuteEffectNode;
				var playerMethods = excuteNode.PlayerMethods;
				var gameMethods = excuteNode.GameMethods;
				for (int i = 0; i < playerMethods.Count; i++) {
					var methodName = string.Empty;
					var par = ParseStringToObjects (playerMethods[i], out methodName);
					m_ExcuteMethods [methodName](par);
				}
				for (int j = 0; j < gameMethods.Count; j++) {
					var methodName = string.Empty;
					var par = ParseStringToObjects (gameMethods[j], out methodName);
					m_ExcuteMethods [methodName](par);
				}
			} break;
			} 
		}

		private object[] ParseStringToObjects (string data, out string methodName) {
			var dataMethod = data.Split('(');
			methodName = dataMethod [0];
			var paramsName = dataMethod [1].Remove (dataMethod [1].Length - 1, 1);
			var paramsDatas = paramsName.Split (',');
			var result = new object[paramsDatas.Length * 2];
			for (int i = 0, j = 0; i < result.Length; i+=2, j++) {
				var argument = paramsDatas[j].Split(':');
				result[i] = argument[0];
				result[i + 1] = argument[1]; 
			}
			return result;
		}
	}
}

