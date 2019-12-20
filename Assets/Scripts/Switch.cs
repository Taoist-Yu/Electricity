
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
	[SerializeField]
	[Header("接受器")]
	private GameObject receiver;

	private SpriteRenderer spriteRenderer;

	[SerializeField]
	[Header("激活时的图片")]
	private Sprite activeSprite;

	//后置精灵
	private Sprite sprite;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		sprite = activeSprite;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Charging" && collision.transform.GetComponent<Collider2D>().isTrigger == false)
		{
			this.SwapSprite();
			receiver.SendMessage("SwitchOn");
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Charging" && collision.transform.GetComponent<Collider2D>().isTrigger == false)
		{
			this.SwapSprite();
			receiver.SendMessage("SwitchOff");
		}
	}

	private void SwapSprite()
	{
		Sprite temp = sprite;
		sprite = spriteRenderer.sprite;
		spriteRenderer.sprite = temp;
	}

}
