using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour {

	private class AchievementPopupData
	{
		[CsvColumnAttribute (0, 0)]
		private int id;

		[CsvColumnAttribute (1, "NoTitle")]
		public string title { get; set; }

		[CsvColumnAttribute (2, "NoImage")]
		public string imageName { get; set; }

		[CsvColumnAttribute (3, "NoDetail")]
		public string detail { get; set; }

		[CsvColumnAttribute(4, "")]
		public string moreDetail { get; set; }

	}

	List<AchievementPopupData> readList;
	[SerializeField]
	List<Sprite> spriteList;
	Dictionary<string, Sprite> spriteDic;
	//Dictionary のセットアップの必要あり

	[SerializeField]
	GameObject popupBase;
	[SerializeField]
	Text titleText;
	[SerializeField]
	Image achievementImg;
	[SerializeField]
	Text detailText;
	[SerializeField]
	Text moreDetailText;
	
	void Start () {
		var reader = new CSVReader<AchievementPopupData> ("Texts/tokugame_achievement", true);
		readList = reader.ToList ();
		spriteDic = new Dictionary<string, Sprite> ();
		foreach ( Sprite spr in spriteList ) spriteDic.Add (spr.name, spr);
	}
	
	public void PopupView(AchievementButtonModel model )
	{
		SetPopupView (model);
		popupBase.SetActive (true);
	}

	public void ClosePopup()
	{
		titleText.text = "";
		achievementImg.sprite = spriteDic["hide_achievement"];
		detailText.text = "?????";
		moreDetailText.text = " ";

		popupBase.SetActive (false);
	}

	private void SetPopupView( AchievementButtonModel model )
	{
		int index = model.GetId() - 1;
		AchievementPopupData target = readList [index];
		titleText.text = target.title;

		if ( !model.IsAchieved () ) return;
		achievementImg.sprite = spriteDic [target.imageName];
		achievementImg.preserveAspect = true;
		detailText.text = target.detail;

		if ( target.moreDetail == null ) return;
		moreDetailText.text = target.moreDetail;
	}

}
