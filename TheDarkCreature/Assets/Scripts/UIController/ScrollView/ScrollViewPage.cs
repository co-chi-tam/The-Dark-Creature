using UnityEngine;
using System.Collections;

public class ScrollViewPage : MonoBehaviour {

	[SerializeField]
	private string m_NamePage;
	public string NamePage {
		get { return m_NamePage; }
		set { m_NamePage = value; }
	}

}
