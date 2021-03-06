﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

	#region 单例模式
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject go = new GameObject("GameManager");
				GameObject.DontDestroyOnLoad(go);
				_instance = go.AddComponent<GameManager>();
			}
			return _instance;
		}
	}
	private static GameManager _instance = null;
	#endregion

	public int currentLevel = 1;

	private GameObject pre_black;
	private GameObject black;

	private void Awake()
	{
		pre_black = Resources.Load<GameObject>("Prefabs/LevelMask");
	}

	private void Update()
	{
		//接受esc输入
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			EscInput();
;		}
	}

	public void LoadScene()
	{
		StartLevelMask();
	}

	private void StartLevelMask()
	{
		GameObject mainCamera = GameObject.FindWithTag("MainCamera");
		black = Instantiate(pre_black, mainCamera.transform);

		Vector3 _pos = black.transform.localPosition;
		_pos.y = - mainCamera.GetComponent<Camera>().orthographicSize;
		black.transform.localPosition = _pos;

		_pos = black.transform.position;
		_pos.z = 0;
		black.transform.position = _pos;
	}

	private void EndLevelMask()
	{
		//选关跳转
		if (PlayScene.Instance.isSelectLevel)
		{
			PlayScene.Instance.isSelectLevel = false;
			SceneManager.LoadScene(0);
			return;
		}
		////带开始界面的第一关转化成不带开始界面的第一关
		//if (currentLevel == 1)
		//{
		//	currentLevel = 3;
		//}
		if (PlayScene.Instance.isOver)
		{
			SceneManager.LoadScene(0);
		}
		else if (PlayScene.Instance.isFinished)
		{
			++currentLevel;
			SceneManager.LoadScene(0);
		}
	}

	private void EscInput()
	{
		if (SceneManager.GetActiveScene().name == "SelectLevel")
		{
			SelectLevelUI ui = GameObject.Find("SelectPanel").GetComponent<SelectLevelUI>();
			ui.OpenPanel(ui.transform.Find("RootPanel").gameObject);
		} 
		else
		{
			PlayScene.Instance.isSelectLevel = true;
			GameManager.Instance.currentLevel = 1;
			GameManager.Instance.LoadScene();
		}
	}

}
