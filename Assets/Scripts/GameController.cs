using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController :MonoBehaviour
{

	public bool gameover = false;
	public int score = 0;
	public float gameoverTime = 300f;
	public float remainingTime;
	public string kaimyoResult = "";

	public List<int> aquiredTokuAchievementIdList;

	[SerializeField]
	Text remainingTimeText;
	[SerializeField]
	Text scoreText;

	[SerializeField]
	GameObject gameoverObj;
	[SerializeField]
	Text resultText;
	[SerializeField]
	Text youText;
	[SerializeField]
	Text kaimyoText;

	[SerializeField] //メインのゲームシーンの名前を設定する
	string sceneName = "main";
	[SerializeField]
	List<string> kaimyo;

	AchievementRepository.Achievement achievementHistory;

	readonly int ONE_GAME_DONE_ACHIEVEMENT_ID = 1;
	readonly int NO_TOKU_SCORE_ACHIEVEMENT_ID = 4;
	readonly int COLLECT_ALL_RARE_TOKU_ACHIEVEMENT_ID = 18;
	readonly int COLLECT_ALL_SRARE_TOKU_ACHIEVEMENT_ID = 19;
	readonly Dictionary<int, int> TOKU_SCORE_ACHIEVEMENT_ID = new Dictionary<int, int> ()
	{
		{2,50 }, {3,100 }	//{ achievementId, needScore}
	};
	readonly Dictionary<int, int> TOKU_TOTAL_SCORE_ACHIEVEMENT_ID = new Dictionary<int, int> ()
	{
		{6,100 }, {7,1000 }, {8,10000 }, {9,100000 }	//{ achievementId, needTotalScore}
	};
	readonly List<int> RARE_TOKU_ACHIEVEMENT_ID = new List<int> () { 10,11,12,13,14,15,16,17};
	readonly List<int> SRARE_TOKU_ACHIEVEMENT_ID = new List<int> () { 19,20,21,22,23,24 };

	void Awake()
	{
		if ( SceneManager.GetActiveScene ().name == sceneName )
		{
			gameoverObj.SetActive (false);
			achievementHistory = GetComponent<AchievementRepository> ().LoadAchievement ();
			aquiredTokuAchievementIdList = achievementHistory.achievedIdList;

			remainingTime = gameoverTime;
			StartCoroutine (GameControl());
			StartCoroutine (AchievementMonitor());
		}
	}

	IEnumerator GameControl()
	{
		while ( !gameover )
		{
			remainingTime -= Time.deltaTime;
			remainingTimeText.text = remainingTime.ToString ("0.0") + " sec";
			scoreText.text = score.ToString () + " 徳";

			if ( remainingTime <= 0 )
			{
				gameover = true;
			}
			yield return null;
		}
		GameOver ();
	}

	IEnumerator AchievementMonitor()
	{
		int temp = aquiredTokuAchievementIdList.Count;
		while ( !gameover )
		{
			int count = aquiredTokuAchievementIdList.Count;
			if(count > temp )
			{
				//popup achievement method
				temp++;
			}
			yield return null;
		}

		AddAquiredAchievement ();
		SaveGame ();
	}

	void AddAquiredAchievement()
	{
		CheckDoneGameAchievement ();
		CheckScoreAchievement ();
		CheckTotalScoreAchievement ();
		CheckCollectAllRareTokuAchievement ();
		CheckCollectAllSRareTokuAchievement ();
	}

	void CheckDoneGameAchievement()
	{
		if ( aquiredTokuAchievementIdList.Contains (ONE_GAME_DONE_ACHIEVEMENT_ID) ) return;
		aquiredTokuAchievementIdList.Add (ONE_GAME_DONE_ACHIEVEMENT_ID);
	}

	void CheckScoreAchievement()
	{
		if(!aquiredTokuAchievementIdList.Contains(NO_TOKU_SCORE_ACHIEVEMENT_ID) && score == 0 )
		{
			aquiredTokuAchievementIdList.Add (NO_TOKU_SCORE_ACHIEVEMENT_ID);
			return;
		}

		foreach ( KeyValuePair<int, int> element in TOKU_SCORE_ACHIEVEMENT_ID )
		{
			if ( aquiredTokuAchievementIdList.Contains (element.Key) ) continue;

			if ( score >= element.Value ) aquiredTokuAchievementIdList.Add (element.Key);
		}

	}

	void CheckTotalScoreAchievement()
	{
		int total = score + achievementHistory.totalScore;
		foreach ( KeyValuePair<int, int> element in TOKU_TOTAL_SCORE_ACHIEVEMENT_ID )
		{
			if ( aquiredTokuAchievementIdList.Contains (element.Key) ) continue;

			if ( total >= element.Value ) aquiredTokuAchievementIdList.Add (element.Key);
		}

	}

	void CheckCollectAllRareTokuAchievement()
	{
		if ( aquiredTokuAchievementIdList.Contains (COLLECT_ALL_RARE_TOKU_ACHIEVEMENT_ID) ) return;

		if ( RARE_TOKU_ACHIEVEMENT_ID.TrueForAll (delegate ( int id ) { return aquiredTokuAchievementIdList.Contains (id); }) )
		{
			aquiredTokuAchievementIdList.Add (COLLECT_ALL_RARE_TOKU_ACHIEVEMENT_ID);
		}
	}

	void CheckCollectAllSRareTokuAchievement()
	{
		if ( aquiredTokuAchievementIdList.Contains (COLLECT_ALL_SRARE_TOKU_ACHIEVEMENT_ID) ) return;

		if ( SRARE_TOKU_ACHIEVEMENT_ID.TrueForAll (delegate ( int id ) { return aquiredTokuAchievementIdList.Contains (id); }) )
		{
			aquiredTokuAchievementIdList.Add (COLLECT_ALL_SRARE_TOKU_ACHIEVEMENT_ID);
		}
	}

	void GameOver()
	{
		resultText.text = score + " の徳を積んだ。";
		int index = 0; //TODO スコアによって、変更する。
		kaimyoResult = kaimyo [index];
		kaimyoText.text = kaimyoResult;

		scoreText.enabled = false;
		remainingTimeText.enabled = false;

		gameoverObj.SetActive (true);
	}

	void SaveGame()
	{
		AchievementRepository arepos = GetComponent<AchievementRepository> ();
		arepos.SaveAchievement (score, kaimyoResult, aquiredTokuAchievementIdList);
	}

	public  void RedirectScenes(string name )
	{
		SceneManager.LoadScene (name);
	}

	public void Tweet()
	{
		GetComponent<Tweet> ().TweetResult (score, kaimyoResult);
	}

	public void PopupCheckInput(GameObject obj)
	{
		GameObject instance = Instantiate (obj, transform.position, Quaternion.identity);
		instance.SetActive (true);
	}
}
