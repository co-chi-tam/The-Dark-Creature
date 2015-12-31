using UnityEngine;
using System.Collections;
using FSM;

public class FSMDieState : FSMBaseState
{
 
	public FSMDieState(TDCBaseController controller) : base (controller)
    {

    }

    public override void StartState()
    {
        m_Controller.SetAnimation(EAnimation.Death);
		m_Controller.SetActive (false);
		var group = m_Controller.GetGroupController();
		if (group != null)
		{
			group.ReturnMember(m_Controller);
		}

		var attacker = m_Controller.GetEnemyController() as TDCCreatureController;
		if (attacker != null) 
		{
			var inventory = m_Controller.GetInventory();
			for (int i = 0; i < inventory.Length; i++)
			{
				var itemInInventory = inventory[i];
				if (itemInInventory != null)
				{
					var itemData = itemInInventory.GetData();
					if (attacker.AddItem(itemData.GameType, itemData.ItemType, itemData.Amount) != 1)
					{
						// TODO
					} 
					else 
					{
						break;
					}
					 
				}
			}
		}
		m_Controller.CallBackEvent("OnDealth");
	}

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
		m_Controller.SetEnemyController (null);
    }
}
