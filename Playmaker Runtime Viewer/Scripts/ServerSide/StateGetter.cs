using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine.Networking;

public class StateGetter : MonoBehaviour {

	// for server side
	public PlayMakerFSM fsmObj;
	Dictionary<string,int> fsmDict;
	FSMStateContainer[] stateMsgs;

	// use to check is state change
	string lastStateName = "";

	// Use this for initialization
	void Start () {
		CollectPlaymakerInfos ();

		// if already create server, send data to exsist client
		if (NetworkServer.active) {

			if (NetworkServer.connections.Count > 1)
				SendFsmDataToClient ();

		// if not, create server
		} else {
			
			NetworkServer.RegisterHandler (FsmNetworkMessageType.RequestFSM, GetStateRequest);
			NetworkServer.Listen (12345);

		}
			
	}
		
	void CollectPlaymakerInfos () {
		FsmState[] _states = fsmObj.FsmStates;
		int num = _states.Length;

		stateMsgs = new FSMStateContainer[num];

		fsmDict = new Dictionary<string, int> ();

		// add states into dictionaries
		for (int i = 0; i < num; i++)
			fsmDict.Add (_states [i].Name, i);

		for (int i = 0; i < num; i++) {
			stateMsgs [i] = new FSMStateContainer ();

			stateMsgs [i].name = _states [i].Name;
			stateMsgs [i].description = _states [i].Description;
			stateMsgs [i].position = _states [i].Position;

			// put in transition datas
			if (_states [i].Transitions.Length > 0) {
				int transitionNum = _states [i].Transitions.Length;

				string[] tNames = new string[transitionNum];
				int[] tTargets = new int[transitionNum];

				for (int j = 0; j < transitionNum; j++) {
					tNames [j] = _states [i].Transitions [j].EventName;
					tTargets [j] = fsmDict [_states [i].Transitions [j].ToState];
				}

				stateMsgs [i].transitionNames = tNames;
				stateMsgs [i].transitionTargets = tTargets;
			}
		}

		lastStateName = fsmObj.ActiveStateName;
	}

	void GetStateRequest ( NetworkMessage netMsg ) {
		int connId = netMsg.conn.connectionId;

		FSMMessage sendMsg = new FSMMessage ();
		sendMsg.states = (FSMStateContainer[])stateMsgs.Clone ();

		// send fsm data
		NetworkServer.SendToClient (connId, FsmNetworkMessageType.SendFSM, sendMsg);

		// send now state
		FSMStateMessage fsmStateMsg = new FSMStateMessage ();
		fsmStateMsg.nowStateName = fsmObj.ActiveStateName;
		fsmStateMsg.nowStateIndex = fsmDict [fsmObj.ActiveStateName];

		NetworkServer.SendToClient (connId, FsmNetworkMessageType.PlayMakerStateChange, fsmStateMsg);
	}

	void SendFsmDataToClient () {
		FSMMessage sendMsg = new FSMMessage ();
		sendMsg.states = (FSMStateContainer[])stateMsgs.Clone ();

		// send data to all client
		NetworkServer.SendToAll( FsmNetworkMessageType.SendFSM, sendMsg);

		// send first state
		FSMStateMessage fsmStateMsg = new FSMStateMessage ();
		fsmStateMsg.nowStateName = fsmObj.ActiveStateName;
		fsmStateMsg.nowStateIndex = fsmDict [fsmObj.ActiveStateName];

		NetworkServer.SendToAll (FsmNetworkMessageType.PlayMakerStateChange, fsmStateMsg);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			NetworkServer.Shutdown ();
		}


		// check is state updated
		if (lastStateName != fsmObj.ActiveStateName) {

			// state changed
			int nowStateIndex = fsmDict[ fsmObj.ActiveStateName ];
			SendStateChangeToClient (fsmObj.ActiveStateName, nowStateIndex);

			lastStateName = fsmObj.ActiveStateName;
		}
	}

	void SendStateChangeToClient (string stateName, int stateIndex) {
		FSMStateMessage msg = new FSMStateMessage ();
		msg.nowStateName = stateName;
		msg.nowStateIndex = stateIndex;

		NetworkServer.SendToAll( FsmNetworkMessageType.PlayMakerStateChange, msg );
	}

	void OnApplicationQuit () {
		Debug.Log ("Server Shutdown");
		NetworkServer.Shutdown ();
	}
}
