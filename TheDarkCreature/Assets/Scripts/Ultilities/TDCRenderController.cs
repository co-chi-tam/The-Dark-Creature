using UnityEngine;
using System.Collections;

public class TDCRenderController : MonoBehaviour {

	private TDCBaseController m_BaseController;

	void Start () {
		LoadController();
	}

	void OnBecameInvisible() {
		LoadController();
		if (m_BaseController != null)
		{
			m_BaseController.OnBecameInvisible();
		}
	}

	void OnBecameVisible() {
		LoadController();
		if (m_BaseController != null)
		{
			m_BaseController.OnBecameVisible();
		}
	}

	void LoadController() {
		if (m_BaseController == null)
		{
			m_BaseController = this.transform.parent.GetComponent<TDCBaseController>();
		}
	}

}
