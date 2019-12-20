using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipPlayer : MonoBehaviour
{
	#region 单例模式
	public static ClipPlayer Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject go = new GameObject("ClipPlayer");
				GameObject.DontDestroyOnLoad(go);
				_instance = go.AddComponent<ClipPlayer>();
			}
			return _instance;
		}
	}
	private static ClipPlayer _instance = null;
	#endregion

	public AudioClip jump;
	public AudioClip dead;
	public AudioClip open;

	private void Awake()
	{
		jump = Resources.Load<AudioClip>("Audios/jump");
		dead = Resources.Load<AudioClip>("Audios/dead");
		open = Resources.Load<AudioClip>("Audios/open");
	}


	public void Play(AudioClip clip)
	{
		GameObject go = new GameObject("Clip");
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.loop = false;
		source.Play();

		GameObject.DontDestroyOnLoad(go);
		GameObject.Destroy(go, 2.0f);

	}



}
