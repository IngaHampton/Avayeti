using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPointsScript : MonoBehaviour {

	private GUIText ScoreText;
	private string Format;
	void OnTriggerEnter2D (Collider2D Player)
	{
		if (Player.gameObject.tag == "Player") 
		{
			int PreviousScore = int.Parse (ScoreText.text);
			int NextScore = PreviousScore + 1;
			ScoreText.text = NextScore.ToString(Format);
			Destroy (gameObject);
		}
	}

	void Start () {

		Format = "000";
		ScoreText = FindObjectOfType<GUIText> ();
		ScoreText.text = 0.ToString (Format);
		
	}
}
