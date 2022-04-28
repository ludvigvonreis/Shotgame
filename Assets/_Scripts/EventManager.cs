using UnityEngine;
using UnityEngine.Events;

public class Hit
{
	public GameObject from;
	public RaycastHit raycastHit;
	public float time;

	public WeaponStats weaponStats;


	public Hit(GameObject _from, RaycastHit _hit, WeaponStats stats)
	{
		from = _from;
		raycastHit = _hit;

		time = Time.time;
		weaponStats = stats;
	}
}

[System.Serializable]
public class InteractEvent : UnityEvent<Interaction> { }

[System.Serializable]
public class HitEvent : UnityEvent<Hit> { }

public class EventManager : MonoBehaviour
{
	[SerializeField] private bool debugPrint;

	// Events
	[HideInInspector] public InteractEvent m_Interact;
	[HideInInspector] public HitEvent m_HitEvent;

	void Start()
	{
		if (m_Interact == null)
			m_Interact = new InteractEvent();

		if (m_HitEvent == null)
			m_HitEvent = new HitEvent();


		if (debugPrint)
		{
			m_Interact.AddListener(InteractListener);
			m_HitEvent.AddListener(HitListener);
		}
	}

	void InteractListener(Interaction interaction)
	{
		Debug.LogFormat("{0} interacted with {1}, distance={2}",
			interaction.from.name, interaction.to.name, interaction.distance);
	}

	void HitListener(Hit hit)
	{
		Debug.LogFormat("{0} hit {1} at {2}",
			hit.from.name, hit.raycastHit.transform.name, hit.time);
	}


	private static EventManager _instance;
	public static EventManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<EventManager>();
			}

			return _instance;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}
}
