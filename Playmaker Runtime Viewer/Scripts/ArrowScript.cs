using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowScript : MonoBehaviour {
	
	public Transform[] testPath;
	Vector3[] arrowPath;

	List<Vector3> drawPath;
	public AnimationCurve turnCurve;

	// Use this for initialization
	void Start () {
		drawPath = new List<Vector3> ();
		arrowPath = new Vector3[testPath.Length];

		for (int i = 0; i < arrowPath.Length; i++)
			arrowPath [i] = testPath [i].position;

		CalculateDrawPath ();
		CreateLineRenderer ();
	}

	void CalculateDrawPath () {
		for (int i = 0; i < arrowPath.Length; i++) {
			if (i == 0)
				drawPath.Add (arrowPath [i]);
			else if (i == arrowPath.Length - 1) {
				drawPath.Add (arrowPath [i]);

				Vector3 endDir = arrowPath [i] - arrowPath [i - 1];
				Vector3 pos = arrowPath[i] - Quaternion.AngleAxis (30.0f, new Vector3 (0.0f, 0.0f, 1.0f)) * endDir * 0.3f;
				drawPath.Add (pos);
			}
			else {

				drawPath.Add (arrowPath [i]);

//				Vector3 dir1 = arrowPath [i] - arrowPath [i - 1];
//				Vector3 dir2 = arrowPath [i + 1] - arrowPath [i];
//
//				Vector3 fromPoint = arrowPath [i] - dir1 * 0.3f;
//				Vector3 toPoint = arrowPath [i] + dir2 * 0.3f;
//
//				for (int t = 0; t <= 20; t++) {
//					float value = (float)t / 10.0f;
//					Vector3 movePoint = Vector3.Lerp (fromPoint, toPoint, value);
//					Vector3 curvedPoint = Vector3.Lerp (movePoint, arrowPath [i], turnCurve.Evaluate (value));
//
//					drawPath.Add (curvedPoint);
//				}

			}
		}
	}

	void CreateLineRenderer () {
		LineRenderer line = gameObject.AddComponent<LineRenderer> ();
		line.SetVertexCount (drawPath.Count);
		line.SetWidth (0.05f, 0.05f);
		for (int i = 0; i < drawPath.Count; i++) {
			line.SetPosition (i, drawPath [i]);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
