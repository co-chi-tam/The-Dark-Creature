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

	public override void AddSkillChain(int index)
	{
		base.AddSkillChain(index);
		switch ((TDCEnum.EGameType)index)
		{
			case TDCEnum.EGameType.WeatherNormalSkill:
				m_SkillSlotComponent.AddSkillChain(0);
				break;
			case TDCEnum.EGameType.WeatherRainySkill:
				m_SkillSlotComponent.AddSkillChain(1);
				break;
			case TDCEnum.EGameType.WeatherOverHeatSkill:
				m_SkillSlotComponent.AddSkillChain(2);
				break;
			case TDCEnum.EGameType.WeatherWindySkill:
				m_SkillSlotComponent.AddSkillChain(3);
				break;
			case TDCEnum.EGameType.WeatherSnowySkill:
				m_SkillSlotComponent.AddSkillChain(4);
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

	public override float GetMinMoistureSpring() {
		return m_Data.MoistureSpring[0];
	}

	public override float GetMaxMoistureSpring() {
		return m_Data.MoistureSpring[1];
	}

	public override float GetMinMoistureSummer() {
		return m_Data.MoistureSummer[0];
	}

	public override float GetMaxMoistureSummer() {
		return m_Data.MoistureSummer[1];
	}

	public override float GetMinMoistureAutumn() {
		return m_Data.MoistureAutumn[0];
	}

	public override float GetMaxMoistureAutumn() {
		return m_Data.MoistureAutumn[1];
	}

	public override float GetMinMoistureWinter() {
		return m_Data.MoistureWinter[0];
	}

	public override float GetMaxMoistureWinter() {
		return m_Data.MoistureWinter[1];
	}

	public override float GetMinTemperatureSpring() {
		return m_Data.TemperatureSpring[0];
	}

	public override float GetMaxTemperatureSpring() {
		return m_Data.TemperatureSpring[1];
	}

	public override float GetMinTemperatureSummer() {
		return m_Data.TemperatureSummer[0];
	}

	public override float GetMaxTemperatureSummer() {
		return m_Data.TemperatureSummer[1];
	}

	public override float GetMinTemperatureAutumn() {
		return m_Data.TemperatureAutumn[0];
	}

	public override float GetMaxTemperatureAutumn() {
		return m_Data.TemperatureAutumn[1];
	}

	public override float GetMinTemperatureWinter() {
		return m_Data.TemperatureWinter[0];
	}

	public override float GetMaxTemperatureWinter() {
		return m_Data.TemperatureWinter[1];
	}

	#endregion

}