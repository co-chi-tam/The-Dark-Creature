using UnityEngine;
using System.Collections;

public class TDCObjectController : TDCBaseController
{
	public override UIItemController[] GetInventory()
	{
		return m_Entity.GetInventory();
	}
}

