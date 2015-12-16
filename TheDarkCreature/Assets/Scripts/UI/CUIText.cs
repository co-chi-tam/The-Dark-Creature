using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("UI/UIText")]
public class CUIText : Text, IUserInterface {
	[SerializeField]
	private bool ChangeFontSize = true;

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

	[SerializeField]
	private float m_FitScreenWidth = 640;
	public float FitScreenWidth {
		get { return m_FitScreenWidth; }
		set { m_FitScreenWidth = value; }
	}

	[SerializeField]
	private float m_FitScreenHeight = 960;
	public float FitScreenHeight {
		get { return m_FitScreenHeight; }
		set { m_FitScreenHeight = value; }
	}

	[SerializeField]
	private int m_CurrentFontSize = 11; 
	public int CurrentFontSize {
		get { return m_CurrentFontSize; }
		set { m_CurrentFontSize = value; }
	}

	[Space (10f)]
	[SerializeField]
	private string m_EndString = "..."; 
	public string EndString {
		get { return m_EndString; }
		set { m_EndString = value; }
	}
	[SerializeField]
	[Tooltip("Detect if overflow then add end string to end line")]
	private bool m_DetectTextOverFlow_X = false; 
	public bool DetectTextOverFlow {
		get { return m_DetectTextOverFlow_X; }
		set { m_DetectTextOverFlow_X = value; }
	}

	[SerializeField]
	private float m_OffsetOverFlow = 10f; 

	[SerializeField]
	private int m_LimitLine = 1;

	private RectTransform m_RectTransform;
	private float m_PrevertWidth = 0f;

	protected override void Awake ()
	{
		base.Awake ();
		FitContent();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	public void SetText(string value) {
//		this.m_RectTransform = this.GetComponent<RectTransform>();
		if (m_DetectTextOverFlow_X == false) {
			this.text = value; 
		} else {
			this.text  			= m_EndString;
			var endStringWidth 	= this.preferredWidth;
			var	rectTextSize 	= (this.rectTransform.rect.width == 0f ? m_PrevertWidth : this.rectTransform.rect.width) * m_LimitLine - endStringWidth - m_OffsetOverFlow;
			var temp 			= value;
			this.text  			= value;
			var textWidth 		= this.preferredWidth;
			var isAddDot 		= false;
			for (int i = temp.Length; i >= 0; i--) {
				if (textWidth > rectTextSize && temp.Length > 0) { 
					temp 		= temp.Substring(0, temp.Length - 1);
					this.text 	= temp;
					textWidth 	= this.preferredWidth;
					isAddDot 	= true;
				} else {
					break;
				}
			}
			this.text = isAddDot ? temp + (m_EndString) : temp;
		}
	}

	public void FitContent () {
		this.m_RectTransform = this.GetComponent<RectTransform>();
		m_PrevertWidth  = m_PrevertWidth == 0f ? this.m_RectTransform.rect.width : m_PrevertWidth;
		var offsetX		= m_PercentX / 100f;
		var offsetY		= 1f - m_PercentY / 100f;
		var perWidth 	= Mathf.Min (1f, offsetX + m_PercentWidth / 100f);
		var perHeight 	= Mathf.Max (0f, offsetY - m_PercentHeight / 100f);
		this.m_RectTransform.anchorMin 			= new Vector2 (offsetX, perHeight);
		this.m_RectTransform.anchorMax 			= new Vector2 (perWidth, offsetY);
		this.m_RectTransform.sizeDelta			= Vector2.zero;
		this.m_RectTransform.localScale			= Vector3.one;
		this.m_RectTransform.anchoredPosition   = Vector3.zero;

		if (this.font == null) {
			this.font = Resources.Load <Font>("Fonts/Arial");
		}

		if (ChangeFontSize) {
			this.fontSize = (int)(Camera.main.pixelWidth * CurrentFontSize / m_FitScreenWidth);
		}
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
		
		if (this.font == null) {
			this.font = Resources.Load <Font>("Fonts/Arial");
		}
		m_FitScreenWidth = Camera.main.pixelWidth;
		m_FitScreenHeight = Camera.main.pixelHeight;
		if (ChangeFontSize) {
			this.fontSize = (int)(Camera.main.pixelWidth * CurrentFontSize / m_FitScreenWidth);
		}
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
