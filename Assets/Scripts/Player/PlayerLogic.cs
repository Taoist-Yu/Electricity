using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerLogic : MonoBehaviour
	{

		private Player player;  //player 脚本的索引

		#region Inspector属性

		[SerializeField]
		[Header("感应半径")]
		private float radius = 0;
		[SerializeField]
		[Header("存活时间")]
		private float lifeTime = 5.0f;

		#endregion

		#region 字段

		public static float Heare
		{
			get
			{
				return heart;
			}
		}

		private static float heart;
		private static bool isLinked;

		private GameObject playerRenderer;

		#endregion

		#region 对象生命周期

		private void Awake()
		{
			this.player = gameObject.GetComponent<Player>();
			this.playerRenderer = transform.Find("PlayerRenderer").gameObject;
		}

		private void Start()
		{
			
		}

		private void Update()
		{
			if (PlayScene.Instance.isOver || PlayScene.Instance.isFinished)
				return;

			this.LinkUpdate();
			this.HeartUpdate();
			this.HandlingUpdate();

		}

		#endregion

		#region 连接相关

		private void LinkUpdate()
		{
			if(player.PlayerID == 2)
			{
				return;
			}
			List<Transform> list = new List<Transform>();
			HashSet<Transform> k_set = new HashSet<Transform>();
			list.Add(transform);
			k_set.Add(transform);
			LinkDFS(list, k_set);
			if (list.Count > 0)
			{
				//搜索成功
				isLinked = true;
				Transform[] path = list.ToArray();
				for (int i = 0; i < path.Length; i++)
				{
					Lightning.AddNode(path[i].position);
				}
			}
			else
			{
				//搜索失败
				isLinked = false;
			}
		}

		/// <summary>
		/// 搜索一条可以连接两个玩家的路径
		/// </summary>
		/// <param name="list"></param>	当前路径链表
		/// <param name="k_set"></param> 当前已经搜索过的节点集合
		/// <returns></returns>
		public void LinkDFS(List<Transform> list, HashSet<Transform> k_set)
		{
			Transform trans = list[list.Count-1];
			//尝试寻找场景中的充电节点
			Collider2D[] nodes = Physics2D.OverlapCircleAll(trans.position, this.radius);
			//若附加有玩家，则连接并返回
			foreach (var node in nodes)
			{
				if (node.transform != transform && node.transform.CompareTag("Player"))
				{
					list.Add(node.transform);
					return;
				}
			}
			//若无玩家，DFS
			foreach(var node in nodes)
			{
				if(node.tag == "Charging" && !k_set.Contains(node.transform))
				{
					list.Add(node.transform);
					k_set.Add(node.transform);
					LinkDFS(list, k_set);
					if(list[list.Count - 1].CompareTag("Player") && list.Count != 1)
					{
						//成功搜索
						return;
					}
				}
			}

			//搜索失败
			list.Remove(trans.transform);
			k_set.Remove(trans.transform);
		}

		/// <summary>
		/// 生成闪电特效
		/// </summary>
		private void GenRenderLink(Transform target, int id)
		{
			
		}

		#endregion

		#region 血量相关

		private void HeartUpdate()
		{
			if (player.PlayerID == 2)
				return;

			if (isLinked == true)
			{
				heart = lifeTime;
			}
			else
			{
				heart -= Time.deltaTime;
				if(heart < 0)
				{
					this.Death();
				}
			}
		}

		private void Death()
		{
			playerRenderer.GetComponent<Animator>().SetTrigger("Dead");
			PlayScene.Instance.isOver = true;
			ClipPlayer.Instance.Play(ClipPlayer.Instance.dead);
			GameManager.Instance.LoadScene();
		}

		#endregion

		#region 触发事件监听

		private void OnTriggerEnter2D(Collider2D collision)
		{
			switch (collision.tag)
			{
				case "Trap":
					this.Death();
					break;
				case "Finish":
					PlayScene.Instance.Finish(player.PlayerID);
					GetComponent<Collider2D>().isTrigger = true;
					break;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			switch (collision.tag)
			{
				case "Finish":
					PlayScene.Instance.UnFinish(player.PlayerID);
					GetComponent<Collider2D>().isTrigger = false;
					break;
			}
		}

		#endregion

		#region 拾取相关

		public bool handle_input;

		[SerializeField]
		[Header("可以拿到多远的物体")]
		private float handle_distance;
		[SerializeField]
		[Header("物体被举起时在哪个位置")]
		private Vector3 handle_pos;
		[SerializeField]

		private GameObject PileInHandle;
		private Vector3 currentPileVelocity = Vector3.zero;

		private void HandlingUpdate()
		{
			//监听输入
			if(handle_input == true)
			{
				if (PileInHandle == null)
				{
					Vector2 direction = Vector2.right;
					if (playerRenderer.GetComponent<SpriteRenderer>().flipX == false)
					{
						direction = Vector2.left;
					}
					RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, handle_distance);
					float min_dis = (float)1e6;
					GameObject target = null;
					foreach (var hit in hits)
					{
						//找到充电桩且充电桩未被拿起
						if (hit.transform.CompareTag("Charging") && hit.collider.isTrigger == false)
						{
							//判断是否为固定
							if (hit.transform.GetComponent<ChargingPile>().IsStatic)
							{
								continue;
							}
							float dis = Vector2.Distance(hit.transform.position, transform.position);
							if(dis < min_dis)
							{
								min_dis = dis;
								target = hit.transform.gameObject;
							}
						}
					}
					//找到最近的充电桩
					if (target != null)
					{
						PileInHandle = target;
						PileInHandle.GetComponent<Collider2D>().isTrigger = true;
						PileInHandle.GetComponent<Rigidbody2D>().gravityScale = 0;
					}
				}
				else
				{
					//放下充电桩
					PileInHandle.GetComponent<Collider2D>().isTrigger = false;
					PileInHandle.GetComponent<Rigidbody2D>().gravityScale = 1;
					PileInHandle = null;
				}
			}
			
			//维护充电桩的位置信息
			if(PileInHandle != null)
			{
				Vector3 _pos = handle_pos;
				if(playerRenderer.GetComponent<SpriteRenderer>().flipX == false)
				{
					_pos.x = - _pos.x;
				}
				
				//PileInHandle.transform.position = transform.position + _pos;
				PileInHandle.transform.position = Vector3.SmoothDamp(
						PileInHandle.transform.position,
						transform.position+_pos,
						ref currentPileVelocity,
						0.1f
					);
			}

		}

		#endregion

	}
}
