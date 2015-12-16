using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("UI/GridView Fit Content")]
[RequireComponent (typeof(ContentSizeFitter))]
public class CUIGridViewFitContent : GridLayoutGroup, IUserInterface {
	
	private RectTransform m_Content;

	[Space(10f)]
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
	[Range(0f, 100f)]
	protected float m_CellPercentWidth = 100f;
	public float CellPercentWidth {
		get { return m_CellPercentWidth; }
		set { m_CellPercentWidth = value; }
	}
	[SerializeField]
	[Range(0f, 100f)]
	protected float m_CellPercentHeight = 100f;
	public float CellPercentHeight {
		get { return m_CellPercentHeight; }
		set { m_CellPercentHeight = value; }
	}

	protected override void OnEnable ()
	{
		base.OnEnable();
		FitContent();
		CalculateContent();
	}
	
	protected override void Start ()
	{
		base.Start ();
		FitContent();
		CalculateContent();
	}
	
	public void FitContent() {
		m_Content 		= this.GetComponent<RectTransform>();
		var rectWidth 	= Camera.main.pixelWidth * m_PercentWidth / 100f;
		var rectHeight 	= Camera.main.pixelHeight * m_PercentHeight / 100f;
		var offsetX		= m_PercentX / 100;
		var offsetY		= 1f - m_PercentY / 100;
		var perWidth 	= Mathf.Min (1f, offsetX + m_PercentWidth / 100);
		var perHeight 	= Mathf.Max (0f, offsetY - m_PercentHeight / 100);

		this.m_CellSize.x 		= rectWidth / 100f * m_CellPercentWidth;
		this.m_CellSize.y 		= rectHeight / 100f * m_CellPercentHeight;
		
		this.rectTransform.anchorMin		= new Vector2 (offsetX, perHeight);
		this.rectTransform.anchorMax		= new Vector2 (perWidth, offsetY);
		this.rectTransform.anchoredPosition = Vector2.zero;
		this.rectTransform.localScale	 	= Vector3.one;
		this.rectTransform.sizeDelta 		= Vector2.zero;
		this.rectTransform.pivot 			= new Vector2 (0f, 1f);
	}
	
	public void CalculateContent() {
		m_Content 			= this.GetComponent<RectTransform>();
		var vectorAnchorMin = this.m_Content.anchorMin;
		var vectorAnchorMax = this.m_Content.anchorMax;
		m_PercentX			= vectorAnchorMin.x * 100f;
		m_PercentY			= (1f - vectorAnchorMax.y) * 100f;
		m_PercentWidth		= vectorAnchorMax.x * 100f - m_PercentX;
		m_PercentHeight		= 100f - (vectorAnchorMin.y * 100f + m_PercentY);
		this.m_Content.sizeDelta			= Vector2.zero;
		this.m_Content.localScale			= Vector3.one;
		this.m_Content.anchoredPosition   	= Vector3.zero;
	}
	
	public void MoveToFirst() {
		
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
}
