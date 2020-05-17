using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootPanel : MonoBehaviour
{
    private SelectLevelUI ui;

    void Awake()
    {
        ui = transform.parent.GetComponent<SelectLevelUI>();
    }

    // Update is called once per frame
    void Update()
    {
		for(KeyCode keyCode = KeyCode.Alpha1;keyCode <= KeyCode.Alpha4; keyCode++)
        {
            if (Input.GetKeyDown(keyCode))
            {
                ui.OpenPanel(ui.transform.GetChild(keyCode - KeyCode.Alpha0 - 1).gameObject);
				return;
			}
        }
	}
}
