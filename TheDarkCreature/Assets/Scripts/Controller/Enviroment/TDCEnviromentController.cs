using UnityEngine;
using System.Collections;
using FSM;

public class TDCEnviromentController : TDCCreatureController {

	#region Property

	#endregion
	
	#region Implementation Mono

	public override void Init()
	{
		base.Init();

		m_FSMManager.RegisterCondition("IsDie", IsDeath);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	protected override void Start()
	{
		base.Start();
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
	}
	
	#endregion
	
	#region Main method

	internal override bool IsDeath() {
		return m_Entity.GetHealth() <= 0f;
	}

	#endregion

	#region Getter & Setter 

	#endregion

}
