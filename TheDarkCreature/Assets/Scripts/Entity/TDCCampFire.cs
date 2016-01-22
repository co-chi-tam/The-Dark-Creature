using UnityEngine;
using System.Collections;

public class TDCCampFire : TDCObject
{
	#region Properties
	protected TDCSkillSlot m_NormalSkill;
	#endregion

	#region Contructor

	public TDCCampFire(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_NormalSkill = new TDCSkillSlot(TDCEnum.EGameType.FlameBodySkill, this);
	}

	#endregion

	#region Main methods

	public override void Update(float dt)
	{
		base.Update(dt);
		m_NormalSkill.UpdateSkill(Time.fixedDeltaTime);
	}

	public override void ActiveSkill(int index)
	{
		base.ActiveSkill(index);
		if (index == 0)
		{
			m_NormalSkill.ActiveSkill();
		}
		else
		{
			// TODO
		}
	}

	#endregion

	#region Getter && Setter

	public override bool GetIsShine()
	{
		return m_Data.IsShine;
	}

	#endregion

}

