using UnityEngine;
using System.Collections;
using FSM;

public class FSMBaseState : IState {

	protected TDCBaseController m_Controller;

	public FSMBaseState(IContext context)
	{
		m_Controller = context as TDCBaseController;
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
