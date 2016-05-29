using UnityEngine;
using System.Collections;

public class SliderDrag : MonoBehaviour {

	public Vector2 movingRange = new Vector2 (-1.5f, 1.5f);
	float range = 0.0f;
	Vector3 pointPos = Vector3.zero;


	bool isDragging = false;
	Vector3 lastPos = Vector3.zero;

	public int stateIndex = -1;

	// for slider through internet
	public StateReciever _client;
	public float value = 0.0f;
	float lastValue = 0.0f;

	// Use this for initialization
	void Start () {
		range = movingRange.y - movingRange.x;
		pointPos.x = movingRange.x;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isDragging) {
			Vector3 nowPos = Camera.main.ScreenToWorldPoint ( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5.0f) );
			Vector3 diff = nowPos - lastPos;

			pointPos.x += diff.x;

			if (pointPos.x < movingRange.x)
				pointPos.x = movingRange.x;
			else if (pointPos.x > movingRange.y)
				pointPos.x = movingRange.y;

			transform.localPosition = pointPos;


			value = Mathf.InverseLerp (movingRange.x, movingRange.y, pointPos.x);

			lastPos = nowPos;
		}

		if (value != lastValue) {
			if (_client != null) {
				_client.SliderValueChange (stateIndex, value);
			}

			lastValue = value;
		}
	}

	void OnMouseDown () {
		isDragging = true;
		lastPos = Camera.main.ScreenToWorldPoint ( new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5.0f) );
	}

	void OnMouseUp () {
		isDragging = false;
	}
}
