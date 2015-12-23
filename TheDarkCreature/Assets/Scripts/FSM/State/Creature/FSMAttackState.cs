using UnityEngine;
using FSM;

public class FSMAttackState : FSMBaseState {

	private TDCCreatureData m_CreatureData;
	private float m_DelayTime = 1f;

	public FSMAttackState(TDCBaseController controller) : base (controller)
	{
		m_CreatureData = controller.GetData () as TDCCreatureData;
	}
	
	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Attack1);
		m_DelayTime = 1f;

		var enemy = m_Controller.GetEnemyController();
		if (enemy != null)
		{
			m_Controller.GetEnemyController().SetEnemyController(m_Controller);
		}
	}

	public override void UpdateState() {
		var enemyPos = m_Controller.GetEnemyPosition();
		m_Controller.LookAtRotation(enemyPos);
		m_DelayTime -= Time.deltaTime;
		if (m_DelayTime < 0f) {
			AttackTarget();
			m_DelayTime = 1f;
		}
	}
	
	public override void ExitState()
	{
		
	}

	private void AttackTarget() {
		var randomDamage = Random.Range (m_CreatureData.MinDamage, m_CreatureData.MaxDamage);
		m_Controller.GetEnemyController ().ApplyDamage (randomDamage, m_Controller);
	}
}
