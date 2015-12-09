using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Effect 
{
	public enum EffectNodeType : byte {
		None = 0,
		YesNoCondition = 1,
		// TODO
		ExcuteEffectNode = 10
	}

	public class EffectReader {

		private BaseEffectNode m_CurrentNode;

		public BaseEffectNode CurrentNode {
			get { return m_CurrentNode; }
			set { m_CurrentNode = value; }
		}

		public EffectReader ()
		{

		}

		public void LoadJson(string path) {
			var textAsset = Resources.Load <TextAsset> (path);
			var jsonDeserialize = MiniJSON.Json.Deserialize (textAsset.text) as Dictionary<string, object>;
			var effect = jsonDeserialize["effect"] as Dictionary<string, object>;
			// Read current node - first node
			ReadNode (effect, true);
		}

		private BaseEffectNode ReadNode(Dictionary<string, object> nodeData, bool firstNode = false) {
			var id = int.Parse (nodeData ["Id"].ToString ());
			var type = (EffectNodeType) byte.Parse (nodeData ["Type"].ToString ());
			
			switch (type) {
			case EffectNodeType.YesNoCondition:
				YesNoNode yesNo = null;
				if (firstNode) {
					m_CurrentNode = new YesNoNode();
					yesNo = m_CurrentNode as YesNoNode;
				} else {
					yesNo = new YesNoNode();
				}
				yesNo.ID = id;
				yesNo.NodeType = type;
				yesNo.Method = nodeData["Method"].ToString ();
				var yesMethods = nodeData["YesMethods"] as List<object>;
				var noMethods = nodeData["NoMethods"] as List<object>;
				for (int i = 0; i < yesMethods.Count; i++) {
					var yesData = yesMethods[i] as Dictionary<string, object>;
					yesNo.YesMethods.Add (ReadNode (yesData));
				}
				for (int i = 0; i < noMethods.Count; i++) {
					var noData = noMethods[i] as Dictionary<string, object>;
					yesNo.NoMethods.Add (ReadNode (noData));
				}
				return yesNo;
			case EffectNodeType.ExcuteEffectNode:
				ExcuteEffectNode excuteMethod = null;
				if (firstNode) {
					m_CurrentNode = new ExcuteEffectNode();
					excuteMethod = m_CurrentNode as ExcuteEffectNode;
				} else {
					excuteMethod = new ExcuteEffectNode();
				}
				excuteMethod.ID = id;
				excuteMethod.NodeType = type;
				excuteMethod.PlayerMethods = (nodeData["PlayerMethods"] as List<object>).OfType<string>().ToList();
				excuteMethod.GameMethods = (nodeData["GameMethods"] as List<object>).OfType<string>().ToList();
				return excuteMethod;
			}
			return null;
		}

	}
}
