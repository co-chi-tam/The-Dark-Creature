using UnityEngine;
using System.Collections;
using FSM;

public class FSMGoHomeState : FSMBaseState
{
	public FSMGoHomeState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Run);
		var groupCtrl = m_Controller.GetGroupEntity();
		var homePosition = m_Controller.GetStartPosition();
		var distance = 20f;
		if (groupCtrl != null && groupCtrl.GetActive())
		{
			homePosition = m_Controller.GetGroupEntity().GetTransformPosition();
		}
		else
		{
			var random = Random.insideUnitCircle * distance; // TDCUltilities.RandomAround(groupRadius);
			homePosition = homePosition + new Vector3(random.x, 0f, random.y);
		}
		m_Controller.SetTargetPosition (homePosition);
	}

	public override void UpdateState()
	{
		var targetPos = m_Controller.GetTargetPosition();
		m_Controller.MovePosition(targetPos);
		m_Controller.CallBackEvent("OnMove");
	}

	public override void ExitState()
	{

	}
}