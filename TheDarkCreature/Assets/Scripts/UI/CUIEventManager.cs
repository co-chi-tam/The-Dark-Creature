using UnityEngine;
using System;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/UI Event Manager")]
public class CUIEventManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
								            IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
	#region Action Event
	public Action<GameObject, Vector3> EventOnPointEnter;
	public Action<GameObject, Vector3> EventOnPointerDown;
	public Action<GameObject, Vector3> EventOnPointerClick;
	public Action<GameObject, Vector3> EventOnPointerUp;
	public Action<GameObject, Vector3> EventOnPointerExit;
//	public Action<GameObject, Vector3> EventOnLongPress;
//	public Action<GameObject, Vector3> EventOnDoublePress;
    #endregion

    #region Properties
    protected float 	m_fTimeBetweenPress 			= 0f;
    protected float 	m_fLongPressInterval 			= 1f;
    protected int 	    m_fTimesPress 					= 0;
    protected float 	m_fTimeBetweenDoblePress 		= 0f;
    protected float 	m_fBetweenDoblePressInterval 	= 0.5f;
	protected bool 	    m_bIsPressed 					= false;
    protected Vector3   m_vTouchPosition;
    protected bool      m_Inside						= false;
    protected bool	    m_bIsActive						= true;
	#endregion

	#region Monobehaviour Implementation

    public virtual void FixedUpdate() {
		if (Input.touchCount != 0) {
			m_vTouchPosition = Input.GetTouch(0).position;
		} else {
			m_vTouchPosition = Input.mousePosition;
		}
//		DetectLongPress ();
	}

	#endregion

	#region Main Methods
//	private void DetectLongPress() {
//		if (!m_bIsActive) return;
//		if (m_bIsPressed) {
//			if ((Time.time - m_fTimeBetweenPress) >= m_fLongPressInterval) {
//				if (EventOnLongPress != null) {
//					EventOnLongPress(this.gameObject, m_vTouchPosition);
//				}
//			}
//		} else {
//			m_fTimeBetweenPress = Time.time;
//		}
//	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (!m_bIsActive) return;
		if (EventOnPointEnter != null) {
			EventOnPointEnter(this.gameObject, m_vTouchPosition);
		}
		m_bIsPressed = false;
		m_Inside = true;
	}

	public void OnPointerDown (UnityEngine.EventSystems.PointerEventData eventData) {
		if (!m_bIsActive) return;
		if (EventOnPointerDown != null) {
			EventOnPointerDown(this.gameObject, m_vTouchPosition);
		}
		m_bIsPressed = true;
	}

	public void OnPointerUp (UnityEngine.EventSystems.PointerEventData eventData) {
		if (!m_bIsActive) return;
		if (EventOnPointerUp != null && m_Inside) {
			EventOnPointerUp(this.gameObject,m_vTouchPosition);
		}
		m_bIsPressed = false;
		m_fTimesPress++;
		if (m_fTimesPress == 1) {
			m_fTimeBetweenDoblePress = Time.time;
		} 
//		else if (m_fTimesPress >= 2) {
//			if ((Time.time - m_fTimeBetweenDoblePress) <= m_fBetweenDoblePressInterval) {
//				if (EventOnDoublePress != null) {
//					EventOnDoublePress(this.gameObject, m_vTouchPosition);
//				}
//			} 
//			m_fTimesPress = 0;
//		} 
	}

	public void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData) {
		if (!m_bIsActive) return;
		if (EventOnPointerClick != null) {
			EventOnPointerClick(this.gameObject, m_vTouchPosition);
		}
		m_bIsPressed = false;
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (!m_bIsActive) return;
		if (EventOnPointerExit != null) {
			EventOnPointerExit(this.gameObject, m_vTouchPosition);
		}
		m_bIsPressed = false;
		m_fTimesPress = 0;
		m_Inside = false;
	}

	
	#endregion

}
