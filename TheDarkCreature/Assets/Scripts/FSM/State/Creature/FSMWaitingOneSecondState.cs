using UnityEngine;
using System.Collections;

public class FSMWaitingOneSecondState : FSMBaseState
{
	public FSMWaitingOneSecondState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Action1);
		m_Controller.SetWaitingTime (1f);
	}
	
	public override void UpdateState()
	{
		
	}
	
	public override void ExitState()
	{
		
	}
}
