using UnityEngine;
using System.Collections;
using FSM;

public class FSMFindRandomState : FSMBaseState
{
	public FSMFindRandomState(TDCBaseController controller) : base (controller)
    {

	}

	public override void StartState()
    {
        m_Controller.SetAnimation(EAnimation.Idle);
		var groupCtrl = m_Controller.GetGroupController();
		var groupPosition = m_Controller.GetStartPosition();
		var groupRadius = 50f;
		if (groupCtrl != null)
		{
			groupRadius = groupCtrl.GetRadius();
			groupPosition = m_Controller.GetGroupController().GetStartPosition();
		}
		var random = Random.insideUnitCircle * groupRadius; // TDCUltilities.RandomAround(groupRadius);
		m_Controller.SetTargetPosition (groupPosition + new Vector3(random.x, 0f, random.y));
		m_Controller.SetWaitingTimeInterval (Random.Range (1f, 3f));
		m_Controller.CallBackEvent("OnFindRandom");
    }

	public override void UpdateState()
    {

	}

	public override void ExitState()
    {

    }
}