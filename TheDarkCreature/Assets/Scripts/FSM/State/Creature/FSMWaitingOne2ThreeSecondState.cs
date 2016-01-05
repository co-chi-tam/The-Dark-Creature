using UnityEngine;
using System.Collections;

public class FSMWaitingOne2ThreeSecondState : FSMBaseState
{
	public FSMWaitingOne2ThreeSecondState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState()
	{
		var random = Random.Range(0, 9999) % 2;
		m_Controller.SetAnimation(random < 1 ? EAnimation.Action1 : EAnimation.Action2);
		m_Controller.SetWaitingTime (random < 1 ? 1f : 3f);
	}
	
	public override void UpdateState()
	{
		
	}
	
	public override void ExitState()
	{
		
	}
}
