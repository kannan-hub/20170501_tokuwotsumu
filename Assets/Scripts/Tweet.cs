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

		Application.OpenURL ("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL (tweetSentence + space + hashtag + space + url));
	}

}
