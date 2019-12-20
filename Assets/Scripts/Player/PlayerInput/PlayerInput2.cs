using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class PlayerInput2 : MonoBehaviour
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
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			playerMove.vInput = 1;
		}
		else
		{
			playerMove.vInput = 0;
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			playerMove.hInput = -1;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			playerMove.hInput = 1;
		}
		else
		{
			playerMove.hInput = 0;
		}

		//搬运输入
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			playerLogic.handle_input = true;
		}
		else
		{
			playerLogic.handle_input = false;
		}
	}
}
