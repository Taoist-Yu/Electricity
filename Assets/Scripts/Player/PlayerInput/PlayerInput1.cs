using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerInput1 : MonoBehaviour
{
	PlayerMove playerMove;
	PlayerLogic playerLogic;

	// Start is called before the first frame update
	void Start()
	{
		playerMove = GetComponent<PlayerMove>();
		playerLogic = GetComponent<PlayerLogic>();
	}

	// Update is called once per frame
	void Update()
	{
		if (PlayScene.Instance.isOver || PlayScene.Instance.isFinished)
			return;
		//垂直输入
		if (Input.GetKeyDown(KeyCode.W))
		{
			playerMove.vInput = 1;
		}
		else
		{
			playerMove.vInput = 0;
		}

		//水平输入
		if (Input.GetKey(KeyCode.A))
		{
			playerMove.hInput = -1;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			playerMove.hInput = 1;
		}
		else
		{
			playerMove.hInput = 0;
		}

		//搬运输入
		if (Input.GetKeyDown(KeyCode.S))
		{
			playerLogic.handle_input = true;
		}
		else
		{
			playerLogic.handle_input = false;
		}


	}
}
