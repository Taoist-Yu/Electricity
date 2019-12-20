using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public class PlayerMove : MonoBehaviour
	{
		private Player player;  //player 脚本的索引

		#region 基于射线检测的碰撞模拟系统

		/// <summary>
		/// 射线的位置信息
		/// </summary>
		[System.Serializable]
		private struct RayInfo
		{
			[Range(0.1f, 10f)]
			public float up;            //上方射线的发射起点
			[Range(0.1f, 10f)]
			public float down;        //下方射线的发射起点
			[Range(0.1f, 10f)]
			public float width;			//玩家的逻辑宽度
			[Range(0.1f, 10f)]
			public float rayLength;		//射线的长度
		};
		[SerializeField]
		private RayInfo rayInfo = new RayInfo
		{
			up = 0.5f,
			down = 0.5f,
			width = 1.0f,
			rayLength = 0.2f,
		};
		/// <summary>
		///  射线种类索引表
		///  记录了模拟碰撞检测所用的所有射线
		/// </summary>
		private enum RayIndexTable
		{
			down, left, right, up,
			ldown, rdown,
			dleft,uleft,dright,uright,
			TABLE_LENGTH
		}
		/// <summary>
		/// 碰撞检测结果信息
		/// </summary>
		private RaycastHit2D[][] hits = new RaycastHit2D[(int)RayIndexTable.TABLE_LENGTH][];

		/// <summary>
		/// 根据射线类型获取射线
		/// </summary>
		/// <param name="index"></param> 需要的射线类型
		/// <returns></returns> 获取的射线
		private Ray2D GetRay(RayIndexTable index)
		{
			Ray2D ray = new Ray2D();
			Vector2 origin = Vector2.zero;
			Vector2 direction = Vector2.zero;
			switch (index)
			{
				case RayIndexTable.down:
					origin = Vector2.down * rayInfo.down;
					direction = Vector2.down;
					break;
				case RayIndexTable.left:
					origin = Vector2.left * rayInfo.width * 0.5f;
					direction = Vector2.left;
					break;
				case RayIndexTable.right:
					origin = Vector2.right * rayInfo.width * 0.5f;
					direction = Vector2.right;
					break;
				case RayIndexTable.up:
					origin = Vector2.up * rayInfo.down;
					direction = Vector2.up;
					break;
				case RayIndexTable.dleft:
					origin = Vector2.left * rayInfo.width * 0.5f + Vector2.down * rayInfo.down;
					direction = Vector2.left;
					break;
				case RayIndexTable.uleft:
					origin = Vector2.left * rayInfo.width * 0.5f + Vector2.up * rayInfo.down;
					direction = Vector2.left;
					break;
				case RayIndexTable.dright:
					origin = Vector2.right * rayInfo.width * 0.5f + Vector2.down * rayInfo.down;
					direction = Vector2.right;
					break;
				case RayIndexTable.uright:
					origin = Vector2.right * rayInfo.width * 0.5f + Vector2.up * rayInfo.down;
					direction = Vector2.right;
					break;
				case RayIndexTable.ldown:
					origin = Vector2.left * rayInfo.width * 0.3f + Vector2.down * rayInfo.down;
					direction = Vector2.down;
					break;
				case RayIndexTable.rdown:
					origin = Vector2.right * rayInfo.width * 0.3f + Vector2.down * rayInfo.down;
					direction = Vector2.down;
					break;
				case RayIndexTable.TABLE_LENGTH:
					Debug.LogError("不允许传入TABLE_LENGTH");
					break;

			}

			origin.x += transform.position.x;
			origin.y += transform.position.y;
			ray.origin = origin;
			ray.direction = direction;
			return ray;
		}

		private Ray2D GetRay(int index)
		{
			return this.GetRay((RayIndexTable)index);
		}

		/// <summary>
		/// 每帧调用，执行一次碰撞检测
		/// 只会在指定碰撞Layer发射射线
		/// </summary>
		private void CastRays()
		{
			int layerMask = ~(1 << 8);
			for(int i = 0; i < (int)RayIndexTable.TABLE_LENGTH; i++)
			{
				Ray2D ray = this.GetRay(i);
				hits[i] = Physics2D.RaycastAll(ray.origin, ray.direction, rayInfo.rayLength * 1.1f, layerMask);
			}
		}

		#endregion

		#region MonoBehaviour生命周期
		private void Awake()
		{
			this.hits = new RaycastHit2D[(int)RayIndexTable.TABLE_LENGTH][];

			this.player = gameObject.GetComponent<Player>();

			this.animator = transform.Find("PlayerRenderer").GetComponent<Animator>();
			this.spriteRenderer = transform.Find("PlayerRenderer").GetComponent<SpriteRenderer>();

			this.dust = Resources.Load<GameObject>("Prefabs/Dust");
		}

		private void Start()
		{
			
		}

		private void Update()
		{
			this.CastRays();
			this.MoveUpdate();
		}

		private void FixedUpdate()
		{
			//this.CastRays();
		}

		#endregion

		#region 玩家移动相关逻辑

		/*
		 * 水平以右为正方向
		 * 垂直以上为正方向
		 */

		private Animator animator;
		private SpriteRenderer spriteRenderer;
		private GameObject dust;

		private float gravity => 8 * jumpHeight / (jumpTime * jumpTime);

		private float hVelocity;		//水平速度
		private float vVelocity;        //垂直速度
		private bool isGround;          //是否脚踏实地

		//取值-1、0、1，代表水平轴的方向键有没有被按下
		[HideInInspector]
		public int hInput = 0;
		//取值0、1，代表上方向键有没有被按下
		[HideInInspector]
		public int vInput = 0;

		[SerializeField]
		[Header("水平方向最大速度")]
		private float hMaxVelocity = 1;
		[SerializeField]
		[Header("悬空时水平方向最大速度")]
		private float hMaxVelocityInSpace = 1;
		[SerializeField]
		[Header("跳跃高度")]
		private float jumpHeight = 3;
		[SerializeField]
		[Header("跳跃时间")]
		private float jumpTime = 1;

		private void CheckIsGround()
		{
			//判断玩家是否在地面上
			this.isGround = false;
			
			foreach (var hit in hits[(int)RayIndexTable.down])
			{
				if (hit.collider.isTrigger == false && hit.transform != transform)
				{
					this.isGround = true;
					//更新y坐标
					Vector3 _pos = transform.position;
					_pos.y = hit.point.y + rayInfo.down + rayInfo.rayLength;
					transform.position = _pos;
					/*if (hit.transform.CompareTag("Player"))
					{
						PlayerMove other_move = hit.transform.GetComponent<PlayerMove>();
						if (other_move.vVelocity > 0)
						{
							this.vVelocity = other_move.vVelocity;
						}
					}*/
				}
			}
			foreach (var hit in hits[(int)RayIndexTable.ldown])
			{
				if (hit.collider.isTrigger == false && hit.transform != transform)
				{
					this.isGround = true;
					//更新y坐标
					Vector3 _pos = transform.position;
					_pos.y = hit.point.y + rayInfo.down + rayInfo.rayLength;
					transform.position = _pos;
					/*if (hit.transform.CompareTag("Player"))
					{
						PlayerMove other_move = hit.transform.GetComponent<PlayerMove>();
						if (other_move.vVelocity > 0)
						{
							this.vVelocity = other_move.vVelocity;
						}
					}*/
				}
			}
			foreach (var hit in hits[(int)RayIndexTable.rdown])
			{
				if (hit.collider.isTrigger == false && hit.transform != transform)
				{
					this.isGround = true;
					//更新y坐标
					Vector3 _pos = transform.position;
					_pos.y = hit.point.y + rayInfo.down + rayInfo.rayLength;
					transform.position = _pos;
					/*if (hit.transform.CompareTag("Player"))
					{
						PlayerMove other_move = hit.transform.GetComponent<PlayerMove>();
						if (other_move.vVelocity > 0)
						{
							this.vVelocity = other_move.vVelocity;
						}
					}*/
				}
			}

			if(isGround == true)
			{
				if(vVelocity < 0)
				{
					if(vVelocity < -30.0f)
					{
						//产生灰尘 
						GameObject.Destroy(GameObject.Instantiate(dust, transform), 1.0f);
					}
					vVelocity = 0;
				}
			}
		}
		
		/// <summary>
		/// 应用传送带
		/// </summary>
		private void CheckBelt()
		{
			Belt belt = null;
			foreach(var hit in hits[(int)RayIndexTable.down])
			{
				Belt _belt = hit.transform.GetComponent<Belt>();
				if(_belt != null)
				{
					belt = _belt;
				}
			}
			foreach (var hit in hits[(int)RayIndexTable.ldown])
			{
				Belt _belt = hit.transform.GetComponent<Belt>();
				if (_belt != null)
				{
					belt = _belt;
				}
			}
			foreach (var hit in hits[(int)RayIndexTable.rdown])
			{
				Belt _belt = hit.transform.GetComponent<Belt>();
				if (_belt != null)
				{
					belt = _belt;
				}
			}

			if(belt != null)
			{
				//transform.Translate(Vector3.right * belt.Speed * Time.deltaTime);
				hVelocity += belt.Speed;
			}
		}

		private void MoveUpdate()
		{
			if (PlayScene.Instance.isOver)
			{
				return;
			}
			if (PlayScene.Instance.isFinished)
			{
				GetComponent<Collider2D>().isTrigger = false;
				Vector3 target = GameObject.FindWithTag("Finish").transform.position;
				transform.position = Vector3.Lerp(transform.position, target, 0.1f);
				return;
			}


			//判断玩家头顶是否有东西
			foreach(var hit in hits[(int)RayIndexTable.up])
			{
				//玩家会被碰撞体挡住
				if(hit.collider.isTrigger == false && hit.transform != transform)
				{
					if(vVelocity > 0)
					{
						vVelocity = 0;
					}
				}
			}

			//判断玩家是否在地面上
			CheckIsGround();

			bool canRight = true;
			{
				foreach (var hit in hits[(int)RayIndexTable.dright])
				{
					if (hit.collider.isTrigger == false && hit.transform != transform)
					{
						canRight = false;
					}
				}
				foreach (var hit in hits[(int)RayIndexTable.right])
				{
					if (hit.collider.isTrigger == false && hit.transform != transform)
					{
						canRight = false;
					}
				}
				foreach (var hit in hits[(int)RayIndexTable.uright])
				{
					if (hit.collider.isTrigger == false && hit.transform != transform)
					{
						canRight = false;
					}
				}
			}


			bool canLeft = true;
			{
				foreach (var hit in hits[(int)RayIndexTable.dleft])
				{
					if (hit.collider.isTrigger == false && hit.transform != transform)
					{
						canLeft = false;
					}
				}
				foreach (var hit in hits[(int)RayIndexTable.left])
				{
					if (hit.collider.isTrigger == false && hit.transform != transform)
					{
						canLeft = false;
					}
				}
				foreach (var hit in hits[(int)RayIndexTable.uleft])
				{
					if (hit.collider.isTrigger == false && hit.transform != transform)
					{
						canLeft = false;
					}
				}
			}

			if (isGround)
			{
				//取消下落速度
				if (vVelocity < 0)
				{
					vVelocity = 0;
				}
				//监听移动
				hVelocity = hInput * hMaxVelocity;
				//监听跳跃
				if(vInput == 1)
				{
					Jump();
				}
			}
			else
			{
				//应用重力
				vVelocity -= gravity * Time.deltaTime;
				//检测向上碰撞
				

				hVelocity = hInput * hMaxVelocityInSpace;
			}

			//堵墙
			if (canRight == false && hVelocity > 0)
			{
				hVelocity = 0;
			}
			if (canLeft == false && hVelocity < 0)
			{
				hVelocity = 0;
			}

			//更新动画状态机(state变量，0静止，1跑步，2跳跃)
			if(!isGround)
			{
				animator.SetInteger("state", 2);
			}
			else
			{
				if(hVelocity > 0.1f || hVelocity < -0.1f)
				{
					animator.SetInteger("state", 1);
				}
				else
				{
					animator.SetInteger("state", 0);
				}
			}

			//设置人物朝向 
			if(hVelocity > 0)
			{
				spriteRenderer.flipX = true;
			}
			if (hVelocity < 0)
			{
				spriteRenderer.flipX = false;
			}

			if(isGround)
			{
				//检查传送带
				CheckBelt();
			}


			//根据速度移动人物
			transform.Translate(Vector3.right * hVelocity * Time.deltaTime);
			transform.Translate(Vector3.up * vVelocity * Time.deltaTime);
			
		}

		private void Jump()
		{
			float jumpForce = 4 * jumpHeight / jumpTime;
			this.vVelocity = jumpForce;

			ClipPlayer.Instance.Play(ClipPlayer.Instance.jump);

		}

		#endregion

		#region Gizmos相关

		private void OnDrawGizmos()
		{
			this.DrawRayGizmos();
		}

		private void DrawRayGizmos()
		{
			Gizmos.color = Color.red;
			//遍历射线索引表
			for (int i = 0; i < (int)RayIndexTable.TABLE_LENGTH; i++)
			{
				Ray2D ray = this.GetRay((RayIndexTable)i);
				Vector3 from = (Vector3)ray.origin;
				Vector3 to = from + (Vector3)ray.direction * rayInfo.rayLength;
				Gizmos.DrawLine(from, to);
			}
		}

		#endregion

	}

}
