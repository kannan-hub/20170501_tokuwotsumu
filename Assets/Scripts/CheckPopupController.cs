using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPopupController : MonoBehaviour {

	public void CheckInput(bool logic )
	{
		if ( logic ) ClearData ();

		Destroy (transform.root.gameObject);
	}

	void ClearData()
	{
		AchievementRepository arepos = FindObjectOfType<AchievementRepository> ();
		arepos.ClearAchievement ();
	}

}
