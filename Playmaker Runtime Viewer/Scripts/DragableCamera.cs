using UnityEngine;
using System.Collections;

public class DragableCamera : MonoBehaviour {

	public Camera _cam;
	bool isDragging = false;

	Vector3 lastPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown (2)) {
			isDragging = true;
			lastPos = GetMousePosInWorld ();
		}

		if (Input.GetMouseButtonUp (2))
			isDragging = false;


		if (isDragging) {
			Vector3 nowPos = GetMousePosInWorld ();
			Vector3 diff = nowPos - lastPos;

			transform.position -= diff;

			// because of camera position change, need to calculate once more
			lastPos = GetMousePosInWorld();
		}
	}

	Vector3 GetMousePosInWorld () {
		return _cam.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 5.0f));
	}
}
