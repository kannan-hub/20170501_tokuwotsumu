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
