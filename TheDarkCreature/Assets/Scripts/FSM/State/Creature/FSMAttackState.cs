using UnityEngine;
using FSM;

public class FSMAttackState : FSMBaseState {

	private TDCCreatureData m_CreatureData;
	private float m_DelayTime = 0f;

	public FSMAttackState(TDCBaseController controller) : base (controller)
	{
		m_CreatureData = controller.GetData () as TDCCreatureData;
	}
	
	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Attack1);
		AttackTarget ();
	}
	
	public override void UpdateState() {
		m_Controller.MoveRotation (m_Controller.GetEnemyPosition ());
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
