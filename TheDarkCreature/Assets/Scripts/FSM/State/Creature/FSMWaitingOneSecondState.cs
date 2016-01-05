using UnityEngine;
using System.Collections;

public class FSMWaitingOneSecondState : FSMBaseState
{
	public FSMWaitingOneSecondState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState()
	{
		var random = Random.Range(0, 9999) % 2;
		m_Controller.SetAnimation(random < 1 ? EAnimation.Action1 : EAnimation.Action2);
		m_Controller.SetWaitingTime (1f);
	}
	
	public override void UpdateState()
	{
		
	}
	
	public override void ExitState()
	{
		
	}
}
