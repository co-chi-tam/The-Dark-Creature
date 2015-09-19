using UnityEngine;
using System.Collections;

public class TDCRenderController : MonoBehaviour {

	private Animator m_Animator;
	private Collider m_Collider;

	void Start () {
		m_Animator = this.transform.parent.GetComponent<Animator> ();
		m_Collider = this.transform.parent.GetComponent<Collider> ();
	}

	void OnBecameInvisible() {
		if (m_Animator != null) {
			m_Animator.enabled = false;
		}
		if (m_Collider != null) {
			m_Collider.enabled = false;
		}
	}

	void OnBecameVisible() {
		if (m_Animator != null) {
			m_Animator.enabled = true;
		}
		if (m_Collider != null) {
			m_Collider.enabled = true;
		}
	}

}
