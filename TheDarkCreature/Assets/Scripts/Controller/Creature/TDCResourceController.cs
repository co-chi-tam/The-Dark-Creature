using UnityEngine;
using System.Collections;
using FSM;

public class TDCResourceController : TDCCreatureController {

	#region Property

	private FSMManager m_FSMMamager;
	private TDCGameManager m_GameManager;
	protected TDCResourceData m_ResourceData;

	#endregion
	
	#region Implementation Mono
	public override void Start()
	{
		base.Start();
		
		m_FSMMamager = new FSMManager();
		
		var idleState = new FSMIdleState(this);
		var dieState = new FSMDieState(this);
		
		m_FSMMamager.RegisterState("IdleState", idleState);
		m_FSMMamager.RegisterState("DieState", dieState);
		
		m_FSMMamager.RegisterCondition("IsDie", IsDie);
		m_FSMMamager.LoadFSM(m_ResourceData.FSMPath);
		
		m_GameManager = TDCGameManager.GetInstance();

	}
	
	public override void FixedUpdate() {
		base.Update ();
		m_FSMMamager.UpdateState();
		StateName = m_FSMMamager.StateCurrentName;
	}
	
	#endregion
	
	#region Main method

	private bool IsDie() {
		return m_CreatureData.CurrentHP <= 0f;
	}

	#endregion

	#region Getter & Setter 

	public override void SetData (TDCBaseData data)
	{
		base.SetData (data);
		m_ResourceData = data as TDCResourceData;
	}

	public override TDCBaseData GetData ()
	{
		return m_ResourceData;
	}

	#endregion
}
