using UnityEngine;
using FSM;

public class FSMChaseState : FSMBaseState {

	private TDCCreatureData m_Data;

	public FSMChaseState(TDCBaseController controller) : base (controller)
	{
		m_Data = controller.GetData() as TDCCreatureData;
	}
	
	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
	}
	
	public override void UpdateState() {
		var enemyPos = m_Controller.GetEnemyPosition();
		m_Controller.RunPosition(enemyPos);
	}
	
	public override void ExitState()
	{
		
	}
}
