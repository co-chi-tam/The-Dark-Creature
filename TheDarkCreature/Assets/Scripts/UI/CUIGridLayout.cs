using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(ContentSizeFitter))]
[AddComponentMenu("UI/Grid Layout Fit Parent")]
public class CUIGridLayout : GridLayoutGroup {

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
	[Space(10f)]
	[SerializeField]
	[Range(0f, 100f)]
	private float m_PercentWidthCell = 100f;
	public float PercentWidthCell {
		get { return m_PercentWidthCell; }
		set { m_PercentWidthCell = value; }
	}
	[SerializeField]
	[Range(0f, 100f)]
	private float m_PercentHeightCell = 100f;
	public float PercentHeightCell {
		get { return m_PercentHeightCell; }
		set { m_PercentHeightCell = value; }
	}

	protected override void Start ()
	{
		base.Start ();
		FitContent ();
		FitCellContent ();
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
		var offsetX		= m_PercentX / 100f;
		var offsetY		= 1f - m_PercentY / 100f;
		var perWidth 	= Mathf.Min (1f, offsetX + m_PercentWidth / 100f);
		var perHeight 	= Mathf.Max (0f, offsetY - m_PercentHeight / 100f);
		this.rectTransform.anchorMin		= new Vector2 (offsetX, perHeight);
		this.rectTransform.anchorMax		= new Vector2 (perWidth, offsetY);
		this.rectTransform.sizeDelta		= new Vector2 (0f, 0f);
		this.rectTransform.localScale		= new Vector3 (1f, 1f, 1f);
		this.rectTransform.anchoredPosition	= new Vector3 (0f, 0f, 0f);
	}

	public void FitCellContent() {
		var width 				= this.rectTransform.rect.width;
		var height				= this.rectTransform.rect.height;
		
		this.m_CellSize.x 		= width / 100f * m_PercentWidthCell;
		this.m_CellSize.y 		= height / 100f * m_PercentHeightCell;
	}

}
