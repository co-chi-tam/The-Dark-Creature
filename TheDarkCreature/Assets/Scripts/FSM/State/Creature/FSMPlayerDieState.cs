using UnityEngine;
using FSM;

public class FSMPlayerDeathState : FSMBaseState
{
	public FSMPlayerDeathState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		m_Controller.SetAnimation(EAnimation.Death);
		m_Controller.SetActive(false);
		m_Controller.CallBackEvent("OnDealth");
	}

	public override void UpdateState()
	{

	}

	public override void ExitState()
	{
		m_Controller.ResetObject();
	}
}

