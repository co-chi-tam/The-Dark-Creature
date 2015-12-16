using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu ("UI/Base ListView")]
public class CBaseList : MonoBehaviour, IEventScrollRect {
	
	public GameObject Cell;
	public RectTransform PullToRefeshRange;

	[SerializeField]
	[Range(0f, 50f)]
	private float m_PullToPercent = 20f;

	private float m_PullToDistance = 100f;
	public bool LoadLate = true;
	protected List<CBaseCell> m_ListViewItems = new List<CBaseCell>(9999);
	protected CUIScrollRectCustom m_ScrollRect;
	protected int itemLoaded = 0;
	public int stepLoad = 5;
	public float endOffset = 0.0f;

	public System.Action EventOnStartScroll;
	public System.Action EventOnScroll;
	public System.Action EventOnEndScroll;

	public System.Action PullToRefesh;
	public System.Action PullToPrevius;

	void OnEnable() {
		FitContent();
	}
	
	void OnDisable() {
		FitContent();
	}

	void Start() {
		m_PullToDistance = Camera.main.pixelHeight / 100f * m_PullToPercent; 
	}
	
	public virtual void Init() {
		m_ScrollRect = this.GetComponent<CUIScrollRectCustom>();
		ReloadData();
	}
	
	protected virtual int CountItem() {
		return 10;
	}
	
	protected virtual CBaseCell CellForIndex(int index) {
		var go 		= Instantiate (Cell);
		go.name 	= string.Format ("Item {0}", index);
		var cell 	= go.GetComponent<CBaseCell>();
		cell.SetupCell(m_ScrollRect.content);
		return cell;
	} 
	
	protected virtual void DidSelectedCell (int index, CBaseCell cell = null) {
		
	}

	public virtual void ReloadDone() {

	}

	public virtual void ReloadData(bool clearList = false) {
		m_ScrollRect = this.GetComponent<CUIScrollRectCustom>();
		Cell.SetActive (false);
		ClearList();
		m_ListViewItems.TrimExcess();
		m_ListViewItems.Clear();
		for (int i = 0; i < this.CountItem(); i++) {
			if (i >= m_ScrollRect.content.childCount) {
				var cell = CellForIndex (i);
				AddEventClick (cell.gameObject, i);
				m_ListViewItems.Add(cell);
			} else {
				var child = m_ScrollRect.content.GetChild (i).gameObject;
				var cell = child.AddComponent<CBaseCell>();
				cell.SetupCell(m_ScrollRect.content);
				AddEventClick (child, i);
				m_ListViewItems.Add (cell);
			}
			//if (LoadLate && m_ListViewItems[i].Active)
			//	m_ListViewItems[i].Active = (i < stepLoad);
			m_ListViewItems[i].ActiveCell();
		}
		itemLoaded = stepLoad;
	}
	
	protected void AddEventClick(GameObject cell, int index) {
		var eventUI = cell.GetComponent<CUIEventManager>();
		var baseCell = cell.GetComponent<CBaseCell>();
		if (eventUI != null) {
			if (eventUI.EventOnPointerClick == null) {
				eventUI.EventOnPointerClick += (x, y) => {
					DidSelectedCell (index, baseCell);
				};
			}
		}
	}
	
	public void ClearList() {
		for (int i = 0; i < m_ScrollRect.content.childCount; i++) {
			m_ScrollRect.content.GetChild(i).gameObject.SetActive (false);
		}
	}
	
	public void DestroyList() {
		for (int i = 0; i < m_ScrollRect.content.childCount; i++) {
			Destroy (m_ScrollRect.content.GetChild(i).gameObject);
		}
	}
	
	public virtual void OnBeginDrag() {
		if (EventOnStartScroll != null) {
			EventOnStartScroll();
		}
	}
	
	protected bool loadingListItem = false;
	public virtual void OnDrag(float value) {
		if (EventOnScroll != null) {
			EventOnScroll();
		}
	}
	
	public virtual void OnScroll(float value) {
		if (EventOnScroll != null) {
			EventOnScroll();
		}
	}
	
	public virtual void OnEndDrag() {
		if (EventOnEndScroll != null && (this.CountItem() % stepLoad) == 0 && this.m_ScrollRect.normalizedPosition.y < endOffset) {
			EventOnEndScroll();
		}

		if (PullToRefeshRange != null) {
			var CurrentDistance = PullToRefeshRange.localPosition.y - m_ScrollRect.content.localPosition.y;
			if ((CurrentDistance) > m_PullToDistance) {
				if (PullToRefesh != null) {
				    PullToRefesh();
				}
			}else if(CurrentDistance > 1.0f && PullToPrevius != null){			
					PullToPrevius();
			}
		}
	}
	
	protected virtual IEnumerator LoadNext() {
		yield return null;
	}

	public void MoveToFirst() {
		m_ScrollRect = this.GetComponent<CUIScrollRectCustom>();
		m_ScrollRect.MoveToFirst();
	}

	public void MoveToEnd() {
		m_ScrollRect = this.GetComponent<CUIScrollRectCustom>();
		m_ScrollRect.MoveToEnd();
	}

	public void FitContent() {
		m_ScrollRect = this.GetComponent<CUIScrollRectCustom>();
		m_ScrollRect.FitContent();
	}
}

























