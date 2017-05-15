using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementButtonModel : MonoBehaviour {

	[SerializeField]
	int id;
	[SerializeField]
	public bool achieved;

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
}
