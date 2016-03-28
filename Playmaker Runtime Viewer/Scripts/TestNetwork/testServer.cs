using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class testServer : MonoBehaviour {

	// Use this for initialization
	void Start () {

		NetworkServer.Listen (12345);
		NetworkServer.RegisterHandler (FsmNetworkMessageType.ClientToServerCommand, GetCommand);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space)) {
			SimpleMessage msg = new SimpleMessage ("HAHA YOU GOT ME");
			NetworkServer.SendToAll( FsmNetworkMessageType.ServerToClientCommand, msg );
		}

	}

	void OnGUI () {
		GUILayout.Label ("Connections: " + NetworkServer.connections.Count);
	}

	void GetCommand( NetworkMessage msg ) {
		Debug.Log (msg.conn.connectionId);
		SimpleMessage smsg = msg.ReadMessage<SimpleMessage> ();

		Debug.Log (smsg._message);
	}


}
