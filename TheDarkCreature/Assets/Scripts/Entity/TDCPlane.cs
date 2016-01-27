using UnityEngine;
using System.Collections;

public class TDCPlane : TDCEntity
{
	#region Properties

	protected TDCPlaneController m_Controller;
	protected TDCPlaneData m_Data;

	#endregion

	#region Contructor

	public TDCPlane(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCPlaneController;
		m_Data = data as TDCPlaneData;
	}

	#endregion

	#region Main methods

	public override void ResetObject()
	{
//		base.ResetObject();
	}

	#endregion

	#region Getter && Setter

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCPlaneData;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCPlaneController;
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

	public override string GetSeasonSpringTexture()
	{
		return m_Data.SpringTexture;
	}

	public override string GetSeasonSummerTexture()
	{
		return m_Data.SummerTexture;
	}

	public override string GetSeasonAutumnTexture()
	{
		return m_Data.AutumnTexture;
	}

	public override string GetSeasonWinterTexture()
	{
		return m_Data.WinterTexture;
	}

	#endregion
}

