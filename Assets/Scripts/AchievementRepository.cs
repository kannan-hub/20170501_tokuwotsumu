using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AchievementRepository :MonoBehaviour
{

	[SerializeField]
	public class Achievement
	{
		public int recentScore;
		public int highScore;
		public string highScoreName;
		public int totalScore;
		public int totalTime;
		public List<int> achievedIdList;

		public Achievement()
		{
			recentScore = 0;
			highScore = 0;
			highScoreName = "";
			totalScore += recentScore;
			totalTime = 0;
			achievedIdList = new List<int> ();
		}
	}

	public void SaveAchievement( int score, string scoreName, List<int> tokuList )
	{
		Achievement saveAchievement = LoadAchievement ();

		saveAchievement.recentScore = score;
		saveAchievement.totalScore += score;

		if ( score > saveAchievement.highScore )
		{
			saveAchievement.highScore = score;
			saveAchievement.highScoreName = scoreName;
		}

		foreach(int id in tokuList.Where(index => !saveAchievement.achievedIdList.Contains(index)))
		{
			saveAchievement.achievedIdList.Add (id);
		}

		SaveData.Remove ("p1");
		SaveData.SetClass<Achievement> ("p1", saveAchievement);
		SaveData.Save ();
	}

	public Achievement LoadAchievement()
	{
		return SaveData.GetClass<Achievement> ("p1", new Achievement ());
	}

	public void ClearAchievement()
	{
		SaveData.Remove ("p1");
		SaveData.SetClass<Achievement> ("p1", new Achievement());
		SaveData.Save ();
	}

}
