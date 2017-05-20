using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPopup :MonoBehaviour
{

	readonly int hashOpen = Animator.StringToHash ("Open");
	readonly int hashHide = Animator.StringToHash ("Hide");

	public GameObject achievementPopup;
	public Animator anim;

	//public void PopupControll (GameObject obj) {
	private void Start()
	{
		GameObject obj = this.gameObject;
		achievementPopup = obj;
		anim = obj.GetComponent<Animator> ();

		anim.Play ("Open");
		//StartCoroutine("PopupFlow")
	}

	IEnumerator PopupFlow()
	{
		anim.Play (hashOpen);
		yield return null;
		yield return new WaitForAnimation (anim, 0);
	}
}
