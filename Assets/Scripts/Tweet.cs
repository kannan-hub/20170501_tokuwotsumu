using UnityEngine;


public class Tweet : MonoBehaviour {

	string space = " ";
	string tweetSentence = "";
	[SerializeField]
	string hashtag = "#徳";
	[SerializeField]
	string url = "https://twitter.com/turkey_inc29"; //仮URL ゲームへのURLに後ほど変更

	public void TweetResult(int score, string name)
	{
		tweetSentence = "徳を " + score + " 積んで、「" + name + "」になりました。";

		var tweet = "http://twitter.com/intent/tweet?text=" + WWW.EscapeURL (tweetSentence + space + hashtag + space + url);
		
#if UNITY_EDITOR
		Application.OpenURL(tweet);
#elif UNITY_WEBGL
		Application.ExternalEval(string.Format("window.open('{0}','_blank')", tweet));
#else
		Application.OpenURL(tweet);
#endif
		
	}

}
