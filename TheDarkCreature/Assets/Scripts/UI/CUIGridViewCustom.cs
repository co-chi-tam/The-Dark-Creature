using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("UI/GridView Custom")]
[RequireComponent (typeof(ContentSizeFitter))]
public class CUIGridViewCustom : GridLayoutGroup, IUserInterface {

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
	protected int m_ItemPerPage = 1;
	public int ItemPerPage {
		get { return m_ItemPerPage; }
	}

	[SerializeField]
	protected RectTransform m_ParentRecttransform;
	public RectTransform ParentRecttransform {
		get { return m_ParentRecttransform; }
		set { m_ParentRecttransform = value; }
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

	public virtual void FitContent() {
		m_ParentRecttransform	= this.transform.parent.GetComponent<RectTransform>();
		var width 				= m_ParentRecttransform.rect.width 	/ 100f * m_PercentWidth;
		var height				= m_ParentRecttransform.rect.height / 100f * m_PercentHeight;

		this.m_CellSize.x 		= width / (m_ItemPerPage == 0f ? 1f : m_ItemPerPage);
		this.m_CellSize.y 		= height;

		this.rectTransform.localScale	 	= Vector3.one;
		this.rectTransform.anchorMin		= Vector2.zero;
		this.rectTransform.anchorMax		= Vector2.one;
		this.rectTransform.sizeDelta 		= Vector2.zero;
		this.rectTransform.anchoredPosition = Vector2.zero;
		
		this.rectTransform.pivot 			= new Vector2 (0f, 1f);
	}
	
	public virtual void CalculateContent() {
		
	}
	
	public virtual void MoveToFirst() {
		 
	}
	
	public float GetPercentX() {
		return 0f;
	}
	public float GetPercentY() {
		return 0f;
	}
	public float GetPercentWidth() {
		return m_PercentWidth;
	}
	public float GetPercentHeight() {
		return m_PercentHeight;
	}
}
