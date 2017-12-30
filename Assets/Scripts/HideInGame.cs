using UnityEngine;
using System.Collections;

public class HideInGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var Renderer = GetComponent<SpriteRenderer> ();
		Renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
