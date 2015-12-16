using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[AddComponentMenu("UI/Raw Image Custom")]
public class CUIRawImage : RawImage, IUserInterface {
	
	#region Properties
	private RectTransform m_RectTransform;
	
	[SerializeField]
	[Range(0f, 100f)]
	private float m_PercentX = 0f;
	public float PercentX {
		get { return m_PercentX; }
		set { m_PercentX = value; }
	}
	[SerializeField]
	[Range(0f, 100f)]
	private float m_PercentY = 0f;
	public float PercentY {
		get { return m_PercentY; }
		set { m_PercentY = value; }
	}
	[SerializeField]
	[Range(0f, 100f)]
	private float m_PercentWidth = 100f;
	public float PercentWidth {
		get { return m_PercentWidth; }
		set { m_PercentWidth = value; }
	}
	[SerializeField]
	[Range(0f, 100f)]
	private float m_PercentHeight = 100f;
	public float PercentHeight {
		get { return m_PercentHeight; }
		set { m_PercentHeight = value; }
	}
	
	#endregion
	
	#region Main method
	
	public virtual void Init (string directory = "images") {
		m_RectTransform = this.GetComponent<RectTransform>();
		this.FitContent();
//		this.color 		= Color.white;
		CalculateContent();
	}
	
	public virtual void FitContent() {
		m_RectTransform = this.GetComponent<RectTransform>();
		var offsetX		= m_PercentX / 100;
		var offsetY		= 1f - m_PercentY / 100;
		var perWidth 	= Mathf.Min (1f, offsetX + m_PercentWidth / 100);
		var perHeight 	= Mathf.Max (0f, offsetY - m_PercentHeight / 100);
		this.m_RectTransform.anchorMin 			= new Vector2 (offsetX, perHeight);
		this.m_RectTransform.anchorMax 			= new Vector2 (perWidth, offsetY);
		this.m_RectTransform.sizeDelta			= Vector2.zero;
		this.m_RectTransform.localScale			= Vector3.one;
		this.m_RectTransform.anchoredPosition   = Vector3.zero;
	}
	
	public virtual void CalculateContent() {
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
	
	public virtual void MoveToFirst() {
		
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
