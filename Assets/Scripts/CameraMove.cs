using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

	public GameObject player1;
	public GameObject player2;

	public float minSize = 7.0f;

	private Vector3 currentVelocity;
	private Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//跟随
		Vector3 _pos = 0.5f * (player1.transform.position + player2.transform.position);
		_pos.z = transform.position.z;
		transform.position = Vector3.SmoothDamp(transform.position, _pos, ref currentVelocity, 1.0f);

		//缩放
		float sh = (1) * Mathf.Abs(player2.transform.position.x - player1.transform.position.x);
		float sv = (1.25f) * Mathf.Abs(player2.transform.position.y - player1.transform.position.y);

		float size = Mathf.Max(Mathf.Max(sh, sv), minSize);
		camera.orthographicSize = camera.orthographicSize + 0.02f*(size - camera.orthographicSize);

	}


}
