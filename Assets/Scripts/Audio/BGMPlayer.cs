using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
	private static bool hasBGM = false;
	

	private void Awake()
	{
		if (!hasBGM)
		{
			GameObject.DontDestroyOnLoad(gameObject);
			hasBGM = true;
		}
		else
		{
			DestroyImmediate(gameObject);
		}
		
	}
}
