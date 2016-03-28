using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Sends a Message to a Game Object. See Unity docs for SendMessage.")]
	public class SliderAction : FsmStateAction {

		public VJObject vjSlider;

		public override void Reset () {
			Name = "SliderAction";
		}

	}
}