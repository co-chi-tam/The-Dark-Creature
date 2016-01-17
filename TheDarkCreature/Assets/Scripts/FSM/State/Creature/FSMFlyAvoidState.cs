using UnityEngine;
using FSM;

public class FSMFlyAvoidState : FSMBaseState
{
	public FSMFlyAvoidState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Run);
		var direction = (m_Controller.TransformPosition - m_Controller.GetEnemyPosition()).normalized;
		var newTarget = m_Controller.TransformPosition + direction * (m_Controller.GetDetectRange() + 10f);
		newTarget.y = 0f;
		m_Controller.SetTargetPosition(newTarget);
		m_Controller.CallBackEvent("OnAvoid");
	}

	public override void UpdateState()
	{
		var target = m_Controller.GetTargetPosition();
		m_Controller.FlyPosition(target);
	}

	public override void ExitState()
	{
		m_Controller.SetEnemyEntity(null);
	}
}