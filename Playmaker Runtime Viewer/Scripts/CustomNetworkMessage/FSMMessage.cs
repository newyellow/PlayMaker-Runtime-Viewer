using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using HutongGames.PlayMaker;

public class FSMMessage : MessageBase  {

	public FSMStateContainer[] states;

	public FSMMessage () {
	}
}

public class FSMStateContainer {
	public string name;
	public string description;
	public Rect position;

	public string[] transitionNames;
	public int[] transitionTargets;

	public FSMStateContainer () {
	}
}