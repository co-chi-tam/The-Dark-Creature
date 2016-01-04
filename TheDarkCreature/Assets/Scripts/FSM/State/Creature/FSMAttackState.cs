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
		if (m_Controller.GetEnemyEntity() != null)
		{
			var enemyPos = m_Controller.GetEnemyPosition();
			m_Controller.LookAtRotation(enemyPos);
			m_Controller.ActiveSkill(0);
			// Test
//			AttackEnemy();
		}
	}
	
	public override void ExitState()
	{
		m_Controller.SetTargetPosition(m_Controller.TransformPosition);
	}

	public void AttackEnemy() {
		var damage = m_Controller.GetDamage();
		m_Controller.GetEnemyEntity().ApplyDamage(damage, m_Controller.GetEntity());
	}
}
