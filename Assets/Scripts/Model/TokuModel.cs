using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokuModel : MonoBehaviour {

	[SerializeField]
	int achievementId = 0;
	[SerializeField]
	string myName = "";
	[SerializeField]
	string rarity = "normal";
	[SerializeField]
	bool acquired = false;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			gameObject.tag = "Ground";
			//gameObject.layer = 0;

			StartCoroutine(CheckSpeed());
		}
	}

	IEnumerator CheckSpeed()
	{
		Rigidbody2D rb = GetComponent<Rigidbody2D>();
		
		yield return new WaitWhile(() => rb.velocity.magnitude > 0.05f);
		yield return new WaitForSeconds(3.0f);
		
		rb.isKinematic = true;
		rb.velocity = Vector2.zero;
		rb.angularVelocity = 0f;
	}

	public int GetId()
	{
		return achievementId;
	}
	public string GetName()
	{
		return myName;
	}
	public string GetRarity()
	{
		return rarity;
	}
	public bool IsAcquired()
	{
		return acquired;
	}
	public void SetAcquired(bool logic )
	{
		acquired = logic;
	}

}
