using UnityEngine;
using FSM;

public class FSMExcuteEffectSkillState : FSMBaseState {

	private TDCSkillController m_SkillController;

	public FSMExcuteEffectSkillState(TDCBaseController controller) : base (controller)
	{
		m_SkillController = controller as TDCSkillController;
	}

	public override void StartState() {
		m_SkillController.ExcuteEffect();
	}

	public override void UpdateState() {
		if (m_SkillController.GetSkillType() == TDCEnum.ESkillType.EffectUpdate) {
			m_SkillController.ExcuteEffect();
		}
	}

	public override void ExitState()
	{

	}
}