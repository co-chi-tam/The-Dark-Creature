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
		m_Controller.SetActive(false);
		var group = m_Controller.GetGroupEntity();
		if (group != null)
		{
			group.ReturnMember(m_Controller.GetEntity());
		}
		else
		{
			m_Controller.ReturnObject();
		}
		m_Controller.CallBackEvent("OnDealth");
		m_Controller.DropItem();
		m_Controller.ResetObject();
	}

    public override void UpdateState()
    {
		
    }

    public override void ExitState()
    {
		
    }
}
