using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
	[Header("每一刻空缺的数量"), SerializeField]
	private int broken_cot;
	[Header("状态切换时间间隔"), SerializeField]
	private float time_interval;

	List<GameObject> clock_list = new List<GameObject>();
	Queue<GameObject> broken_queue = new Queue<GameObject>();
	Queue<GameObject> waiting_queue = new Queue<GameObject>();

	float timeVal;

	private void UpdateState()
	{
		GameObject obj;

		obj = waiting_queue.Dequeue();
		obj.GetComponent<ClockNode>().Disable();
		broken_queue.Enqueue(obj);

		obj = broken_queue.Dequeue();
		obj.GetComponent<ClockNode>().Enable();
		waiting_queue.Enqueue(obj);
	}

	#region Generate

	private void MoveNextPosition(int width,int height,bool clockwise,ref int direction , ref Vector2 pos)
	{
		int x = Mathf.RoundToInt(pos.x);
		int y = Mathf.RoundToInt(pos.y);

		int _dir = clockwise ? direction : (direction + 2)%4;
		switch (_dir)
		{
			case 0:
				x++;
				break;
			case 1:
				y--;
				break;
			case 2:
				x--;
				break;
			case 3:
				y++;
				break;
		}
		if(x<0 || y>0 || x>=width || y <= -height)
		{
			if (clockwise)
			{
				direction = (direction + 1) % 4;
			}
			else
			{
				direction -= 1;
				if (direction < 0)
				{
					direction = 3;
				}
			}
			MoveNextPosition(width, height, clockwise, ref direction, ref pos);
			return;
		}
		else
		{
			pos.x = x;
			pos.y = y;
		}
	}

	[EditorButton]
	public void Generate(int width, int height, bool clockwise,GameObject origin)
	{
		//clear
		List<GameObject> wait_destroy = new List<GameObject>();
		for(int index = 0; index < transform.childCount; index++)
		{
			wait_destroy.Add(transform.GetChild(index).gameObject);
		}
		foreach (GameObject obj in wait_destroy)
		{
			DestroyImmediate(obj);
		}

		int direction = clockwise ? 0 : 3;
		Vector2 current_pos=Vector2.zero;
		int i = 0;
		do
		{
			GameObject obj = GameObject.Instantiate(origin, this.transform);
			if(obj.GetComponent<ClockNode>() == null)
			{
				obj.AddComponent<ClockNode>();
			}
			obj.transform.localPosition = current_pos;
			obj.name = "Node_" + i;
			obj.SetActive(true);
			i++;
			MoveNextPosition(width, height, clockwise, ref direction, ref current_pos);
		} while (Vector2.Distance(current_pos, Vector2.zero) > 1e-6);
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		int i = 0;
		while (true)
		{
			string name = "Node_" + i;
			Transform trans = transform.Find(name);
			if(trans != null)
			{
				clock_list.Add(trans.gameObject);
				i++;
			}
			else
			{
				break;
			}
		}
	}

	private void Start()
    {
		IEnumerator<GameObject> it = clock_list.GetEnumerator();
		for(int i = 0; i < broken_cot; i++)
		{
			it.MoveNext();
			broken_queue.Enqueue(it.Current);
			it.Current.SetActive(false);
		}
		while(it.MoveNext())
		{
			waiting_queue.Enqueue(it.Current);	
		}
    }

    // Update is called once per frame
    private void Update()
    {
		if (timeVal < time_interval)
		{
			timeVal += Time.deltaTime;
		}else
		{
			UpdateState();
			timeVal = 0;
		}
    }

	#endregion
}
