using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine.Networking;


public class StateReciever : MonoBehaviour {

	public GameObject eventObj;
	public GameObject lineObj;


	GameObject[] eventObjs;
	List<LineRenderer> lines;
	SliderDrag[] sliders;

	NetworkClient client;

	// Use this for initialization
	void Start () {
		client = new NetworkClient ();
		client.RegisterHandler (FsmNetworkMessageType.SendFSM, OnGetFSMData);
		client.RegisterHandler (MsgType.Error, OnError);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			client.Connect ("127.0.0.1", 12345);
		}

		if (Input.GetKeyDown (KeyCode.W)) {
			SimpleMessage msg = new SimpleMessage ("give me states!");
			client.Send (FsmNetworkMessageType.RequestFSM, msg);
		}
	}

	void OnGUI () {
		if (client.isConnected)
			GUILayout.Label ("Connected to " + client.connection.address + " with ID: " + client.connection.connectionId);
		else
			GUILayout.Label ("No Connect");
	}

	public void OnError( NetworkMessage msg ) {
		
		Debug.Log ("Conn Error");
	}

	void OnGetFSMData ( NetworkMessage netMsg ) {
		FSMMessage fmsg = netMsg.ReadMessage<FSMMessage> ();
		FSMStateContainer[] fsms = fmsg.states;

		eventObjs = new GameObject[fsms.Length];
		lines = new List<LineRenderer> ();
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
}
