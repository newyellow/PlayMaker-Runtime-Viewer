using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class testGetState : MonoBehaviour {

	public PlayMakerFSM fsmObj;
	Dictionary<string,int> fsmDict;
	public GameObject eventObj;
	public GameObject lineObj;

	GameObject[] eventObjs;
	List<LineRenderer> lines;

	// Use this for initialization
	void Start () {
		lines = new List<LineRenderer> ();
		fsmDict = new Dictionary<string,int> ();
		FsmState[] _states = fsmObj.FsmStates;
		eventObjs = new GameObject [_states.Length];

		// put all states in dictionary
		for (int i = 0; i < _states.Length; i++)
			fsmDict.Add (_states [i].Name, i);
		
		for (int i = 0; i < _states.Length; i++) {
			Debug.Log (_states [i].Name);
			Debug.Log (_states [i].Description);

			// create objs
			eventObjs [i] = (GameObject)Instantiate( eventObj, new Vector3( 1.0f, i * -1.5f - 1.0f, 0.0f ), Quaternion.identity );
			eventObjs [i].transform.parent = transform;
			EventObj objScript = eventObjs [i].GetComponent<EventObj> ();
			objScript.UpdateText (_states [i].Name);
			objScript.UpdateBackSizeByText ();

			// positioning
			Vector3 pos = new Vector3();
			pos.x = _states [i].Position.center.x * 0.01f;
			pos.y = _states [i].Position.center.y * -0.01f;
			pos.z = 0.0f;

			eventObjs [i].transform.position = pos;

			FsmTransition[] transitions = _states [i].Transitions;

			if (transitions.Length > 0) {
				
				GameObject lineTemp = (GameObject)Instantiate (lineObj, Vector3.zero, Quaternion.identity);
				LineRenderer line = lineTemp.GetComponent<LineRenderer> ();

				int toIndex = fsmDict[ transitions [0].ToState ];

				Vector3 lineFrom = new Vector3 (0.0f, 0.0f, 1.0f);
				lineFrom.x = _states [i].Position.center.x * 0.01f;
				lineFrom.y = _states [i].Position.center.y * -0.01f;

				Vector3 lineTo = new Vector3 (0.0f, 0.0f, 1.0f);
				lineTo.x = _states [toIndex].Position.center.x * 0.01f;
				lineTo.y = _states [toIndex].Position.center.y * -0.01f;

				line.SetVertexCount (2);
				line.SetPosition (0, lineFrom);
				line.SetPosition (1, lineTo);

				Debug.Log ("Line Size:" + line.bounds.size);
				line.material.mainTextureScale = new Vector2 (line.bounds.size.x / 0.3f, 1.0f);

			} else {
				Debug.Log ("No Transition");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
