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
//		var direction = (m_Controller.TransformPosition - m_Controller.GetEnemyPosition()).normalized;
//		var newTarget = m_Controller.TransformPosition + direction * (m_Controller.GetDetectRange() + 10f);
//		newTarget.y = 0f;
//		m_Controller.SetTargetPosition(newTarget);

		var groupCtrl = m_Controller.GetGroupEntity();
		var groupPosition = m_Controller.GetStartPosition();
		var groupRadius = 50f;
		if (groupCtrl != null)
		{
			groupRadius = groupCtrl.GetGroupRadius();
			groupPosition = m_Controller.GetGroupEntity().GetTransformPosition();
		}
		var random = Random.insideUnitCircle * groupRadius; // TDCUltilities.RandomAround(groupRadius);
		m_Controller.SetTargetPosition (groupPosition + new Vector3(random.x, 0f, random.y));
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