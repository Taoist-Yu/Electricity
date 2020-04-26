using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ClockNode : MonoBehaviour
{
	//渐变速度
	private const float rate_speed = 5.0f;

	Material sprite_material;
	SpriteRenderer spriteRenderer;

	[SerializeField]
	private float rate = 0.0f;
	private enum State
	{
		starting,
		running,
		closing,
		sleeping
	}
	private State state;

	#region Public Method
	public void Enable()
	{
		gameObject.SetActive(true);
		this.state = State.starting;
		rate = 0.0f;
	}

	public void Disable()
	{
		this.state = State.closing;
		rate = 1.0f;
	}
	#endregion

	#region MonoBehaviour
	private void Awake()
	{
		this.spriteRenderer = GetComponent<SpriteRenderer>();
		this.sprite_material = spriteRenderer.material;
	}

	private void Start()
	{
		state = State.running;
		rate = 1.0f;
	}

	private void Update()
	{
		//设置Rate
		sprite_material.SetFloat("_Rate", rate);

		//更新状态
		switch (this.state)
		{
			case State.closing:
				this.Closing();
				break;
			case State.starting:
				this.Starting();
				break;
			default:
				break;
		}
	}

	#endregion

	#region State Manager

	private void Starting()
	{
		if(rate < 1)
		{
			rate += rate_speed * Time.deltaTime;
		}
		else
		{
			rate = 1.0f;
			this.state = State.running;
		}
	}

	private void Closing()
	{
		if (rate > 0)
		{
			rate -= rate_speed * Time.deltaTime;
		}
		else
		{
			rate = 0.0f;
			this.state = State.sleeping;
			gameObject.SetActive(false);
		}
	}

	#endregion
}