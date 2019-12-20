using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	private static GameObject lObj1;
	private static GameObject lObj2;

	private static LightningMono link;

	#region 对外接口
	public static void Init()
	{
		GameObject go = Resources.Load<GameObject>("Prefabs/LinkRenderer");
		lObj1 = GameObject.Instantiate(go);

		link = lObj1.GetComponent<LightningMono>();
	}

	/// <summary>
	/// 为线段渲染器添加顶点
	/// 有效期为一帧
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="id"></param>
	public static void AddNode(Vector2 pos)
	{
		Vector3 _pos = Vector3.zero;
		_pos.x = pos.x;
		_pos.y = pos.y;

		link.AddNode(_pos);
	}

	#endregion


	#region 私有内容



	#endregion



}
