using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 负责执行关卡场景的初始化工作。
/// </summary>
class PlayScene : MonoBehaviour
{

	public static PlayScene Instance { get; private set; }

	public bool isOver = false;
	public bool isFinished = false;
	public bool isSelectLevel = false;

	private bool[] isPlayerFinished = { false, false };

	//一个玩家离开终点
	public void UnFinish(int id)
	{
		isPlayerFinished[id - 1] = false;
	}

	// 一个玩家到终点
	public void Finish(int id)
	{
		isPlayerFinished[id - 1] = true;
		if(isPlayerFinished[0] && isPlayerFinished[1])
		{
			Finish();
		}
		
	}

	public void Finish()
	{
		Debug.Log("你赢了");
		this.isFinished = true;

		GameManager.Instance.LoadScene();
	}

	private void Awake()
	{
		Instance = this;

		//
		isOver = false;
		isFinished = false;

		//初始化
		Lightning.Init();
	}

	private void Start()
	{
		
		
	}



	
}

