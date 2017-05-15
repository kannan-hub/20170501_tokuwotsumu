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

	public List<TokuModel> getTokuList;

	[SerializeField]
	Text remainingTimeText;
	[SerializeField]
	Text scoreText;

	[SerializeField]
	GameObject gameoverObj;
	[SerializeField]
	Text gameoverText;
	[SerializeField]
	Text resultText;
	[SerializeField]
	Text youText;
	[SerializeField]
	Text kaimyoText;

	[SerializeField] //メインのゲームシーンの名前を設定する
	string sceneName = "main";
	[SerializeField]
	string [] kaimyo;

	void Awake()
	{
		if ( SceneManager.GetActiveScene ().name == sceneName )
		{
			gameoverObj.SetActive (false);
			getTokuList = new List<TokuModel> ();

			remainingTime = gameoverTime;
			StartCoroutine ("GameControl");
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
		yield return null;
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

		SaveGame ();
	}

	void SaveGame()
	{
		AchievementRepository arepos = GetComponent<AchievementRepository> ();
		arepos.SaveAchievement (score, kaimyoResult, getTokuList);
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
