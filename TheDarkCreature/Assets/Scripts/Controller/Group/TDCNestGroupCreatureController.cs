﻿using UnityEngine;
using System.Collections;

public class TDCNestGroupCreatureController : TDCGroupCreatureController
{
	#region Properties

	#endregion

	#region Implementation Mono

	public override void Init()
	{
		base.Init();
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
