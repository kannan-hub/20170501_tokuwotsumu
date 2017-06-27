using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public bool gameover = false;
	public float gameoverTime = 30f;
	public float remainingTime;

	[SerializeField] GameManager gameManager;
	[SerializeField] Text remainingTimeText;
	[SerializeField] Text scoreText;
	[SerializeField] GameObject gameoverObj;
	[SerializeField] Text resultText;
	[SerializeField] Text youText;
	[SerializeField] Text kaimyoText;
	[SerializeField] string sceneName = "main";
	[SerializeField] Toggle maleToggle;
	[SerializeField] Toggle femaleToggle;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		if (SceneManager.GetActiveScene().name == sceneName)
		{
			gameManager.score = 0;
			gameover = false;

			gameoverObj.SetActive(false);
			remainingTime = gameoverTime;
			StartCoroutine(GameControl());
//			StartCoroutine(AchievementMonitor());
		}

		if (SceneManager.GetActiveScene().name == "start")
		{
			maleToggle.isOn = (gameManager.sex == 0);
			femaleToggle.isOn = (gameManager.sex == 1);
		}
	}

	IEnumerator GameControl()
	{
		while (!gameover)
		{
			remainingTime -= Time.deltaTime;
			remainingTimeText.text = remainingTime.ToString("0.0") + " sec";
			scoreText.text = gameManager.score + " 徳";

			if (remainingTime <= 0)
			{
				gameover = true;
			}
			yield return null;
		}
		GameOver();
	}

	void GameOver()
	{
		resultText.text = gameManager.score + " の徳を積んだ。";
		gameManager.kaimyoResult = gameManager.GetKaimyoFromScore();
		kaimyoText.text = gameManager.kaimyoResult;

		scoreText.enabled = false;
		remainingTimeText.enabled = false;

		gameoverObj.SetActive(true);

//		gameManager.AddAquiredAchievement();
//		gameManager.SaveGame();
	}

//	IEnumerator AchievementMonitor()
//	{
//		int temp = gameManager.aquiredTokuAchievementIdList.Count;
//		while (!gameover)
//		{
//			int count = gameManager.aquiredTokuAchievementIdList.Count;
//			if (count > temp)
//			{
//				//popup achievement method
//				temp++;
//			}
//			yield return null;
//		}
//
//		gameManager.AddAquiredAchievement();
//		gameManager.SaveGame();
//	}
	
	public void RedirectScenes(string name)
	{
		SceneManager.LoadScene(name);
	}
	
	public void Tweet()
	{
		gameManager.Tweet();
	}

	public void SetPlaySex(int num)
	{
		gameManager.sex = num;
	}

	public void OnClickDownload()
	{
		var url = "https://www.dropbox.com/s/f32nfu95s2cy8v9/%E5%BE%B3Simulator.zip?dl=0";
#if UNITY_EDITOR
		Application.OpenURL(url);
#elif UNITY_WEBGL
		Application.ExternalEval(string.Format("window.open('{0}','_blank')", url));
#else
		Application.OpenURL(url);
#endif
	}
}