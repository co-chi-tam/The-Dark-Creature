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
		m_Controller.VisibleObject(false);
	}

	public override void UpdateState()
	{

	}

	public override void ExitState()
	{
		m_Controller.VisibleObject(true);
	}
}