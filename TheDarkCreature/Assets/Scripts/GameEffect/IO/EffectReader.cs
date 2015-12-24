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
			var id = int.Parse (nodeData ["ID"].ToString ());
			var type = (EffectNodeType) byte.Parse (nodeData ["Type"].ToString ());
			
            switch (type)
            {
                case EffectNodeType.YesNoCondition:
                    {
                        YesNoNode yesNo = null;
                        if (firstNode)
                        {
                            m_CurrentNode = new YesNoNode();
                            yesNo = m_CurrentNode as YesNoNode;
                        }
                        else
                        {
                            yesNo = new YesNoNode();
                        }
                        yesNo.ID = id;
                        yesNo.NodeType = type;
                        var methodName = string.Empty;
                        var pars = ParseStringToObjects(nodeData["Method"].ToString(), out methodName);
                        yesNo.EffectParam.Method = methodName;
                        yesNo.EffectParam.Params = pars;
                        var yesMethods = nodeData["YesMethods"] as List<object>;
                        var noMethods = nodeData["NoMethods"] as List<object>;
                        for (int i = 0; i < yesMethods.Count; i++)
                        {
                            var yesData = yesMethods[i] as Dictionary<string, object>;
                            yesNo.YesMethods.Add(ReadNode(yesData));
                        }
                        for (int i = 0; i < noMethods.Count; i++)
                        {
                            var noData = noMethods[i] as Dictionary<string, object>;
                            yesNo.NoMethods.Add(ReadNode(noData));
                        }
                        return yesNo;
                    }
                case EffectNodeType.ExcuteEffectNode:
                    {
                        ExcuteEffectNode excuteMethod = null;
                        if (firstNode)
                        {
                            m_CurrentNode = new ExcuteEffectNode();
                            excuteMethod = m_CurrentNode as ExcuteEffectNode;
                        }
                        else
                        {
                            excuteMethod = new ExcuteEffectNode();
                        }
                        excuteMethod.ID = id;
                        excuteMethod.NodeType = type;
                        var pars = nodeData["ExcuteMethods"] as List<object>;
                        for (int i = 0; i < pars.Count; i++)
                        {
                            var methodName = string.Empty;
                            var par = ParseStringToObjects(pars[i].ToString(), out methodName);
                            excuteMethod.ExcuteMethods.Add(new EffectParam() { Method = methodName, Params = par });
                        }
                        return excuteMethod;
                    }
			}
			return null;
		}

        private object[] ParseStringToObjects (string data, out string methodName) {
            var dataMethod = data.Split('(');
            methodName = dataMethod [0];
            var paramsName = dataMethod [1].Remove (dataMethod [1].Length - 1, 1);
            var paramsDatas = paramsName.Split (',');
            var result = new object[paramsDatas.Length * 2];
            if (string.IsNullOrEmpty (paramsDatas[0]))
                return null;
            for (int i = 0, j = 0; i < result.Length; i+=2, j++) {
                var argument = paramsDatas[j].Split(':');
                result[i] = argument[0];
                result[i + 1] = argument[1]; 
            }
            return result;
        }
	}
}
