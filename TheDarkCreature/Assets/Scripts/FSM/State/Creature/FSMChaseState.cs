using UnityEngine;
using FSM;

public class FSMChaseState : FSMBaseState {

	public FSMChaseState(TDCBaseController controller) : base (controller)
	{
		
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
