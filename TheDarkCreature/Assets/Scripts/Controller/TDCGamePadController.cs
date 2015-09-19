using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TDCGamePadController : MonoBehaviour {

    [SerializeField]
    private TDCPlayerController m_PlayerController;
    [SerializeField]
    private CUIEventManager m_TrapButton;

    private Vector2 m_StartDragPosition;

    void Start () {
        
    }


}
