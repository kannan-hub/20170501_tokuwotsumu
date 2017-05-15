using UnityEngine;
using System.Collections;
using System.Linq;
public class CsvReaderTest :MonoBehaviour
{

	private class TestEnemyData
	{
		[CsvColumnAttribute (0, 0)]
		private int id;

		[CsvColumnAttribute (1, "NoName")]
		public string Name { get; set; }

		[CsvColumnAttribute (2, 100)]
		public int Hp;

		public override string ToString()
		{
			return string.Format ("Id={0}, Name={1}, HP={2}", id, Name, Hp);
		}

	}
	// Use this for initialization
	/* 以下のようなデータを読み込み
	#id, name, hp
	0,"ゴブリン",10
	1,"ボム",20
	2,"スライム",10
	3,"ベヒーモス",30
	*/
	void Start()
	{
		//readerをList<TestEnemyData>化して、それぞれ表示
		using ( var reader = new CSVReader<TestEnemyData> ("Texts/test_enemy", true) )
		{
			reader.ToList ().ForEach (enemy => Debug.Log (enemy.ToString ()));
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}