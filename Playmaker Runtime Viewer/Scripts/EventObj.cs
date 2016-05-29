using UnityEngine;
using System.Collections;

public class EventObj : MonoBehaviour {

	public GameObject textObj;
	public TextMesh _tMesh;
	public GameObject backObj;

	public TextMesh _desc;
	public GameObject descBack;

	public GameObject outlineObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A))
			StateOn ();
		else if (Input.GetKeyDown (KeyCode.B))
			StateOff ();
	}

	public void UpdateText ( string toText, string toDesc ) {
		_tMesh.text = toText;
		_desc.text = toDesc;

		UpdateBackSizeByText ();
	}

	public void UpdateBackSizeByText () {

		// scale backobj size by text size
		Bounds tBound = textObj.GetComponent<MeshRenderer>().bounds;
		Vector3 newSize = tBound.size;
		newSize.z = 0.1f;
		newSize.x += 1.0f;
		newSize.y += 0.2f;
		backObj.transform.localScale = newSize;

		// update desc back
		if (_desc.text == "") {
			descBack.transform.localScale = Vector3.zero;
		} else {
			Bounds descBound = _desc.GetComponent<MeshRenderer> ().bounds;
			Vector3 descSize = descBound.size;
			descSize.z = 0.1f;
			descSize.x += 1.0f;
			descSize.y += 0.2f;
			descBack.transform.localScale = descSize;
			descBack.transform.position = descBound.center + new Vector3 (0.0f, 0.0f, 0.5f);
		}
	}

	void StateOn () {
		Vector3 from = backObj.transform.localScale - new Vector3 (0.2f, 0.2f, 0.0f);
		Vector3 to = backObj.transform.localScale + new Vector3 (0.2f, 0.2f, 0.0f);
		StartCoroutine (OutlineSizeChange (from, to));
	}

	void StateOff () {
		Vector3 from = outlineObj.transform.localScale;
		Vector3 to = backObj.transform.localScale - new Vector3 (0.2f, 0.2f, 0.0f);
		StartCoroutine (OutlineSizeChange (from, to));
	}

	static float animateTime = 0.5f;
	IEnumerator OutlineSizeChange ( Vector3 fromSize, Vector3 toSize ) {
		int frames = (int)(animateTime * 30.0f);
		float t = 0.0f;
		float step = 1.0f / (float)frames;

		for (int i = 0; i <= frames; i++) {
			if (i != 0)
				t += step;

			outlineObj.transform.localScale = Vector3.Lerp (fromSize, toSize, t);
			yield return new WaitForFixedUpdate ();
		}
	}
}
