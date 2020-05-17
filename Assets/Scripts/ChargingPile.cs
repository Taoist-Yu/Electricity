using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingPile : MonoBehaviour
{
	[SerializeField]
	[Header("是否为静态的")]
	private bool isStatic = false;

	public bool IsStatic
	{
		get
		{
			return isStatic;
		}
	}

	#region MonoBehaviour

	private void Awake()
	{
		
	}

	private void Update()
	{
		this.CheckBelt();
	}

	#endregion

	#region private Method

	private void CheckBelt()
	{
		//向下射线检测
		//RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1.0f);
		RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, new Vector2(1.0f, .2f), 0, Vector2.down, 1.0f);
		//遍历检测结果，查找其中是否由传送带
		Belt belt = null;
		foreach(var hit in hits)
		{
			GameObject go = hit.transform.gameObject;
			belt = go.GetComponent<Belt>();
			if(belt != null)
			{
				break;
			}
		}
		//应用传送带效果
		if(belt != null)
		{
			float speed = belt.Speed;
			transform.Translate(speed * Vector3.right * Time.deltaTime);
		}
	}

	#endregion

}
