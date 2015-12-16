using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
[AddComponentMenu("UI/ScrollRect Custom")]
public class CUIScrollRectCustom : ScrollRect, IUserInterface {
	
	#region Properties
	protected RectTransform m_RectTransform;
	
	[SerializeField]
	[Range(0f, 100f)]
	protected float m_PercentX = 0f;
	public float PercentX {
		get { return m_PercentX; }
		set { m_PercentX = value; }
	}
	[SerializeField]
	[Range(0f, 100f)]
	protected float m_PercentY = 0f;
	public float PercentY {
		get { return m_PercentY; }
		set { m_PercentY = value; }
	}
	[SerializeField]
	[Range(0f, 100f)]
	protected float m_PercentWidth = 100f;
	public float PercentWidth {
		get { return m_PercentWidth; }
		set { m_PercentWidth = value; }
	}

	[SerializeField]
	[Range(0f, 100f)]
	protected float m_PercentHeight = 100f;
	public float PercentHeight {
		get { return m_PercentHeight; }
		set { m_PercentHeight = value; }
	}

	[SerializeField]
	protected bool m_ActiveScroll = true;
	public bool ActiveScroll {
		get { return m_ActiveScroll; }
		set { m_ActiveScroll = value; }
	}
	
	protected CUIGridViewCustom m_GridContent;
	protected IEventScrollRect m_EventScrollRect;

	public Action<int> OnEventStartScrollDrag;
	public Action<int> OnEventScrollDrag;
	public Action<int> OnEventEndScrollDrag;
	
	public Action<float> OnEventEndScrollValue;

	public Action<PointerEventData> OnEventBeginDrag;
	public Action<PointerEventData> OnEventDrag;
	public Action<PointerEventData> OnEventEndDrag;
	public Action<PointerEventData> OnEventInitializePotentialDrag;

	#endregion
	
	#region Implementation  

	protected override void OnEnable ()
	{
		base.OnEnable ();
		MoveToFirst();
	}

	protected override void Start() {
		base.Start();
		this.m_GridContent			= this.content.GetComponent<CUIGridViewCustom>();
		m_GridContent.ParentRecttransform	= this.GetComponent<RectTransform>();
		this.m_EventScrollRect			= this.transform.GetComponent<IEventScrollRect>();
//		var itemPerPage				= this.m_GridContent != null ? (float) this.m_GridContent.ItemPerPage : 1f;
		FitContent();
		CalculateContent();
		MoveToFirst();
	}

	public override void OnInitializePotentialDrag (PointerEventData eventData)
	{
		base.OnInitializePotentialDrag (eventData);
		if (OnEventInitializePotentialDrag != null) {
			OnEventInitializePotentialDrag (eventData);
		}
	}

	public override void OnBeginDrag (PointerEventData eventData)
	{
		if (m_ActiveScroll == false) return;
		base.OnBeginDrag(eventData);
		if (m_EventScrollRect != null) {
			m_EventScrollRect.OnBeginDrag();
		}
		if (OnEventStartScrollDrag != null) {
			var scrollValue = ScrollCurrentValue();
			var page 	= FindNearest(scrollValue);
			OnEventStartScrollDrag (page);
		}
		if (OnEventBeginDrag != null) {
			OnEventBeginDrag (eventData);
		}
	}
	
	public override void OnDrag (PointerEventData eventData)
	{
		if (m_ActiveScroll == false) return;
		base.OnDrag (eventData);
		if (m_EventScrollRect != null) {
			m_EventScrollRect.OnDrag(ScrollCurrentValue());
		}
		if (OnEventScrollDrag != null) {
			var scrollValue = ScrollCurrentValue();
			var page 	= FindNearest(scrollValue);
			OnEventScrollDrag (page);
		}
		if (OnEventDrag != null) {
			OnEventDrag (eventData);
		}
	}
	
	public override void OnScroll (PointerEventData eventData)
	{
		if (m_ActiveScroll == false) return;
		base.OnScroll (eventData);
		if (m_EventScrollRect != null) {
			m_EventScrollRect.OnScroll(ScrollCurrentValue());
		}
		if (OnEventScrollDrag != null) {
			var scrollValue = ScrollCurrentValue();
			var page 	= FindNearest(scrollValue);
			OnEventScrollDrag (page);
		}
		if (OnEventDrag != null) {
			OnEventDrag (eventData);
		}
	}
	
	public override void OnEndDrag (PointerEventData eventData)
	{
		if (m_ActiveScroll == false) return;
		base.OnEndDrag (eventData);
		if (m_EventScrollRect != null) {
			m_EventScrollRect.OnEndDrag();
		}
		if (OnEventEndScrollDrag != null) {
			var scrollValue = ScrollCurrentValue();
			var page 	= FindNearest(scrollValue);
			OnEventEndScrollDrag (page);
		}
		if (OnEventEndScrollValue != null) {
			OnEventEndScrollValue (ScrollCurrentValue());
		}
		if (OnEventEndDrag != null) {
			OnEventEndDrag (eventData);
		}
	}

	#endregion
	
	#region Main Method

	public virtual void MoveToFirst() {
		this.horizontalNormalizedPosition = 0;
		this.verticalNormalizedPosition = 1;
	}

	public virtual void MoveToEnd() {
		this.horizontalNormalizedPosition = 1;
		this.verticalNormalizedPosition = 0;
	}
	
	public virtual void MoveToLast() {
		this.horizontalNormalizedPosition = 1;
		this.verticalNormalizedPosition = 0;
	}
	
	public virtual void FitContent () {
		m_GridContent 					= this.content.GetComponent<CUIGridViewCustom>();
		this.m_RectTransform			= this.GetComponent <RectTransform>();
		var offsetX						= m_PercentX / 100f;
		var offsetY						= 1f - m_PercentY / 100f;
		var perWidth 					= Mathf.Min (1f, offsetX + m_PercentWidth / 100f);
		var perHeight 					= Mathf.Max (0f, offsetY - m_PercentHeight / 100f);
		this.m_RectTransform.anchorMin 			= new Vector2 (offsetX, perHeight);
		this.m_RectTransform.anchorMax 			= new Vector2 (perWidth, offsetY);
		this.m_RectTransform.sizeDelta			= Vector2.zero;
		this.m_RectTransform.localScale			= Vector3.one;
		this.m_RectTransform.anchoredPosition   = Vector3.zero;
		m_GridContent.ParentRecttransform 		= this.m_RectTransform;
		m_GridContent.FitContent();
	}
	
	public void CalculateContent() {
		this.m_RectTransform = this.GetComponent<RectTransform>();
		var vectorAnchorMin = this.m_RectTransform.anchorMin;
		var vectorAnchorMax = this.m_RectTransform.anchorMax;
		m_PercentX			= vectorAnchorMin.x * 100f;
		m_PercentY			= (1f - vectorAnchorMax.y) * 100f;
		m_PercentWidth		= vectorAnchorMax.x * 100f - m_PercentX;
		m_PercentHeight		= 100f - (vectorAnchorMin.y * 100f + m_PercentY);
		this.m_RectTransform.sizeDelta			= Vector2.zero;
		this.m_RectTransform.localScale			= Vector3.one;
		this.m_RectTransform.anchoredPosition   = Vector3.zero;
	}

	protected virtual int FindNearest(float scrollValue)
	{
		return 0;
	}

	protected virtual float ScrollCurrentValue() {
		if (this.horizontal) {
			return this.horizontalNormalizedPosition;
		} else if (this.vertical) {
			return this.verticalNormalizedPosition;
		}
		return ScrollVectorCurrentValue().magnitude;
	}
			
	private Vector2 ScrollVectorCurrentValue() {
		return this.normalizedPosition;
	}

	public float GetPercentX() {
		return m_PercentX;
	}
	public float GetPercentY() {
		return m_PercentY;
	}
	public float GetPercentWidth() {
		return m_PercentWidth;
	}
	public float GetPercentHeight() {
		return m_PercentHeight;
	}

	#endregion
	
}