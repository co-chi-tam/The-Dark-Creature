using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	#region Singleton
	public static object m_objectSingleton = new object();
	public static UIController m_Instance;

	public static UIController Instance {
		get {
			lock (m_objectSingleton)
			{
				if (m_Instance == null) {
					GameObject go = new GameObject ("UIController");
					m_Instance = go.AddComponent <UIController>();
				}
				return m_Instance;
			}
		}
	}

	public static UIController GetInstance() {
		return m_Instance;
	}

	public UIController ()
	{
		m_Instance = this;
	}
	#endregion

}
