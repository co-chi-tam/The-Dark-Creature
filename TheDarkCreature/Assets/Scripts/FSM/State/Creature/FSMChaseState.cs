using UnityEngine;
using FSM;

public class FSMChaseState : FSMBaseState {

	public FSMChaseState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState() {
		m_Controller.SetAnimation(EAnimation.Run);
		m_Controller.CallBackEvent("OnMove");
	}
	
	public override void UpdateState() {
		if (m_Controller.GetEnemyEntity() != null)
		{
			var enemyPos = m_Controller.GetEnemyPosition();
			m_Controller.MovePosition(enemyPos);
		}
	}
	
	public override void ExitState()
	{
		
	}
}
