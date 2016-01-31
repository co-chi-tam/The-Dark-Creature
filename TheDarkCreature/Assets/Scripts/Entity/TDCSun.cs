using UnityEngine;
using System.Collections;

public class TDCSun : TDCEntity
{
	#region Properties

	protected TDCSunController m_Controller;
	protected TDCSunData m_Data;

	protected TDCObjectProperty<float> m_IntensityOffset;

	#endregion

	#region Contructor

	public TDCSun(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCSunController;
		m_Data = data as TDCSunData;

		m_IntensityOffset = new TDCObjectProperty<float>("IntensityOffset");

		RegisterProperty(m_IntensityOffset);
	}

	#endregion

	#region Main methods

	public override void Update(float dt)
	{
		base.Update(dt);
	}

	public override void ResetObject()
	{
		base.ResetObject();
	}

	#endregion

	#region Getter && Setter

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCSunData;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCSunController;
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

	public override float GetIntensityOffset() {
		var value = m_IntensityOffset.Value;
		m_IntensityOffset.Value = 0f;
		return value;
	}

	#endregion

}

