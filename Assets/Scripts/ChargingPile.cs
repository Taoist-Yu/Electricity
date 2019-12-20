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
}
