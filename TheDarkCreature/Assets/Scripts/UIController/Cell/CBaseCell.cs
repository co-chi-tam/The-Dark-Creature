using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (BillboardRenderer))]
public class CBaseCell : MonoBehaviour {

	public bool m_Active = true;

	public void SetupCell (Transform parent)
	{
		this.transform.SetParent (parent);
		this.transform.localScale 	 = Vector3.one;
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, 0f);
	}

	public void ActiveCell() {
		this.gameObject.SetActive (m_Active);
	}

	public virtual void OnOutOfScreen(bool active) {

	}

	private void OnBecameVisible() {
		OnOutOfScreen (true);
	}

	private void OnBecameInvisible() {
		OnOutOfScreen (false);
	}

}
