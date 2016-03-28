using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class testClient : MonoBehaviour {

	public KeyCode startServerkey;
	public KeyCode sendMsgKey;

	public string testMsg = "";

	NetworkClient client;

	// Use this for initialization
	void Start () {

		client = new NetworkClient ();
		client.RegisterHandler (FsmNetworkMessageType.ServerToClientCommand, OnServerSendCommand);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (startServerkey)) {

			if (!client.isConnected) {
				client.Connect ("127.0.0.1", 12345);
			}
		}

		if (Input.GetKeyDown (sendMsgKey)) {
			SimpleMessage message = new SimpleMessage (testMsg);
			client.Send (FsmNetworkMessageType.ClientToServerCommand, message );
		}
	}

	void OnServerSendCommand( NetworkMessage networkMsg ) {
		SimpleMessage msg = networkMsg.ReadMessage<SimpleMessage> ();
		Debug.Log (msg._message);
	}
}
