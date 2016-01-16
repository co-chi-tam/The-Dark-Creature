using UnityEngine;
using System.Collections;

public class TDCEggController : TDCCreatureController
{
	#region Implementation Monobehaviour

	public override void Init ()
	{
		base.Init ();

		var hatchingState	= new FSMHatchingState(this);
		var hatchedState   	= new FSMHatchedState(this);

		m_FSMManager.RegisterState("HatchingState", hatchingState);
		m_FSMManager.RegisterState("HatchedState", hatchedState);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	#endregion

	#region Main methods

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
	}

	public override TDCEntity SpawnMember()
	{
		return base.SpawnMember();
	}

	public override System.Collections.Generic.Dictionary<string, object> GetObjectCurrentValue()
	{
		var tmp = base.GetObjectCurrentValue();
		tmp["MemberType"] = m_Entity.GetMemberType();
		tmp["Duration"] = m_WaitingTime;
		return tmp;
	}

	#endregion

	#region FSM

	internal override bool IsDeath()
	{
		return GetActive() == false || m_Entity.GetHealth() <= 0 || m_Entity.GetHeat() >= m_Entity.GetMaxHeat() || m_Entity.GetHeat() <= 0;
	}

	#endregion

	#region Getter && Setter

	public override float GetDuration()
	{
		return m_Entity.GetDuration();
	}

	#endregion

}

