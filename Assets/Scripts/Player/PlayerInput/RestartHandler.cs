using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

/// <summary>
/// 挂载到终点下
/// </summary>
public class RestartHandler : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameManager.Instance.currentLevel = 0;
			GameManager.Instance.LoadScene();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			PlayScene.Instance.isOver = true;
			GameManager.Instance.LoadScene();
		}
	}
}
