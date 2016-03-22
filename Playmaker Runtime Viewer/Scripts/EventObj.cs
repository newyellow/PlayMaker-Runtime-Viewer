using UnityEngine;
using System.Collections;

public class EventObj : MonoBehaviour {

	public GameObject textObj;
	public TextMesh _tMesh;

	public GameObject backObj;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateText ( string toText ) {
		_tMesh.text = toText;
	}

	public void UpdateBackSizeByText () {
		// scale backobj size by text size
		Bounds tBound = textObj.GetComponent<MeshRenderer>().bounds;
		Vector3 newSize = tBound.size;
		newSize.z = 0.1f;
		newSize.x += 1.0f;
		newSize.y += 0.2f;
		backObj.transform.localScale = newSize;
	}
}
