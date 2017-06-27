using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
	/// <summary>
	/// 男:0, 女:1
	/// </summary>
	[Range(0, 1)] public int sex = 0;

	public int score = 0;
	public string kaimyoResult = "-";

	public List<int> aquiredTokuAchievementIdList;

	readonly int ONE_GAME_DONE_ACHIEVEMENT_ID = 1;
	readonly int NO_TOKU_SCORE_ACHIEVEMENT_ID = 4;
	readonly int COLLECT_ALL_RARE_TOKU_ACHIEVEMENT_ID = 18;
	readonly int COLLECT_ALL_SRARE_TOKU_ACHIEVEMENT_ID = 25;

	/// <summary>
	/// 1ゲーム実績：Idと1ゲームの獲得徳数
	/// </summary>
	readonly Dictionary<int, int> TOKU_SCORE_ACHIEVEMENT_ID = new Dictionary<int, int>
	{
		{2, 50},
		{3, 100} //{ achievementId, needScore}
	};

	/// <summary>
	/// 累計実績：Idと達成するのに必要な累計徳数
	/// </summary>
	readonly Dictionary<int, int> TOKU_TOTAL_SCORE_ACHIEVEMENT_ID = new Dictionary<int, int>
	{
		{6, 100},
		{7, 1000},
		{8, 10000},
		{9, 100000}
	};

	readonly List<int> RARE_TOKU_ACHIEVEMENT_ID = new List<int> {10, 11, 12, 13, 14, 15, 16, 17};
	readonly List<int> SRARE_TOKU_ACHIEVEMENT_ID = new List<int> {19, 20, 21, 22, 23, 24};

	AchievementRepository.Achievement achievementHistory;

	void Awake()
	{
		if (this != Instance)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

//		achievementHistory = GetComponent<AchievementRepository>().LoadAchievement();
//		aquiredTokuAchievementIdList = achievementHistory.achievedIdList;
	}

	public string GetKaimyoFromScore()
	{
		List<int> list = new List<int> {0, 50, 100, 150};
		Dictionary<int, List<string>> KAIMYO_DIC = new Dictionary<int, List<string>>()
		{
			{0, new List<string> {"積不得意", "初々徳", "積雑魚", "糞電子遊戯二度不遊", "n00b"}},
			{50, new List<string> {"積得意", "積慣", "Beginner"}},
			{100, new List<string> {"免許皆伝", "積玄人", "皆恐積人", "Expert"}},
			{150, new List<string> {"積神", "暇人", "Awesome"}}
		};

		Dictionary<int, List<string>> KAIMYO_SUFFIX_DIC = new Dictionary<int, List<string>>()
		{
			{0, new List<string> {"信士", "信女"}},
			{50, new List<string> {"禅定門", "禅定尼"}},
			{100, new List<string> {"居士", "大姉"}},
			{150, new List<string> {"大居士", "清大姉"}}
		};

		var last = list.Last(value => value <= score);
		var index = Random.Range(0, KAIMYO_DIC[last].Count);

		return KAIMYO_DIC[last][index] + KAIMYO_SUFFIX_DIC[last][sex];
	}

//	public void AddAquiredAchievement()
//	{
//		CheckDoneGameAchievement();
//		CheckScoreAchievement();
//		CheckTotalScoreAchievement();
//		CheckCollectAllRareTokuAchievement();
//		CheckCollectAllSRareTokuAchievement();
//	}

	void CheckDoneGameAchievement()
	{
		if (aquiredTokuAchievementIdList.Contains(ONE_GAME_DONE_ACHIEVEMENT_ID)) return;
		aquiredTokuAchievementIdList.Add(ONE_GAME_DONE_ACHIEVEMENT_ID);
	}

	void CheckScoreAchievement()
	{
		if (!aquiredTokuAchievementIdList.Contains(NO_TOKU_SCORE_ACHIEVEMENT_ID) && score == 0)
		{
			aquiredTokuAchievementIdList.Add(NO_TOKU_SCORE_ACHIEVEMENT_ID);
			return;
		}

		var notYetAchieveIdList = TOKU_SCORE_ACHIEVEMENT_ID
			.Where(id => !aquiredTokuAchievementIdList.Contains(id.Key))
			.Where(id => score > id.Value);

		foreach (var notYetAquireAchievement in notYetAchieveIdList)
		{
			aquiredTokuAchievementIdList.Add(notYetAquireAchievement.Key);
		}
	}

	void CheckTotalScoreAchievement()
	{
		int total = score + achievementHistory.totalScore;

		var notYetAchieveIdList = TOKU_TOTAL_SCORE_ACHIEVEMENT_ID
			.Where(id => !aquiredTokuAchievementIdList.Contains(id.Key))
			.Where(id => total > id.Value);

		foreach (var notYetAquireAchievement in notYetAchieveIdList)
		{
			aquiredTokuAchievementIdList.Add(notYetAquireAchievement.Key);
		}
	}

	void CheckCollectAllRareTokuAchievement()
	{
		if (aquiredTokuAchievementIdList.Contains(COLLECT_ALL_RARE_TOKU_ACHIEVEMENT_ID)) return;

		var aquiredRareIdCount = RARE_TOKU_ACHIEVEMENT_ID.Count(id => aquiredTokuAchievementIdList.Contains(id));
		
		if(aquiredRareIdCount != RARE_TOKU_ACHIEVEMENT_ID.Count) return;
		
		aquiredTokuAchievementIdList.Add(COLLECT_ALL_RARE_TOKU_ACHIEVEMENT_ID);
	}

	void CheckCollectAllSRareTokuAchievement()
	{
		if (aquiredTokuAchievementIdList.Contains(COLLECT_ALL_SRARE_TOKU_ACHIEVEMENT_ID)) return;

		var aquiredRareIdCount = SRARE_TOKU_ACHIEVEMENT_ID.Count(id => aquiredTokuAchievementIdList.Contains(id));
		
		if(aquiredRareIdCount != SRARE_TOKU_ACHIEVEMENT_ID.Count) return;
		
		aquiredTokuAchievementIdList.Add(COLLECT_ALL_SRARE_TOKU_ACHIEVEMENT_ID);
	}

//	public void SaveGame()
//	{
//		AchievementRepository arepos = GetComponent<AchievementRepository>();
//		arepos.SaveAchievement(score, kaimyoResult, aquiredTokuAchievementIdList);
//	}

	public void PopupCheckInput(GameObject obj)
	{
		GameObject instance = Instantiate(obj, transform.position, Quaternion.identity);
		instance.SetActive(true);
	}

	public void Tweet()
	{
		GetComponent<Tweet>().TweetResult(score, kaimyoResult);
	}
}