using UnityEngine;
using FSM;

public class FSMAttackState : FSMBaseState {

	private TDCCreatureData m_CreatureData;

	public FSMAttackState(TDCBaseController controller) : base (controller)
	{
		m_CreatureData = controller.GetData () as TDCCreatureData;
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
			AttackTarget();
		}
	}
	
	public override void ExitState()
	{
		m_Controller.SetTargetPosition(m_Controller.TransformPosition);
	}

	private void AttackTarget() {
//		var randomDamage = Random.Range (m_CreatureData.MinDamage, m_CreatureData.MaxDamage);
//		m_Controller.GetEnemyController ().ApplyDamage (randomDamage, m_Controller);
		m_Controller.ActiveSkill();
	}
}
