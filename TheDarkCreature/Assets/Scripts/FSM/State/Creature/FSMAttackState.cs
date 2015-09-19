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
		var randomDamage = Random.Range (m_CreatureData.MinDamage, m_CreatureData.MaxDamage);
		m_Controller.GetEnemyController ().ApplyDamage (randomDamage);
	}
	
	public override void UpdateState() {
		
	}
	
	public override void ExitState()
	{
		
	}
}
