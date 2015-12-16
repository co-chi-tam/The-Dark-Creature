using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum ETransition : byte {
	None 				= 0,
	LeftToRight 		= 1,
	RightToLeft 		= 2,
	TopToBottom 		= 3,
	BottomToTop			= 4,
	Back				= 5,
	Popup 				= 6,
	FadeInFadeOut		= 7,
	LeftToRightThrought	= 8,
	RightToLeftThrought = 9,
	TopToBottomThrought = 10,
	BottomToTopThrought = 11,
}

[ExecuteInEditMode]
[AddComponentMenu("UI/Page Transition")]
public class CUIPageTransition : MonoBehaviour, IUserInterface {
	
	#region Properties
	[SerializeField]
	private float m_TimeFade = 0.1f;
	[SerializeField]
	private Camera m_CameraControl;
	public Camera CameraControl {
		get { return m_CameraControl; } 
		set { m_CameraControl = value; }
	}
	[SerializeField]
	private ETransition m_TransitionType = ETransition.None;
	public ETransition TransactionType {
		get { return m_TransitionType; }
		set { m_TransitionType = value; }
	}

	[SerializeField]
	private float m_SpeedTransition = 150f;
	
	private CUIPageTransition m_ActivePageTransition;
	public CUIPageTransition ActivePageTransition {
		get { return m_ActivePageTransition; }
	}
	private RectTransform m_RectTransform;
	private float m_ScreenWidth 		= 0f;
	private float m_ScreenHeight 		= 0f;
	private bool m_IsScroll				= false;
	private IPageTransition m_IPageTransaction;
	private CanvasGroup m_CanvasGroup;
	private float m_ZOffset = 0f;
	
	#endregion
	
	#region Implementation MonoBehavior

	public virtual void Awake () {
		this.m_RectTransform	= this.transform as RectTransform;
		this.m_IPageTransaction	= this.GetComponent<IPageTransition>();
	}
	
	public virtual void Start () {
		Init();
	}
	
	#endregion
	
	#region Main methods

	protected void GetPageActive (CUIPageTransition value) {
//		Debug.LogError (value.name);
		m_ActivePageTransition = value;
	}

	public void Init() {
		this.m_CameraControl	= Camera.main;
		this.m_ScreenWidth 		= this.m_CameraControl.pixelWidth;
		this.m_ScreenHeight 	= this.m_CameraControl.pixelHeight;
		this.FitContent();	
	}

	public virtual void BeginTransition(ETransition transition) {
		if (m_IsScroll == false) {
			if (transition == ETransition.Back) {
				MoveBack();
			} else {
				this.m_TransitionType = transition;
				MoveCenter();
			}
		}
	}

	public virtual void MoveCenter() {
		var _x = 0f;
		var _y = 0f;
		if (m_IsScroll == false) {
			this.FitTransaction();
			switch (m_TransitionType) {
			case ETransition.FadeInFadeOut:
				StartCoroutine (FadeInFadeOut (1f));
				break;
			case ETransition.Popup:
				StartCoroutine (Popup (1f));
				break;
			default:
				StartCoroutine (this.MoveToPosition(_x, _y));
			break;
			}
		}
	}

	public virtual void MoveBack() {
		var posX = 0f;
		var posY = 0f;
		switch (m_TransitionType) {
		case ETransition.LeftToRight: 
			posX = -this.m_ScreenWidth;
			break;
		case ETransition.RightToLeft:
			posX = this.m_ScreenWidth;
			break;
		case ETransition.TopToBottom:
			posY = this.m_ScreenHeight;
			break;
		case ETransition.BottomToTop:
			posY = -this.m_ScreenHeight;
			break;
		case ETransition.LeftToRightThrought:
			posX = this.m_ScreenWidth;
			break;
		case ETransition.RightToLeftThrought:
			posX = -this.m_ScreenWidth;
			break;
		case ETransition.TopToBottomThrought:
			posY = -this.m_ScreenHeight;
			break;
		case ETransition.BottomToTopThrought:
			posY = this.m_ScreenHeight;
			break;
		case ETransition.FadeInFadeOut:
			this.m_TransitionType = ETransition.Back;
			StartCoroutine (FadeInFadeOut (0f));
			return;
		case ETransition.Popup:
			this.m_TransitionType = ETransition.Back;
			StartCoroutine (Popup (0f));
			return;
		default:
			break;
		}
		if (m_IsScroll == false) {
			StartCoroutine (this.MoveToPosition(posX, posY));
		}
	}

	private IEnumerator FadeInFadeOut(float fade) {
		var speed = m_TimeFade;
		m_IsScroll = true;
		var interfaceValid = this.m_IPageTransaction != null;
		if (interfaceValid && this.m_TransitionType != ETransition.Back) { this.m_IPageTransaction.OnStartTransition (); }
		var waiter = new WaitForEndOfFrame();
		m_CanvasGroup = this.GetComponent<CanvasGroup>();
		if (m_CanvasGroup == null) {
			m_CanvasGroup = this.gameObject.AddComponent<CanvasGroup>(); 
		}
		var offsetFade = fade == 1f ? 0 : 1f;
		m_CanvasGroup.alpha = offsetFade;
		speed = fade == 1f ? speed : -speed;
		while(m_CanvasGroup.alpha != fade) {
			m_CanvasGroup.alpha = Mathf.Clamp (m_CanvasGroup.alpha + speed, 0f, 1f);
			yield return waiter;
			if (interfaceValid) { this.m_IPageTransaction.OnTransition(); }
		}
		m_CanvasGroup.alpha = fade;
		yield return waiter;
		if (interfaceValid) { this.m_IPageTransaction.OnEndTransition(); }
		m_IsScroll = false;
		if (this.m_TransitionType == ETransition.Back) {
			this.m_TransitionType = ETransition.RightToLeft;
			FitTransaction();
		}
	}

	private IEnumerator Popup(float scale, float speed = 0.25f) {
		m_IsScroll = true;
		var interfaceValid = this.m_IPageTransaction != null;
		if (interfaceValid && this.m_TransitionType != ETransition.Back) { this.m_IPageTransaction.OnStartTransition (); }
		var waiter = new WaitForEndOfFrame();
		m_CanvasGroup = this.GetComponent<CanvasGroup>();
		if (m_CanvasGroup == null) {
			m_CanvasGroup = this.gameObject.AddComponent<CanvasGroup>(); 
		}
		var offsetScale = scale == 1f ? 0f : 1f;
		m_CanvasGroup.alpha = offsetScale;
		this.transform.localScale = new Vector3 (offsetScale, offsetScale, 1f);
		speed = scale == 1f ? speed : -speed;
		var vector3Scale = Vector3.zero;
		while(m_CanvasGroup.alpha != scale) {
			m_CanvasGroup.alpha = Mathf.Clamp (m_CanvasGroup.alpha + speed, 0f, 1f);
			vector3Scale.x = Mathf.Clamp (this.transform.localScale.x + speed, 0f, 1f);
			vector3Scale.y = Mathf.Clamp (this.transform.localScale.y + speed, 0f, 1f);
			vector3Scale.z = 1f;
			this.transform.localScale = vector3Scale;
			yield return waiter;
			if (interfaceValid) { this.m_IPageTransaction.OnTransition(); }
		}
		m_CanvasGroup.alpha = scale;
		yield return waiter;
		this.transform.localScale = new Vector3 (scale, scale, 1f);
		if (interfaceValid) { this.m_IPageTransaction.OnEndTransition(); }
		m_IsScroll = false;
		if (this.m_TransitionType == ETransition.Back) {
			this.m_TransitionType = ETransition.RightToLeft;
			FitTransaction();
		}
	}

	private IEnumerator MoveToPosition(float moveToPosX, float moveToPosY) {
		var speed = m_SpeedTransition;
		m_IsScroll = true;
		var interfaceValid = this.m_IPageTransaction != null;
		if (interfaceValid && this.m_TransitionType != ETransition.Back) { this.m_IPageTransaction.OnStartTransition (); }
		var waiter = new WaitForEndOfFrame();
		var stepWidth 	= 1f;
		var stepHeight 	= 1f;
		var stepX 		= this.m_RectTransform.localPosition.x > moveToPosX ? -1 : 1;
		var stepY 		= this.m_RectTransform.localPosition.y > moveToPosY ? -1 : 1;
		var movePos 	= this.m_RectTransform.localPosition;
		movePos.z 		= m_ZOffset;
		var countStep	= 0;
		while(this.m_RectTransform.localPosition.x != moveToPosX || this.m_RectTransform.localPosition.y != moveToPosY) {
			if (this.m_RectTransform.localPosition.x != moveToPosX) {
				movePos.x += stepWidth * stepX;
			}
			if (this.m_RectTransform.localPosition.y != moveToPosY) {
				movePos.y += stepHeight * stepY;
			}
			countStep++;
			this.m_RectTransform.localPosition = this.m_RectTransform.anchoredPosition = movePos;
			if (countStep % speed == 0) {
				countStep = 0;
//				speed -= 2f;
				if (interfaceValid && this.m_TransitionType != ETransition.Back) { this.m_IPageTransaction.OnTransition(); }
				yield return waiter;
			}
		}
		movePos.x = moveToPosX;
		movePos.y = moveToPosY;
		this.m_RectTransform.anchoredPosition = this.m_RectTransform.localPosition = movePos;
		yield return waiter;
		if (interfaceValid && this.m_TransitionType != ETransition.Back) { this.m_IPageTransaction.OnEndTransition(); }
		m_IsScroll = false;
	}

	public void FitTransaction() {
		Init();
		var posX = 0f;
		var posY = 0f;
		switch (m_TransitionType) {
		case ETransition.LeftToRight: 
		case ETransition.LeftToRightThrought:
			posX = -this.m_ScreenWidth;
			break;
		case ETransition.RightToLeft:
		case ETransition.RightToLeftThrought:
			posX = this.m_ScreenWidth;
			break;
		case ETransition.TopToBottom:
		case ETransition.TopToBottomThrought:
			posY = this.m_ScreenHeight;
			break;
		case ETransition.BottomToTop:
		case ETransition.BottomToTopThrought:
			posY = -this.m_ScreenHeight;
			break;
		case ETransition.FadeInFadeOut:
		case ETransition.None:
			posX = 0f;
			posY = 0f;
			break;
		}
		this.m_RectTransform.anchoredPosition = this.m_RectTransform.localPosition = new Vector3 (posX, posY, 0f);
	}

	public virtual void FitContent() {
		this.m_RectTransform			= this.transform as RectTransform;
		this.m_RectTransform.anchorMin = Vector2.zero;
		this.m_RectTransform.localScale = Vector3.one;
		this.m_RectTransform.anchorMax 	= Vector2.one;
		this.m_RectTransform.sizeDelta 	= Vector2.one;
	}
	
	public void CalculateContent() {
		
	}
	
	public void MoveToFirst() {
		
	}
	
	public float GetPercentX() {
		return this.m_RectTransform.rect.x;
	}
	public float GetPercentY() {
		return this.m_RectTransform.rect.y;
	}
	public float GetPercentWidth() {
		return this.m_RectTransform.rect.width;
	}
	public float GetPercentHeight() {
		return this.m_RectTransform.rect.height;
	}
	
	#endregion
}
