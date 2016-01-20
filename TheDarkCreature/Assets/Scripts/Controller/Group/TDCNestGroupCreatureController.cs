using UnityEngine;
using System.Collections;

public class TDCNestGroupCreatureController : TDCGroupCreatureController
{
	#region Properties

	#endregion

	#region Implementation Mono

	public override void Init()
	{
		base.Init();

		m_FSMManager.RegisterCondition("CanSpawnEggANDCountdownWaitingTime", CanSpawnEggANDCountdownWaitingTime);
	}

	protected override void Start()
	{
		base.Start();
	}

	#endregion

	#region Main methods

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		m_Entity.ApplyDamage(damage, attacker);
	}

	public override void SetAnimation(EAnimation anim)
	{
		base.SetAnimation(anim);
		if (m_AnimatorController.runtimeAnimatorController != null) {
			m_AnimatorController.SetInteger ("AnimParam", (int) anim);
		}
	}

	public override void ResetObject()
	{
		base.ResetObject();
		m_Entity.ResetObject();
		m_PositionIndex = -1;
		SetCurrentMember(0);
	}

	#endregion

	#region FSM

	internal override bool IsDeath() {
		return GetHealth() <= 0f || GetActive() == false;
	}

	#endregion

	#region Getter && Setter

	public override int GetHealth()
	{
		return m_Entity.GetHealth();
	}

	public override bool GetActive()
	{
		return m_Entity.GetActive();
	}

	#endregion

}

