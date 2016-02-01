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
		var group = m_Controller.GetGroupEntity();
		if (group != null)
		{
			if (group.GetActive())
			{
				group.ReturnMember(m_Controller.GetEntity());
			}
			else
			{
				m_Controller.ReturnObject();
			}
		}
		else
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
