using EntityStates;

namespace WitchMod.SkillStates
{
	class BaseWitchSkill : BaseSkillState
	{
		public override void OnEnter()
		{
			base.OnEnter();

			WitchItemBehavior item = GetComponent<WitchItemBehavior>();

			if (item != null)
			{
				item.SkillCasted();
			}
		}
	}
}