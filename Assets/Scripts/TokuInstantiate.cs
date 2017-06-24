using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TokuInstantiate :MonoBehaviour
{

	[SerializeField]
	GameObject tokuParents;
	[SerializeField]
	GameObject tokuRareParents;
	[SerializeField]
	GameObject tokuSRareParents;
	[SerializeField]
	GameController gameCol;

	Camera cam;
	List<GameObject> tokuInstance;
	List<GameObject> tokuRareInstance;
	List<GameObject> tokuSRareInstance;

	[SerializeField]
	int rareRatio = 5;
	[SerializeField]
	int srareRatio = 1;
	[SerializeField]
	bool autoInstance = false;

	const int instantiateRmin = 5;
	const int instantiateRmax = 10;
	const int instantiateSmin = 6;
	const int instantiateSmax = 12;
	const int instantiateSconst = 30;

	void Start()
	{
		cam = Camera.main;
		tokuInstance = new List<GameObject> ();
		foreach ( Transform child in tokuParents.transform ) tokuInstance.Add (child.gameObject);

		if ( SceneManager.GetActiveScene ().name == "main" )
		{
			tokuRareInstance = new List<GameObject> ();
			foreach ( Transform rarechild in tokuRareParents.transform ) tokuRareInstance.Add (rarechild.gameObject);
			tokuSRareInstance = new List<GameObject> ();
			foreach ( Transform srarechild in tokuSRareParents.transform ) tokuSRareInstance.Add (srarechild.gameObject);
			
			StartCoroutine (InstantiateToku());
		}

		if (autoInstance) StartCoroutine(AutoInstanceToku());
	}

	IEnumerator InstantiateToku()
	{
		while ( !gameCol.gameover)
		{
			transform.position = MousePosition ();

			if ( Input.GetMouseButtonDown (0))
			{
				string rarity = TokuRarity ();
				Vector2 instantiatePosition = InstantiateOffsetPosition ();
				GameObject toku = InstantiateToku (rarity, instantiatePosition);
				toku.transform.SetParent (transform);
				toku.SetActive (true);


				StartCoroutine ("ControllToku", toku);
			}

			yield return null;
		}
	}

	string TokuRarity()
	{
		int ratio = Random.Range (0, 100);

		if ( ratio <= srareRatio )
		{
			return "srare";
		}
		else if ( ratio <= rareRatio + srareRatio && ratio >= srareRatio )
		{
			return "rare";
		}

		return "";
	}

	GameObject InstantiateToku( string rarity, Vector2 position )
	{
		List<GameObject> list = GetInstanteList (rarity);
		int count = list.Count;
		int index = Random.Range (0, count);
		GameObject toku = Instantiate (list [index], position, Quaternion.identity);
		AddTokuList (toku);

		return toku;
	}

	List<GameObject> GetInstanteList( string rarity )
	{
		if ( rarity == "srare" )
		{
			return tokuSRareInstance;
		}
		else if ( rarity == "rare" )
		{
			return tokuRareInstance;
		}
		return tokuInstance;
	}

	IEnumerator ControllToku( GameObject obj )
	{
		while ( true )
		{
			if ( Input.GetMouseButtonUp (0) )
			{
				obj.GetComponent<HingeJoint2D> ().breakForce = 0;
				obj.transform.SetParent (tokuParents.transform);
				gameCol.score++;
				yield break;
			}
			yield return null;
		}
	}

	Vector3 MousePosition()
	{
		Vector3 screenPoint;
		screenPoint = cam.WorldToScreenPoint (transform.position);

		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;

		Vector3 currentScreenPoint = new Vector3 (x, y, screenPoint.z);
		Vector3 currentPosition = cam.ScreenToWorldPoint (currentScreenPoint);

		return currentPosition;
	}

	Vector2 InstantiateOffsetPosition()
	{
		float r = Random.Range (instantiateRmin, instantiateRmax + 1) * 0.1f;
		float s = Mathf.Deg2Rad * Random.Range (instantiateSmin, instantiateSmax + 1) * instantiateSconst;

		float offsetX = r * Mathf.Cos (s);
		float offsetY = r * Mathf.Sin (s);

		return new Vector2 (transform.position.x + offsetX, transform.position.y + offsetY);
	}

	void AddTokuList( GameObject obj )
	{
		TokuModel tm = obj.GetComponent<TokuModel> ();
		string rarity = tm.GetRarity ();
		if (rarity == "srare"  || rarity == "rare")
		{
			gameCol.aquiredTokuAchievementIdList.Add (tm.GetId());
		}
		
	}

	IEnumerator AutoInstanceToku()
	{
		while (true)
		{
			List<GameObject> list = tokuInstance;
			int count = list.Count;
			int index = Random.Range (0, count);
			Vector3 randPos = randPosition();
			GameObject toku = Instantiate (list [index], randPos, Quaternion.identity);
			toku.GetComponent<HingeJoint2D> ().breakForce = 0;
			toku.gameObject.SetActive(true);
		
			yield return new WaitForSeconds(0.5f);
		}
	}

	Vector3 randPosition()
	{
		float rand = Random.Range(-6, 6);
		float posX = cam.orthographicSize / 6 * rand;
		float posY = cam.orthographicSize * 0.8f;
		
		return new Vector3(posX, posY);
	}
}
