using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPath : MonoBehaviour
{
    private List<Collider2D> path;

    void Start()
    {
        path = new List<Collider2D>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            string str = "";
            for (int i = 0; i < path.Count; i++)
            {
                str += path[i].transform.position + " ";
            }
            Debug.Log(str);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Path")
        {
            path.Add(collider);
        }
    }
}
