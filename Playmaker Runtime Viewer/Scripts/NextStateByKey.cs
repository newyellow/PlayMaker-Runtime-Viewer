using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class NextStateByKey : MonoBehaviour {

	public PlayMakerFSM fsmObj;
	public KeyCode nextEventKey = KeyCode.Q;
	public string eventName = "GoNext";

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (nextEventKey))
			fsmObj.SendEvent (eventName);
	}
}
