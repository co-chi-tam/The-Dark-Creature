using UnityEngine;
using FSM;

public class FSMHuntingState : FSMBaseState {
	
	public FSMHuntingState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
	}
	
	public override void UpdateState() {
		var enemyPos = m_Controller.GetEnemyPosition();
		m_Controller.RunPosition(enemyPos);
		m_Controller.MoveRotation(enemyPos);
	}
	
	public override void ExitState()
	{
		
	}
}
