using UnityEngine;
using System.Collections;
using HutongGames.PlayMaker;

public class StateData : MonoBehaviour {

	public TextMesh _textMesh;
	public GameObject connectLines;
	LineRenderer[] lines;

	public FsmState _state
	{
		set
		{
			FsmState tempState = value;

			// set position
			Vector3 pos = new Vector3( 0.0f, 0.0f, 0.5f );
			pos.x = tempState.Position.center.x * 0.01f;
			pos.y = tempState.Position.center.y * -0.01f;
			transform.position = pos;

		}
		get
		{
			return _state;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
