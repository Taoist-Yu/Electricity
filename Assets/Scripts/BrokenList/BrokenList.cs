using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenList : MonoBehaviour
{
	[SerializeField]//测试用
	private List<BrokenNode> brokenList = new List<BrokenNode>();

	[SerializeField]
	[Header("破碎一个单位的时间")]
	private float duration = 1.0f;

	private void Awake()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			BrokenNode node = transform.GetChild(i).GetComponent<BrokenNode>();
			if(node.GetComponent<BrokenNode>() != null)
			{
				brokenList.Add(node);
			}
			
		}
	}

	private void SwitchOn()
	{
		StartCoroutine(Broke());
	}

	private void SwitchOff()
	{
		
	}
	
	private IEnumerator Broke()
	{
		BrokenNode[] array = brokenList.ToArray();
		for(int i = 0; i < array.Length; i++)
		{
			yield return new WaitForSeconds(duration);
			array[i].Broke();
		}
		yield return null;
	}



}
