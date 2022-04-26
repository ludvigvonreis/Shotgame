using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecalManager : MonoBehaviour
{
	public GameObject decalPrefab;
	[Range(1, 100)]
	public int poolSize;
	private Queue<GameObject> pool = new Queue<GameObject>();

	private GameObject decalParent;

	private static DecalManager _instance;
	public static DecalManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<DecalManager>();
			}

			return _instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		decalParent = new GameObject("DecalParent");
		decalParent.transform.parent = this.transform;

		for (int i = 0; i < poolSize; i++)
		{
			var temp = Instantiate(decalPrefab, Vector3.zero, Quaternion.identity);
			temp.SetActive(false);
			pool.Enqueue(temp);
			temp.transform.parent = decalParent.transform;
		}
	}

	public GameObject PlaceDecal(Vector3 position, Quaternion rotation)
	{
		var obj = pool.Dequeue();
		obj.transform.position = position;
		obj.transform.rotation = rotation;

		obj.SetActive(true);
		pool.Enqueue(obj);

		obj.transform.parent = null;

		return obj;
	}

	public void RemoveDecal(GameObject obj)
	{
		obj.transform.position = Vector3.zero;
		obj.transform.rotation = Quaternion.identity;
		obj.SetActive(false);

		obj.transform.parent = decalParent.transform;
	}
}
