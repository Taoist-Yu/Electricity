using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMono : MonoBehaviour
{
	#region 字段
	private LineRenderer lineRenderer;
	#endregion

	#region 生命周期函数

	private void Awake()
	{
		lineRenderer = gameObject.GetComponent<LineRenderer>();
	}

	private void Start()
	{
		this.InitComponent();
	}

	private void Update()
	{
		//生成顶点链表
		Vector3[] nodeArray = nodeList.ToArray();
		if (nodeArray.Length >= 2)
		{
			Vector3 start = nodeArray[0];
			Vector3 end = nodeArray[1];
			for(int i = 1; i < nodeArray.Length; i++)
			{
				start = nodeArray[i-1];
				end = nodeArray[i];
				this.CollectVertex(start, end);
			}
		}

		//更新LineRender的顶点数组
		lineRenderer.positionCount = posList.Count;
		lineRenderer.SetPositions(posList.ToArray());

		//清空顶点链表与节点链表
		nodeList.Clear();
		posList.Clear();
	}

	#endregion

	#region 对外接口

	public void AddNode(Vector3 pos)
	{
		nodeList.Add(pos);
	}

	#endregion

	#region 私有内容

	//绘制相关
	private float detail = 0.05f;           //单节闪电最小中点偏移量，该值越小闪电链节数越多
	private List<Vector3> posList = new List<Vector3>();			//顶点链表
	private List<Vector3> nodeList = new List<Vector3>();           //节点链表

	private void InitComponent()
	{
		//LineRenderer
		lineRenderer.startColor = Color.white;
		lineRenderer.endColor = Color.white;
		//lineRenderer.material = Resources.Load<Material>("Resources/unity_builtin_extra/Default-Line(Material)");
		lineRenderer.startWidth = 0.03f;
		lineRenderer.endWidth = 0.03f;
	}

	private void CollectVertex(Vector3 start, Vector3 end)
	{
		this.CollectVertex(start, end, Vector3.Distance(start, end));
	}

	private void CollectVertex(Vector3 start, Vector3 end, float dis)
	{
		if (dis < detail)
		{
			start.z = 0;
			posList.Add(start);
			return;
		}

		//生成中点
		float midx = (start.x + end.x) / 2 + (Random.value - 0.5f) * dis * 0.15f;
		float midy = (start.y + end.y) / 2 + (Random.value - 0.5f) * dis * 0.15f;
		float midz = (start.z + end.z) / 2 + (Random.value - 0.5f) * dis * 0.15f;
		Vector3 midPos = new Vector3(midx, midy, midz);

		//递归左右线段
		CollectVertex(start, midPos, dis / 2);
		CollectVertex(midPos, end, dis / 2);
	}

	#endregion

}
