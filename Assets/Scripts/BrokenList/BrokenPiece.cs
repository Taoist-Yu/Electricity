using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiece : MonoBehaviour
{
	Material mat;
	Color color;

	float timeVal = 1;

	private void Awake()
	{
		mat = GetComponent<MeshRenderer>().materials[0];
		color = mat.color;
	}

	private void Start()
	{
		//添加随机力
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		Vector2 force = Vector2.zero;
		force.x = Random.Range(-1, 1);
		force.y = Random.Range(-1, 1);
		rigidbody.mass = 0.005f;
		rigidbody.AddForce(force);

		//更改层级
		gameObject.layer = 8;
	}

	// Update is called once per frame
	void Update()
    {
		timeVal -= Time.deltaTime;
		color.a = timeVal;
		mat.color = color;
		if(timeVal < 0)
		{
			Destroy(gameObject);
		}
    }
}
