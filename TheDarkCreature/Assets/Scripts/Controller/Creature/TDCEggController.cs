using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TDCEggController : TDCCreatureController
{
	#region Implementation Monobehaviour

	public override void Init ()
	{
		base.Init ();

		m_ColliderLayerMask = 1 << 8;

		var hatchingState	= new FSMHatchingState(this);
		var hatchedState   	= new FSMHatchedState(this);

		m_FSMManager.RegisterState("HatchingState", hatchingState);
		m_FSMManager.RegisterState("HatchedState", hatchedState);

		m_FSMManager.LoadFSM(m_Entity.GetFSMPath());
	}

	#endregion

	#region Main methods

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		m_Entity.ApplyDamage(damage, attacker);
	}

	protected override void FixedUpdate ()
	{
		base.FixedUpdate ();
		m_FSMManager.UpdateState();
	}

	public override TDCEntity SpawnMember()
	{
		var memberType = m_Entity.GetMemberType();
		TDCEntity member = null;
		TDCEntity leader = null;
		var mPos = TransformPosition;
		if (m_GameManager.GetObjectPool(memberType, ref member))
		{
			var colliders = Physics.OverlapSphere(mPos, GetDetectRange(), m_ColliderLayerMask);
			if (colliders.Length == 0)
				leader = null;
			for (int i = 0; i < colliders.Length; i++) {
				var target = m_GameManager.GetEntityByName (colliders[i].name);
				if (target == null || target.GetActive () == false || target == this.GetEntity()) {
					continue;
				} else {
					var isPlayer = TDCUltilities.IsPlayer(target);
					if (isPlayer)
					{
						leader = target;
					}
				}
			}
			member.GetController().ResetObject();
			member.SetLeaderEntity(leader);
			member.GetController().TransformPosition = this.TransformPosition;
			member.SetStartPosition(this.TransformPosition);
			member.SetGroupEntity(null);
			member.SetActive(true);	
		}
		return member;
	}

	public override Dictionary<string, object> GetObjectCurrentValue()
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

	public override float GetDetectRange()
	{
		return m_Entity.GetDetectRange();
	}

	#endregion

}

