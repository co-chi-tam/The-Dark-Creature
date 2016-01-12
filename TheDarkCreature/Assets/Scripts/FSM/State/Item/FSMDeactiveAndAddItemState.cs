using UnityEngine;
using System.Collections;
using FSM;

public class FSMDeactiveAndAddItemState : FSMBaseState
{

	public FSMDeactiveAndAddItemState(TDCBaseController controller) : base (controller)
	{

	}

	public override void StartState()
	{
		var enemy = m_Controller.GetEnemyEntity();
		var canAddItem = true;
		if (enemy != null)
		{
			var entity = m_Controller.GetEntity();
			var gameType = entity.GetGameType();
			var itemType = entity.GetItemType();
			var amount = 1;
			canAddItem = enemy.AddItemInventory(gameType, itemType, amount) != -1;
		}
		m_Controller.SetActive(false);
		var group = m_Controller.GetGroupEntity();
		if (group != null)
		{
			group.ReturnMember(m_Controller.GetEntity());
		}
		else
		{
			m_Controller.ReturnObject(m_Controller.GetEntity());
		}
		if (canAddItem == false)
		{
			m_Controller.DropItem();
		}
	}

	public override void UpdateState()
	{

	}

	public override void ExitState()
	{
		m_Controller.ResetObject();
	}
}

