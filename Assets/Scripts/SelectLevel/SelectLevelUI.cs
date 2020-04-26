using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class SelectLevelUI : MonoBehaviour
{

	GameObject current_panel;

	#region MonoBehaviour
	private void Awake()
	{
		current_panel = transform.Find("RootPanel").gameObject;
	}
	#endregion

	public void OpenPanel(GameObject panel)
	{
		this.CloseCurrent();
		panel.SetActive(true);
		current_panel = panel;
	}

	public void OpenLevel(int level_id)
	{
		PlayScene.Instance.isSelectLevel = true;
		GameManager.Instance.currentLevel = level_id;
		GameManager.Instance.LoadScene();
	}

	private void CloseCurrent()
	{
		current_panel.SetActive(false);
	}

}

