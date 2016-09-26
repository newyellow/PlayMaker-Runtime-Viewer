using UnityEngine;
using System.Collections;

public class FSMStateContainer {
	public int stateId;
	public string name;
	public string description;
	public Rect position;

	public string[] transitionNames;
	public int[] transitionTargets;

	public FSMStateContainer () {
	}
}
