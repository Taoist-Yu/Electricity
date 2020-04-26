using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectLevelScene : MonoBehaviour
{
	public GameObject players;
	public GameObject block1;
	public GameObject block2;

	public GameObject introduction;
	public GameObject SelevePanel;

	private Text introduction_text;
	private Text title_text;

	//文字的Alpha值
	private float alpha1 = 1;

	//
	private bool isStarted = false;

	private void Awake()
	{
		introduction_text = introduction.GetComponent<Text>();
		title_text = introduction.transform.GetChild(0).GetComponent<Text>();
	}

	// Update is called once per frame
	void Update()
	{
		SetAlpha(introduction_text, alpha1);
		SetAlpha(title_text, alpha1);

		if (Input.anyKeyDown)
		{
			this.PreStartGame();
		}
		if (alpha1 < 0.05f && isStarted == false)
		{
			StartGame();
		}
	}

	void PreStartGame()
	{
		Sequence s = DOTween.Sequence();
		s.Append(DOTween.To(() => alpha1, x => alpha1 = x, 0.0f, 1.00f));
		SelevePanel.SetActive(true);
	}

	void StartGame()
	{
		isStarted = true;

		players.SetActive(true);

		block1.GetComponent<Explodable>().explode();
		block2.GetComponent<Explodable>().explode();
	}

	void SetAlpha(Text text, float alpha)
	{
		Color color = text.color;
		color.a = alpha;
		text.color = color;
	}



}
