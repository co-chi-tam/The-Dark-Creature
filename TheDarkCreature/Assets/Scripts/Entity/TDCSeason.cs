using UnityEngine;
using System.Collections;

public class TDCSeason : TDCEntity
{
	#region Properties

	protected TDCSeasonController m_Controller;
	protected TDCSeasonData m_Data;

	#endregion

	#region Contructor

	public TDCSeason(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCSeasonController;
		m_Data = data as TDCSeasonData;

		m_SkillSlotComponent = new TDCSkillSlotComponent(this);
		m_SkillSlotComponent.CreateActiveSkillSlot(TDCEnum.EGameType.WeatherNormalSkill, 
													TDCEnum.EGameType.WeatherRainySkill,
													TDCEnum.EGameType.WeatherOverHeatSkill,
													TDCEnum.EGameType.WeatherWindySkill,
													TDCEnum.EGameType.WeatherSnowySkill);
	}

	#endregion

	#region Main methods

	public override void Update(float dt)
	{
		base.Update(dt);
		m_SkillSlotComponent.UpdateComponent(dt);
	}

	public override void ActiveSkill(int index)
	{
		base.ActiveSkill(index);
		switch ((TDCEnum.EGameType)index)
		{
			case TDCEnum.EGameType.WeatherNormalSkill:
				m_SkillSlotComponent.ActiveSkill(0);
				break;
			case TDCEnum.EGameType.WeatherRainySkill:
				m_SkillSlotComponent.ActiveSkill(1);
				break;
			case TDCEnum.EGameType.WeatherOverHeatSkill:
				m_SkillSlotComponent.ActiveSkill(2);
				break;
			case TDCEnum.EGameType.WeatherWindySkill:
				m_SkillSlotComponent.ActiveSkill(3);
				break;
			case TDCEnum.EGameType.WeatherSnowySkill:
				m_SkillSlotComponent.ActiveSkill(4);
				break;
		}
	}

	public override void ResetObject()
	{
//		base.ResetObject();
	}

	#endregion

	#region Getter && Setter

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCSeasonData;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCSeasonController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetActive(bool value)
	{
		base.SetActive(value);
		if (m_Controller != null && m_Controller.gameObject != null)
		{
			m_Controller.gameObject.SetActive(value);
		}
	}

	public override string GetFSMPath()
	{
		return m_Data.FSMPath;
	}

	public override TDCEnum.EGameType GetGameType()
	{
		return m_Data.GameType;
	}

	public override void SetTransformPosition(Vector3 pos) {
		m_Controller.TransformPosition = pos;
	}

	public override Vector3 GetTransformPosition() {
		return m_Controller.TransformPosition;
	}

	public override float GetDuration()
	{
		return m_Data.Duration;
	}

	#endregion

}