using UnityEngine;
using System.Collections;

public class FSMSleepState : FSMBaseState
{
	public FSMSleepState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Sleep);
	}

	public override void UpdateState()
	{

	}

	public override void ExitState()
	{

	}
}