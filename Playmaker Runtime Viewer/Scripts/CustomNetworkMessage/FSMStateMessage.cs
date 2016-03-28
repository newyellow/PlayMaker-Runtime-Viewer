using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FSMStateMessage : MessageBase {

	public string nowStateName = "";
	public int nowStateIndex = 0;

	public FSMStateMessage () {
	}
}
