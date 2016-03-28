﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine.Networking;


public class StateReciever : MonoBehaviour {

	public GameObject eventObj;
	public GameObject lineObj;

	GameObject[] eventObjs;
	List<GameObject> lines;
	SliderDrag[] sliders;

	// network part
	NetworkClient client;
	string serverIp = "127.0.0.1";
	int serverPort = 12345;

	int nowStateIndex = -1;

	// Use this for initialization
	void Start () {
		lines = new List<GameObject> ();

		client = new NetworkClient ();
		client.RegisterHandler (FsmNetworkMessageType.SendFSM, OnGetFSMData);
		client.RegisterHandler (FsmNetworkMessageType.PlayMakerStateChange, OnFsmStateChange);
		client.RegisterHandler (MsgType.Connect, OnConnected);
		client.RegisterHandler (MsgType.Error, OnError);
	}

	// use when switch scene
	void ClearData () {

		nowStateIndex = -1;

		// if there are data inside
		if (eventObjs != null) {
			for (int i = 0; i < eventObjs.Length; i++)
				Destroy (eventObjs [i]);
			
			System.Array.Clear (eventObjs, 0, eventObjs.Length);
		}

		// delete all lines
		if (lines.Count > 0) {
			for (int i = 0; i < lines.Count; i++)
				Destroy (lines [i].gameObject);

			lines.Clear ();
		}

		// if there are data inside
		if (sliders != null) {
			for (int i = 0; i < sliders.Length; i++)
				Destroy (sliders [i]);
			
			System.Array.Clear (sliders, 0, sliders.Length);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI () {
		if (client.isConnected) {
			GUILayout.Label ("Server Status: Connected to " + client.serverIp + ":" + client.serverPort);
		} else {
			GUILayout.Label ("Server Status: Not Connected");

			GUILayout.BeginHorizontal ();

			GUILayout.Label ("IP : ");
			serverIp = GUILayout.TextField (serverIp);

			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();

			GUILayout.Label ("Port : ");
			serverPort = System.Convert.ToInt32( GUILayout.TextField (serverPort.ToString ()) );

			GUILayout.EndHorizontal ();

			if (GUILayout.Button ("Connect"))
				client.Connect (serverIp, serverPort);
		}
	}

	public void OnError( NetworkMessage msg ) {
		
		Debug.Log ("Conn Error");

	}

	void OnGetFSMData ( NetworkMessage netMsg ) {

		// if switched from other scene
		ClearData ();

		FSMMessage fmsg = netMsg.ReadMessage<FSMMessage> ();
		FSMStateContainer[] fsms = fmsg.states;

		eventObjs = new GameObject[fsms.Length];
		sliders = new SliderDrag[fsms.Length];

		List<SliderDrag> sliderList = new List<SliderDrag> ();

		int sliderIndex = 0;

		for (int i = 0; i < fsms.Length; i++) {

			// create event obj
			eventObjs [i] = (GameObject)Instantiate (eventObj, new Vector3 (1.0f, i * -1.5f - 1.0f, 0.0f), Quaternion.identity);	
			eventObjs [i].transform.parent = transform;

			EventObj objScript = eventObjs [i].GetComponent<EventObj> ();
			objScript.UpdateText (fsms [i].name, fsms[i].description);
			objScript.UpdateBackSizeByText ();

			// positioning
			Vector3 pos = new Vector3();
			pos.x = fsms [i].position.center.x * 0.01f;
			pos.y = fsms [i].position.center.y * -0.01f;
			pos.z = 0.0f;

			eventObjs [i].transform.position = pos;
		}

		sliders = sliderList.ToArray ();

		// setting lines after objs inited
		for (int i = 0; i < fsms.Length; i++) {
			if (fsms [i].transitionNames.Length > 0) {
				for (int j = 0; j < fsms [i].transitionNames.Length; j++) {

					int targetIndex = fsms [i].transitionTargets [j];

					GameObject lineTemp = (GameObject)Instantiate (lineObj, Vector3.zero, Quaternion.identity);
					lineTemp.transform.parent = transform;
					lines.Add (lineTemp);

					LineRenderer line = lineTemp.GetComponent<LineRenderer> ();

					Vector3 lineFrom = eventObjs [i].transform.position;
					lineFrom.z = 1.0f;

					Vector3 lineTo = eventObjs [targetIndex].transform.position;
					lineTo.z = 1.0f;

					line.SetVertexCount (2);
					line.SetPosition (0, lineFrom);
					line.SetPosition (1, lineTo);

					Debug.Log ("Line Size:" + line.bounds.size);
					line.material.mainTextureScale = new Vector2 (Vector3.Distance (lineFrom, lineTo) / 0.2f, 1.0f);

				} 
			}
			else
			{
					Debug.Log ("No Transition");
			}
		}
	}
		
	public void SliderValueChange( int sliderIndex, float value ) {
		SliderMessage msg = new SliderMessage ();
		msg.sliderIndex = sliderIndex;
		msg.value = value;

		client.Send (FsmNetworkMessageType.SliderValueChange, msg);
	}

	void OnFsmStateChange ( NetworkMessage netMsg ) {
		FSMStateMessage msg = netMsg.ReadMessage<FSMStateMessage> ();

		if (nowStateIndex != -1)
			eventObjs [nowStateIndex].SendMessage ("StateOff");
		
		eventObjs [msg.nowStateIndex].SendMessage ("StateOn");
		nowStateIndex = msg.nowStateIndex;
	}

	void OnConnected ( NetworkMessage netMsg ) {
		SimpleMessage msg = new SimpleMessage ("give me states!");
		client.Send (FsmNetworkMessageType.RequestFSM, msg);
	}

	void OnApplicationQuit () {
		Debug.Log ("client shutdown");
		client.Shutdown ();
	}
}
