using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour
{
	public Sprite[] l_sprites;
	public Sprite[] m_sprites;
	public Sprite[] r_sprites;

	[SerializeField]
	private List<SpriteRenderer> nodes = new List<SpriteRenderer>();

	[SerializeField]
	[Header("传输速度（👉为正方向）")]
	private float speed = 1.0f;
	public float Speed
	{
		get
		{
			return speed;
		}
	}

	private void Awake()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			SpriteRenderer node = transform.GetChild(i).GetComponent<SpriteRenderer>();
			if (node != null)
			{
				nodes.Add(node);
			}
		}
	}

	private void Update()
	{
		this.SpriteUpdate();
	}



	#region 切换精灵

	private float spriteTimeVal = 0;

	private void SpriteUpdate()
	{
		spriteTimeVal += Time.deltaTime;
		if(spriteTimeVal > 0.1f)
		{
			Swap();
			spriteTimeVal = 0;
		}
	}

	private void Swap()
	{
		Swap(l_sprites);
		Swap(m_sprites);
		Swap(r_sprites);

		SpriteRenderer[] array = nodes.ToArray();
		for(int i = 0; i < array.Length; i++)
		{
			if (i == 0)
			{
				array[i].sprite = l_sprites[0];
			}
			else if (i == array.Length - 1)
			{
				array[i].sprite = r_sprites[0];
			}
			else
			{
				array[i].sprite = m_sprites[0];
			}
		}
	}

	/// <summary>
	/// 整个数组往前移动一格，0号元素补到尾部
	/// </summary>
	private void Swap(Sprite[] sprites)
	{
		Sprite sprite = sprites[0];
		for(int i = 1; i < sprites.Length; i++)
		{
			sprites[i - 1] = sprites[i];
		}
		sprites[sprites.Length - 1] = sprite;
	}
	#endregion

}
