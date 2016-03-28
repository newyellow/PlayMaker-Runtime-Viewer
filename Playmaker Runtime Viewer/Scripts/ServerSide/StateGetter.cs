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

	VJObject[] vjEffects;

	// Use this for initialization
	void Start () {
		CollectPlaymakerInfos ();
		NetworkServer.RegisterHandler (FsmNetworkMessageType.RequestFSM, GetStateRequest);
		NetworkServer.RegisterHandler (FsmNetworkMessageType.SliderValueChange, OnSliderValueChange);
		NetworkServer.Listen (12345);
	}
		
	void CollectPlaymakerInfos () {
		FsmState[] _states = fsmObj.FsmStates;
		int num = _states.Length;

		stateMsgs = new FSMStateContainer[num];

		fsmDict = new Dictionary<string, int> ();

		// for vj slider
		List<VJObject> vjList = new List<VJObject>();


		// add states into dictionaries
		for (int i = 0; i < num; i++)
			fsmDict.Add (_states [i].Name, i);

		for (int i = 0; i < num; i++) {
			stateMsgs [i] = new FSMStateContainer ();

			stateMsgs [i].name = _states [i].Name;
			stateMsgs [i].description = _states [i].Description;
			stateMsgs [i].position = _states [i].Position;

			// check is slider
			if (_states [i].Actions.Length > 0) {
				for (int j = 0; j < _states [i].Actions.Length; j++) {
					if (_states [i].Actions [j].Name == "SliderAction") {
						stateMsgs [i].hasSlider = true;

						SliderAction sliderAction = (SliderAction)_states [i].Actions [j];
						vjList.Add (sliderAction.vjSlider);
					}
				}
			}

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

		vjEffects = vjList.ToArray ();
	}

	void GetStateRequest ( NetworkMessage netMsg ) {
		int connId = netMsg.conn.connectionId;

		FSMMessage sendMsg = new FSMMessage ();
		sendMsg.states = (FSMStateContainer[])stateMsgs.Clone ();

		NetworkServer.SendToClient (connId, FsmNetworkMessageType.SendFSM, sendMsg);
	}

	void OnSliderValueChange( NetworkMessage netMsg ) {
		SliderMessage sliderMsg = netMsg.ReadMessage<SliderMessage> ();

		int index = sliderMsg.sliderIndex;
		float value = sliderMsg.value;

		vjEffects [index].SetValue (value);
		Debug.Log ("Get slider [" + index + "]  on value [ " + value + "] ");
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			NetworkServer.Shutdown ();
		}
	}
}
