using UnityEngine;
using System.Collections;
using FSM;

public class FSMFlyHomeState : FSMBaseState
{
	public FSMFlyHomeState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Run);
		var groupCtrl = m_Controller.GetGroupEntity();
		var homePosition = m_Controller.GetStartPosition();
		if (groupCtrl != null)
		{
			homePosition = m_Controller.GetGroupEntity().GetTransformPosition();
		}
		m_Controller.SetTargetPosition (homePosition);
	}

	public override void UpdateState()
	{
		var targetPos = m_Controller.GetTargetPosition();
		m_Controller.FlyPosition(targetPos);
		m_Controller.CallBackEvent("OnMove");
	}

	public override void ExitState()
	{

	}
}