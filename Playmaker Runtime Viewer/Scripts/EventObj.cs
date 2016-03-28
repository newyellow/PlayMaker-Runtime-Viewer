using UnityEngine;
using System.Collections;

public class EventObj : MonoBehaviour {

	public GameObject textObj;
	public TextMesh _tMesh;
	public GameObject backObj;

	public TextMesh _desc;
	public GameObject descBack;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
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
		Bounds descBound = _desc.GetComponent<MeshRenderer>().bounds;
		Vector3 descSize = descBound.size;
		descSize.z = 0.1f;
		descSize.x += 1.0f;
		descSize.y += 0.2f;
		descBack.transform.localScale = descSize;
		descBack.transform.position = descBound.center + new Vector3 (0.0f, 0.0f, 0.5f);;
	}
}
