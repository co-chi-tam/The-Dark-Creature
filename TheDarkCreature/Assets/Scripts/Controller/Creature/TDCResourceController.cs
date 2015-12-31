using UnityEngine;
using System.Collections;
using FSM;

public class TDCResourceController : TDCCreatureController {

	#region Property

	protected TDCResourceData m_ResourceData;

	#endregion
	
	#region Implementation Mono

	protected override void Start()
	{
		base.Start();

		m_FSMManager.RegisterCondition("IsDie", IsDeath);

		m_FSMManager.LoadFSM(m_ResourceData.FSMPath);
		
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
		StateName = m_FSMManager.StateCurrentName;
	}
	
	#endregion
	
	#region Main method

	internal override bool IsDeath() {
		base.IsDeath();
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
