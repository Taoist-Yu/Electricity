using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : MonoBehaviour
{

	[SerializeField]
	[Header("上升高度")]
	private float height;

	[SerializeField]
	[Header("上升速度")]
	private float speed;

	[SerializeField]
	[Header("最大（最小）高度时的停留时间")]
	private float duration;

	[SerializeField]
	[Header("初始状态(0下方停留,1上升，2上方停留，3下降)")]
	private int initStatus;

	private Vector3 init_pos;
	private float timeVal;	//停留时的计时器
	private int status;		//0下停留,1上升，2上停留，3下降
	

    // Start is called before the first frame update
    void Start()
    {
		init_pos = transform.position;
		status = initStatus;
		if(status >= 2)
		{
			init_pos.y -= height;
		}
    }

    // Update is called once per frame
    void Update()
    {
        if(status == 0)
		{
			timeVal += Time.deltaTime;
			if (timeVal > duration)
			{
				timeVal = 0;
				status = 1;
			}
		}
		else if(status == 1)
		{
			if(transform.position.y < init_pos.y + height)
			{
				Vector3 _pos = transform.position;
				_pos.y += speed * Time.deltaTime;
				transform.position = _pos;
			}
			else
			{
				Vector3 _pos = init_pos;
				_pos.y += height;
				transform.position = _pos;
				status = 2;
			}
		}
		else if(status == 2)
		{
			timeVal += Time.deltaTime;
			if (timeVal > duration)
			{
				timeVal = 0;
				status = 3;
			}
		}
		else
		{
			if (transform.position.y > init_pos.y)
			{
				Vector3 _pos = transform.position;
				_pos.y -= speed * Time.deltaTime;
				transform.position = _pos;
			}
			else
			{
				transform.position = init_pos;
				status = 0;
			}
		}
    }
}
