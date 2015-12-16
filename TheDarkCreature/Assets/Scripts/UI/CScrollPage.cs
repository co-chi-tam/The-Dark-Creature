using UnityEngine;
using System.Collections;

public class CScrollPage : MonoBehaviour {

	public CUIImageDownloader m_ImageDownload;
	[SerializeField]
	private GameObject[] m_ChildGameObjects;

	public void SetActive(int value) {
//		for (int i = 0; i < m_ChildGameObjects.Length; i++) {
//			m_ChildGameObjects[i].SetActive (value);
//		}

		if (value == 2) {
			this.gameObject.transform.SetAsLastSibling();
		} else if (value == 0) {
			this.gameObject.transform.SetAsFirstSibling();
		}
	}

}
