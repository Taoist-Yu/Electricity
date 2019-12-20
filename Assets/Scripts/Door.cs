﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	private Animator animator;
	private Collider2D collider;

	private GameObject upDoor;
	private GameObject downDoor;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();

		upDoor = transform.Find("UpDoor").gameObject;
		downDoor = transform.Find("DownDoor").gameObject;
	}

	private void SwitchOn()
	{
		ClipPlayer.Instance.Play(ClipPlayer.Instance.open);
		animator.SetBool("IsOpen", true);
	}

	private void SwitchOff()
	{
		//animator.SetBool("IsOpen", false);
	}

	private void DestorySelf()
	{
		Destroy(gameObject);
	}

}
