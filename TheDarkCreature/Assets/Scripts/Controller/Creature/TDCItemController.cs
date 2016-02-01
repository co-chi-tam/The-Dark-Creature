using UnityEngine;
using System.Collections;

public class TDCItemController : TDCCreatureController
{
	#region Property

	#endregion

	#region Implementation Mono

	public override void Init()
	{
		base.Init();

		var deactiveAndAddItemState = new FSMDeactiveAndAddItemState(this);

		m_FSMManager.RegisterState("DeactiveAndAddItemState", deactiveAndAddItemState);
		m_FSMManager.RegisterCondition("IsDeactive", IsDeactive);

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

	#region Main methods

	public override void ApplyDamage(int damage, TDCEntity attacker)
	{
		base.ApplyDamage(damage, attacker);
		m_Entity.ApplyDamage(damage, attacker);
	}

	public override void ResetObject()
	{
		base.ResetObject();
		m_Entity.ResetObject();
	}

	public override void ReturnObject()
	{
		base.ReturnObject();
		m_GameManager.SetObjectPool(this.GetEntity());
	}

	public override void DropItem()
	{
		base.DropItem();
		var itemType = m_Entity.GetGameType();
		var amount = 1;
		TDCEntity item = null;
		if (m_GameManager.GetObjectPool(itemType, ref item))
		{
			var pos = UnityEngine.Random.insideUnitCircle * GetColliderRadius();
			item.GetController().TransformPosition = this.TransformPosition + new Vector3(pos.x, 0f, pos.y);
			item.SetActive(true);
		}
	}

	#endregion

	#region FSM

	internal virtual bool IsDeactive() {
		return m_Entity.GetItemActive();
	}

	#endregion

	#region Getter & Setter 

	public override TDCEntity GetEnemyEntity()
	{
		return m_Entity.GetEnemyEntity();
	}

	public override float GetColliderRadius()
	{
		return base.GetColliderRadius();
	}

	#endregion

}

