using UnityEngine;
using System.Collections;
using FSM;

public class TDCResourceController : TDCCreatureController {

	#region Property

	protected TDCResourceData m_ResourceData;

	#endregion
	
	#region Implementation Mono
	public override void Start()
	{
		base.Start();

		m_FSMMamager.RegisterCondition("IsDie", IsDie);

		m_FSMMamager.LoadFSM(m_ResourceData.FSMPath);
		
	}
	
	public override void FixedUpdate() {
		base.FixedUpdate ();
		m_FSMMamager.UpdateState();
		StateName = m_FSMMamager.StateCurrentName;
	}
	
	#endregion
	
	#region Main method

	internal override bool IsDie() {
		base.IsDie();
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
