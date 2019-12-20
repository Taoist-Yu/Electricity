using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMask : MonoBehaviour
{

	private float begin = 0;
	private float end = 180;

	private float acc = 5;
	private float x = 0;
	private float v = 1;

	private void Awake()
	{
		Vector3 _pos = transform.position;
		_pos.z = 0;
		transform.position = _pos;
	}

	void Start()
    {
		Transform dtrans = transform.Find("Sprite_down");
		Transform utrans = transform.Find("Sprite_up");
		if (PlayScene.Instance.isOver || PlayScene.Instance.isFinished)
		{
			dtrans.gameObject.SetActive(true);
		}
		else
		{
			utrans.gameObject.SetActive(true);
		}
	}

    void Update()
    {
		x += v * Time.deltaTime;
		v += acc * Time.deltaTime;

		if(x > 1)
		{
			x = 1;
			v = Mathf.Max(-3, -v/3);
			if(Mathf.Abs(v) < 0.2f)
			{
				GameManager.Instance.SendMessage("EndLevelMask");
				Destroy(gameObject, 0.1f);
			}
		}

		float r = begin + x * (end - begin);
		Vector3 euler = transform.rotation.eulerAngles;
		euler.z = r;
		transform.rotation = Quaternion.Euler(euler);

	}
}
