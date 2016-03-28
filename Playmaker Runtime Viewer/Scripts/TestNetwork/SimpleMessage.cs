using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SimpleMessage : MessageBase {

	public string _message;

	public SimpleMessage() {
		_message = "";
	}

	public SimpleMessage( string message ) {
		_message = message;
	}
}
