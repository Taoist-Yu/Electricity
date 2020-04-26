using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransfer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		if(GameManager.Instance.currentLevel == SceneManager.GetActiveScene().buildIndex)
		{
			GameManager.Instance.currentLevel = 1;
		}
		SceneManager.LoadScene(GameManager.Instance.currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
