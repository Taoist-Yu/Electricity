using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	[RequireComponent(typeof(PlayerMove))]
	[RequireComponent(typeof(PlayerLogic))]
	public class Player : MonoBehaviour
	{

		#region 字段

		public int PlayerID
		{
			get
			{
				return player_id;
			}
		}
		[SerializeField]
		private int player_id = 1;

		#endregion

		#region 子组件
		public PlayerMove Move { get; private set; }
		public PlayerLogic logic { get; private set; }


		#endregion

		#region MonoBehaviour生命周期
		private void Awake()
		{
			//初始化对象组件
			this.Move = gameObject.GetComponent<PlayerMove>();
			this.logic = gameObject.GetComponent<PlayerLogic>();
			
		}

		private void Start()
		{
			gameObject.tag = "Player";
		}

		#endregion

	}
}


