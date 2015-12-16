using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum EGridMove : byte {
	Left 	= 0,
	Right 	= 1,
	Up 		= 3,
	Down 	= 4
}

[AddComponentMenu("UI/GridView Transition")]
[RequireComponent (typeof(ContentSizeFitter))]
public class CUIGridViewTransition : GridLayoutGroup, IUserInterface  {

	#region Properties
	private RectTransform m_Content;
	[SerializeField]
	private GameObject m_ParentContent;
	private float m_ScreenWidth;
	private float m_ScreenHeight;
	
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
	private int m_CurrentPage = 0;
	public int CurrentPages {
		get { return m_CurrentPage; } 
		set { m_CurrentPage = value; }
	}
	[SerializeField]
	private int m_MaxPage;
	public int MaxPage {
		get { return m_MaxPage; }
		set { m_MaxPage = value; }
	}

	[SerializeField]
	private AnimationCurve m_SpeedCurve;

	private bool m_IsScrolling = false;

	public Func<int, bool> OnEventStartScroll;
	public Action OnEventScroll;
	public Action<int> OnEventEndScroll;

	#endregion

	#region Implementation Mono

	protected override void OnEnable ()
	{
		base.OnEnable();
		m_Content = this.GetComponent<RectTransform>();
		FitContent();
		CalculateContent();
	}
	
	protected override void Start ()
	{
		base.Start ();
		m_Content 		= this.GetComponent<RectTransform>();
		FitContent();
		CalculateContent();

		if (m_SpeedCurve.length == 0) {
			m_SpeedCurve.AddKey (0f, 1f);
			m_SpeedCurve.AddKey (1f, 0f);
		}
	}

	#endregion

	#region Main methods

	public void FitContent() {
		var iUserInterface = m_ParentContent.GetComponent<IUserInterface>();
		m_ScreenWidth 	= Camera.main.pixelWidth * 1.001f * iUserInterface.GetPercentWidth() / 100f;
		m_ScreenHeight 	= Camera.main.pixelHeight * 1.001f * iUserInterface.GetPercentHeight() / 100f;
		var offsetX		= m_PercentX / 100;
		var offsetY		= 1f - m_PercentY / 100;
		var perWidth 	= Mathf.Min (1f, offsetX + m_PercentWidth / 100f);
		var perHeight 	= Mathf.Max (0f, offsetY - m_PercentHeight / 100f);

		this.m_CellSize.x 		= m_ScreenWidth;
		this.m_CellSize.y 		= m_ScreenHeight;
		
		this.rectTransform.anchorMin		= new Vector2 (offsetX, perHeight);
		this.rectTransform.anchorMax		= new Vector2 (perWidth, offsetY);
		this.rectTransform.anchoredPosition = Vector2.zero;
		this.rectTransform.localScale	 	= Vector3.one;
		this.rectTransform.sizeDelta 		= Vector2.zero;
		this.rectTransform.pivot 			= new Vector2 (0f, 1f);
	}
	
	public void CalculateContent() {

	}
	
	public void MoveToFirst() {
		FitContent();
	}

	public void MoveLeft() {
		Move (EGridMove.Left, 1);
	}

	public void MoveRight() {
		Move (EGridMove.Right, 1);
	}

	public void MoveUp() {
		Move (EGridMove.Up, 1);
	}
	
	public void MoveDown() {
		Move (EGridMove.Down, 1);
	}

	public void MoveToPage(int index) {
		m_CurrentPage = index < 0 ? 0 : index > m_MaxPage - 1 ? m_MaxPage - 1 : index;
		var length = -1 * m_CurrentPage * m_ScreenWidth;
		this.m_Content.localPosition = this.m_Content.anchoredPosition =  new Vector2 (length , 0f);
	}

	public void ScrollToPage(int index) {
		m_CurrentPage = index < 0 ? 0 : index > m_MaxPage - 1 ? m_MaxPage - 1 : index;
		var length = -1 * m_CurrentPage * m_ScreenWidth;
		var newPos = new Vector2 (length , 0f);
		StartCoroutine (DoMove(newPos));
	}

	public void Move(EGridMove dir, int step) {
		if (m_IsScrolling) {
//			StopCoroutine ("DoMove");
			return;
		}
		var nowPage = m_CurrentPage;
		var _x = 0f;
		var _y = 0f;
		switch (dir) {
		case EGridMove.Left:
		case EGridMove.Right:
			step = dir == EGridMove.Right ? step : -step;
			m_CurrentPage = m_CurrentPage + step > m_MaxPage - 1 ? m_MaxPage - 1 : m_CurrentPage + step < 0 ? 0 : m_CurrentPage + step;
			_x = -1 * m_CurrentPage * m_ScreenWidth;
			break;
		case EGridMove.Up:
		case EGridMove.Down:
			step = dir == EGridMove.Up ? -step : step;
			m_CurrentPage = m_CurrentPage + step > m_MaxPage - 1 ? m_MaxPage - 1 : m_CurrentPage + step < 0 ? 0 : m_CurrentPage + step;
			_y = m_CurrentPage * m_ScreenHeight;
			break;
		}
		if (OnEventStartScroll != null) {
			if (!OnEventStartScroll(m_CurrentPage)) {
				m_CurrentPage = nowPage;
				return;
			}
		}
		var newPos = new Vector2 (_x , _y);
		StartCoroutine (DoMove(newPos));
	}

	private IEnumerator DoMove(Vector2 newPos, float speed = 1f) {
		m_IsScrolling = true;
		var newSpeed = speed < 1f ? 1f - speed : speed;
		while (newSpeed >= 0f) {
			newSpeed -= Time.deltaTime;
			var curveSpeed = m_SpeedCurve.Evaluate (newSpeed);
			this.m_Content.anchoredPosition = Vector3.Lerp (this.m_Content.anchoredPosition, newPos, curveSpeed);
			if (OnEventScroll != null) {
				OnEventScroll();
			}
			if ((this.m_Content.anchoredPosition - newPos).sqrMagnitude <= 0.05f) {
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		this.m_Content.anchoredPosition = newPos;
		if (OnEventEndScroll != null) {
			OnEventEndScroll(m_CurrentPage);
		}
		m_IsScrolling = false;
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
