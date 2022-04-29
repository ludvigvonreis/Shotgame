using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarDisplay : MonoBehaviour
{
	[SerializeField] private Player player;

	[SerializeField] private Image bar;
	[SerializeField] private TMP_Text text;
	[SerializeField, Range(0f, 10f)] private float animTime;

	void Start()
	{
		player.m_HealthChange.AddListener(HandleHealthChange);
	}

	public void HandleHealthChange(HealthChange healthChange)
	{
		UpdateDisplay(healthChange.health, healthChange.maxHealth);
	}

	void UpdateDisplay(float health, float maxHealth)
	{
		text.text = Mathf.CeilToInt(health).ToString();

		StartCoroutine(HealthAnim(health, maxHealth));
	}

	IEnumerator HealthAnim(float health, float maxHealth)
	{
		var percent = health / maxHealth;
		var oldAmount = bar.fillAmount;

		var t = 0f;
		var flow = 0f;
		while (flow < 1f)
		{
			t += Time.deltaTime;
			flow = t / animTime;

			bar.fillAmount = Mathf.Lerp(oldAmount, percent, flow);

			yield return null;
		}

		bar.fillAmount = percent;
	}
}
