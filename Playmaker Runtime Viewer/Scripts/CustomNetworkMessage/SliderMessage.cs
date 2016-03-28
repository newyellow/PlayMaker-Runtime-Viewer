using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SliderMessage : MessageBase {

	public int sliderIndex = 0;
	public float value = 0.0f;

	public SliderMessage () {
		sliderIndex = -1;
		value = 0.0f;
	}
}
