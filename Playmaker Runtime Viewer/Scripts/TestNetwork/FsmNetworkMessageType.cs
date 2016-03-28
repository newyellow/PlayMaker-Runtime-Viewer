using UnityEngine;
using UnityEngine.Networking;

public class FsmNetworkMessageType {

	public static short RequestFSM = MsgType.Highest+1;
	public static short SendFSM = MsgType.Highest+2;
	public static short ClientToServerCommand = MsgType.Highest+3;
	public static short ServerToClientCommand = MsgType.Highest+4;
	public static short SliderValueChange = MsgType.Highest+5;
	public static short PlayMakerStateChange = MsgType.Highest+6;
}
