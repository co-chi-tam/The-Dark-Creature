using UnityEngine;
using System.Collections;
using FSM;

public class FSMBaseState : IState {

	protected TDCBaseController m_Controller;

	public FSMBaseState(TDCBaseController controller)
	{
		m_Controller = controller;
	}
	
	public virtual void StartState()
	{

	}
	
	public virtual void UpdateState()
	{
		
	}
	
	public virtual void ExitState()
	{
		
	}

}
