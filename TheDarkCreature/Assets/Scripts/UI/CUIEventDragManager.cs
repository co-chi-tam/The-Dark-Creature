using UnityEngine;
using System;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/UI Event Drag Manager")]
public class CUIEventDragManager : CUIEventManager, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Action
    public Action<GameObject, Vector3> EventOnBeginDrag;
    public Action<GameObject, Vector3> EventOnDrag;
    public Action<GameObject, Vector3> EventOnEndDrag;
    public Action<GameObject, Vector3> EventOnDrop;
    #endregion

    #region Monobehaviour Implementation


    #endregion

    #region main method
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!m_bIsActive) return;
        if (EventOnBeginDrag != null)
        {
            EventOnBeginDrag(this.gameObject, m_vTouchPosition);
        }
        m_bIsPressed = false;
        m_fTimesPress = 0;
    }

    public void OnDrag(PointerEventData data)
    {
        if (!m_bIsActive) return;
        if (EventOnDrag != null)
        {
            EventOnDrag(this.gameObject, m_vTouchPosition);
        }
        m_bIsPressed = false;
        m_fTimesPress = 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_bIsActive) return;
        if (EventOnEndDrag != null)
        {
            EventOnEndDrag(this.gameObject, m_vTouchPosition);
        }
        m_bIsPressed = false;
        m_fTimesPress = 0;
    }

    public void OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (!m_bIsActive) return;
        if (EventOnDrop != null)
        {
            EventOnDrop(this.gameObject, m_vTouchPosition);
        }
        m_bIsPressed = false;
        m_fTimesPress = 0;
    }
    #endregion
}
