using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementButtonModel : MonoBehaviour {

	[SerializeField]
	int id;
	[SerializeField]
	public bool achieved;
	[SerializeField]
	public bool secret;

	public int GetId()
	{
		return id;
	}
	public bool IsAchieved()
	{
		return achieved;
	}
	public void SetAchieved(bool logic )
	{
		achieved = logic;
	}

	public bool IsSecret()
	{
		return secret;
	}
}
