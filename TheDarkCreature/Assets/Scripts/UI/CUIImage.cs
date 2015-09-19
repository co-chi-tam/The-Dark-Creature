using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Image Custom")]
public class CUIImage : Image {

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

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
	}

	public void CalculateContent() {
		var _x = this.rectTransform.anchorMin.x * 100f;
		var _y = 100f - this.rectTransform.anchorMax.y * 100f;
		var _width = this.rectTransform.anchorMax.x * 100f - _x;
		var _height = 100f - (this.rectTransform.anchorMin.y * 100f + _y);
		m_PercentX = _x;
		m_PercentY = _y;
		m_PercentWidth = _width;
		m_PercentHeight = _height;
		this.rectTransform.sizeDelta		= new Vector2 (0f, 0f);
		this.rectTransform.localScale		= new Vector3 (1f, 1f, 1f);
		this.rectTransform.anchoredPosition	= new Vector3 (0f, 0f, 0f);
	}

	public void FitContent() {
		var offsetX		= m_PercentX / 100;
		var offsetY		= 1f - m_PercentY / 100;
		var perWidth 	= Mathf.Min (1f, offsetX + m_PercentWidth / 100);
		var perHeight 	= Mathf.Max (0f, offsetY - m_PercentHeight / 100);
		this.rectTransform.anchorMin 			= new Vector2 (offsetX, perHeight);
		this.rectTransform.anchorMax 			= new Vector2 (perWidth, offsetY);
		this.rectTransform.sizeDelta			= new Vector2 (0f, 0f);
		this.rectTransform.localScale			= new Vector3 (1f, 1f, 1f);
		this.rectTransform.anchoredPosition   = new Vector3 (0f, 0f, 0f);
	}
}
