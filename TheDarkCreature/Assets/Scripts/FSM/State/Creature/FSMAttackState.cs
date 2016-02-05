using UnityEngine;
using FSM;

public class FSMAttackState : FSMBaseState {

	public FSMAttackState(TDCBaseController controller) : base (controller)
	{

	}
	
	public override void StartState() {
		var random = Random.Range(0, 9999) % 2;
		m_Controller.SetAnimation(random < 1 ? EAnimation.Attack1 : EAnimation.Attack2);
		m_Controller.CallBackEvent("OnAttack");
	}

	public override void UpdateState() {
		if (m_Controller.GetEnemyEntity() != null)
		{
			var enemyPos = m_Controller.GetEnemyPosition();
			m_Controller.LookAtRotation(enemyPos);
			m_Controller.Activekill(0);
		}
	}
	
	public override void ExitState()
	{
		m_Controller.SetTargetPosition(m_Controller.TransformPosition);
	}

}
