using UnityEngine;
using System.Collections;

public class FSMWaitingThree2FiveSecondState : FSMBaseState
{
	public FSMWaitingThree2FiveSecondState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		var random = Mathf.PerlinNoise (Time.time, Time.time) * 5f; // Random.Range(0, 9999) % 2;
		m_Controller.SetAnimation(random < 1f ? EAnimation.Action1 : EAnimation.Action2);
		m_Controller.SetWaitingTime (random < 2.5f ? 1f : 5f);
	}

	public override void UpdateState()
	{

	}

	public override void ExitState()
	{

	}
}
