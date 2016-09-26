using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FsmDataMessage : MessageBase {

	public FSMStateContainer stateData;

	public FsmDataMessage (FSMStateContainer fsmStateData) {
		stateData = fsmStateData;
	}

	public FsmDataMessage () {
	}
}
