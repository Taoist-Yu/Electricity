using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class SelectLevelButton : MonoBehaviour
{

	private Button m_button;
	private Text m_text;

	#region MonoBehaviour

	private void Awake()
	{
		m_button = GetComponent<Button>();
		m_text = transform.Find("Text").GetComponent<Text>();
	}

	private void Start()
	{
		m_button.onClick.AddListener(LoadLevel);
	}

	#endregion

	private void LoadLevel()
	{
		//Get the level name
		string name = m_text.text;
		int major = name[5] - '0';
		int minor = name[7] - '0';
		int level_id = 2 + (major-1)*6 + minor;
		//Load Scene
		PlayScene.Instance.isSelectLevel = true;
		GameManager.Instance.currentLevel = level_id;
		GameManager.Instance.LoadScene();
	}

}

