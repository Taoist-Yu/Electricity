using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransfer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		SceneManager.LoadScene(GameManager.Instance.currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
