using UnityEngine;
using System.Collections;

public class LerpToPosition : MonoBehaviour {

	public float moveTime = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator MoveToPosition ( Vector3 toPosition ) {
		int frames = (int)(moveTime * 30.0f);
		float t = 0.0f;
		float step = 1.0f / (float)(frames);

		Vector3 fromPos = transform.position;
		Vector3 toPos = toPosition;
		toPos.z = fromPos.z;

		for (int i = 0; i <= frames; i++) {
			if (i != 0)
				t += step;

			transform.position = Vector3.Lerp (fromPos, toPos, t);
			yield return new WaitForFixedUpdate ();
		}
	}
}
