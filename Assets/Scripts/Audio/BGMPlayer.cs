﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
	private void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}
}