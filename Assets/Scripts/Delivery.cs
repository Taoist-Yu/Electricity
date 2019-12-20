using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
	public float rotate_speed = 10.0f;
	public float scale_speed = 1.0f;

	Vector3 init_scale;

    // Start is called before the first frame update
    void Start()
    {
		init_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
		//transform.Rotate(Vector3.forward * rotate_speed * Time.deltaTime);

		//transform.localScale = init_scale * (1 + Mathf.Sin(Time.time * scale_speed) * 0.05f); 
    }
}
