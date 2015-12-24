using UnityEngine;
using System.Collections;
using Effect;

public class TDCFlameBodyController : TDCSkillController {

	public TDCFlameBodyController() : base ()
	{
		
	}

	public TDCFlameBodyController(TDCSkillData skillData, TDCBaseController ownerCtrl) : base (skillData, ownerCtrl)
	{
		m_TimeDelay = skillData.TimeDelay;
	}

	public void UpdateSkill(float dt) {
		m_TimeDelay -= dt;
	}

	internal override void ExcuteEffect(object[] pas)
	{
		var name = pas[0].ToString();
		var heatPoint = int.Parse (pas[1].ToString());
		for (int i = 0; i < m_ControllerInsideRadius.Length; i++)
		{
			m_ControllerInsideRadius[i].SetHeat(heatPoint);
		}
	}

}
