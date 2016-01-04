using UnityEngine;
using System.Collections;
using FSM;

public class FSMDisableState : FSMBaseState {

	public FSMDisableState(TDCBaseController controller) : base (controller)
	{

	}
	
	public override void StartState() {
		m_Controller.SetActive(false);
		m_Controller.ResetObject();
		if (m_Controller.GetGroupEntity() == null)
		{
			m_Controller.ReturnObject();
		}
	}
	
	public override void UpdateState() {
		
	}
	
	public override void ExitState()
	{
		
	}
}
