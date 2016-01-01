using UnityEngine;
using FSM;

public class FSMAttackState : FSMBaseState {

	public FSMAttackState(TDCBaseController controller) : base (controller)
	{

	}
	
	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Attack1);
		m_Controller.CallBackEvent("OnAttack");
	}

	public override void UpdateState() {
		if (m_Controller.GetEnemyController() != null)
		{
			var enemyPos = m_Controller.GetEnemyPosition();
			m_Controller.LookAtRotation(enemyPos);
			m_Controller.ActiveSkill(0);
			// Test
			AttackEnemy();
		}
	}
	
	public override void ExitState()
	{
		m_Controller.SetTargetPosition(m_Controller.TransformPosition);
	}

	public void AttackEnemy() {
		var damage = Random.Range(m_Controller.GetMinDamage(), m_Controller.GetMaxDamage());
		m_Controller.GetEnemyController().ApplyDamage(1, m_Controller);
	}
}
