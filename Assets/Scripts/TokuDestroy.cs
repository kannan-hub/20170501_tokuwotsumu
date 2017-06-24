using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokuDestroy : MonoBehaviour {

	[SerializeField]
	GameController gameCol;

	void OnTriggerEnter2D( Collider2D other )
	{
		if ( other.CompareTag ("Toku") || other.CompareTag("Ground") )
		{
			Destroy (other.gameObject);
			other = null;
			gameCol.score--;
		}
	}
}
