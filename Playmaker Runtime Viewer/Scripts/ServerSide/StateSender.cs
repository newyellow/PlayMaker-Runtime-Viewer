using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine.Networking;

public class StateSender : MonoBehaviour {

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

			if (NetworkServer.connections.Count > 1)
				SendFsmDataToClient ();
		}
			
	}
		
	void CollectPlaymakerInfos () {
		FsmState[] _states = fsmObj.FsmStates;
		int num = _states.Length;

		stateMsgs = new FSMStateContainer[num];

		fsmDict = new Dictionary<string, int> ();

		// add states into dictionaries
		// for knowing the state indexes
		for (int i = 0; i < num; i++)
			fsmDict.Add (_states [i].Name, i);

		for (int i = 0; i < num; i++) {
			stateMsgs [i] = new FSMStateContainer ();

			stateMsgs [i].stateId = i; // knowing itself's id, use for linking
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
		// if want to send to spacific client
		// int connId = netMsg.conn.connectionId; 

		SendFsmDataToClient ();
		UpdateNowStateToClient ();
	}

	void SendFsmDataToClient () {

		// send start sending signal
		SimpleMessage startMessage = new SimpleMessage("Sending Start!");
		NetworkServer.SendToAll (FsmNetworkMessageType.FsmDataSendingStart, startMessage);

		// send every state data
		for (int i = 0; i < stateMsgs.Length; i++) {
			FsmDataMessage dataMsg = new FsmDataMessage (stateMsgs [i]);
			NetworkServer.SendToAll (FsmNetworkMessageType.SendFsmData, dataMsg);
		}

		// send end sending signal
		SimpleMessage endMessage = new SimpleMessage("Sending Done!");
		NetworkServer.SendToAll (FsmNetworkMessageType.FsmDataSendingDone, endMessage);

	}

	void Update () {
		// check is state updated
		if (lastStateName != fsmObj.ActiveStateName) {

			// state changed
			UpdateNowStateToClient ();

			lastStateName = fsmObj.ActiveStateName;
		}
	}

	void UpdateNowStateToClient () {
		int nowStateIndex = fsmDict[ fsmObj.ActiveStateName ];
		SendStateChangeToClient (fsmObj.ActiveStateName, nowStateIndex);
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
