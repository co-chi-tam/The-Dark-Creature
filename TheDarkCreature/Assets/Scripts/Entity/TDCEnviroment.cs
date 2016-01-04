using UnityEngine;
using System.Collections;

public class TDCEnviroment : TDCCreature
{
	private TDCEnviromentController m_Controller;
	private TDCEnviromentData m_Data;

	public TDCEnviroment(TDCBaseController ctrl, TDCBaseData data): base(ctrl, data)
	{
		m_Controller = ctrl as TDCEnviromentController;
		m_Data = data as TDCEnviromentData;
	}

	public override void SetController(TDCBaseController controller)
	{
		m_Controller = controller as TDCEnviromentController;
	}

	public override TDCBaseController GetController()
	{
		return m_Controller;
	}

	public override void SetData(TDCBaseData data)
	{
		m_Data = data as TDCEnviromentData;
	}

	public override void SetActive(bool value)
	{
		base.SetActive(value);
		if (m_Controller != null && m_Controller.gameObject != null)
		{
			m_Controller.gameObject.SetActive(value);
		}
	}

	public override int GetHealth ()
	{
		return m_Data.CurrentHP;
	}

	public override string GetFSMPath()
	{
		return m_Data.FSMPath;
	}

}

