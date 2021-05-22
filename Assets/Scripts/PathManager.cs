using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool destroy;

    void Start()
    {
        if (up)
        {
            name = "PathUp";
        }
        if (down)
        {
            name = "PathDown";
        }
        if (left)
        {
            name = "PathLeft";
        }
        if (right)
        {
            name = "PathRight";
        }
        if (destroy)
        {
            name = "DestroyBlock";
        }
    }
}
