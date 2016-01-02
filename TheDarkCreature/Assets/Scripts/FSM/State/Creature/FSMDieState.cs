using UnityEngine;
using System.Collections;
using FSM;

public class FSMDieState : FSMBaseState
{
 
	public FSMDieState(TDCBaseController controller) : base (controller)
    {

    }

    public override void StartState()
    {
        m_Controller.SetAnimation(EAnimation.Death);
		m_Controller.ResetObject();
		m_Controller.SetActive(false);
		var group = m_Controller.GetGroupController();
		if (group != null)
		{
			group.ReturnMember(m_Controller);
		}
		m_Controller.CallBackEvent("OnDealth");
	}

    public override void UpdateState()
    {
		
    }

    public override void ExitState()
    {
		
    }
}
