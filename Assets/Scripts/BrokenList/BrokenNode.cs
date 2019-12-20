using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Broke()
	{
		GetComponent<Explodable>().explode();
		Destroy(gameObject, 1.0f);
	}
}
