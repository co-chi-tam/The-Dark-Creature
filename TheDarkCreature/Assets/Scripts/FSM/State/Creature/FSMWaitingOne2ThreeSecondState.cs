using UnityEngine;
using System.Collections;

public class FSMWaitingOne2ThreeSecondState : FSMBaseState
{
	public FSMWaitingOne2ThreeSecondState(TDCBaseController controller) : base (controller)
	{
		
	}
	
	public override void StartState()
	{
		var random = Mathf.PerlinNoise (Time.time, Time.time) * 3f; // Random.Range(0, 9999) % 2;
		m_Controller.SetAnimation(random < 1f ? EAnimation.Action1 : EAnimation.Action2);
		m_Controller.SetWaitingTime (random < 1f ? 1f : 3f);
	}
	
	public override void UpdateState()
	{
		
	}
	
	public override void ExitState()
	{
		
	}
}
