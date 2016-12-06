﻿using UnityEngine;
using StaticClass;

public class OutLineColor : MonoBehaviour
{
    [SerializeField]
    Color[] color = new Color[GameRule.playerNum + 1];

    void Start ()
    {
        if (!GetComponent<SkinnedMeshRenderer>())
            return;

        SkinnedMeshRenderer mesh = GetComponent<SkinnedMeshRenderer>();

        switch (transform.root.gameObject.tag)
        {
            case "Player1":
                foreach (Material e in mesh.materials)
                    if (e.HasProperty("_OutlineColor"))
                        e.SetColor("_OutlineColor", color[0]);
                break;
            case "Player2":
                foreach (Material e in mesh.materials)
                    if (e.HasProperty("_OutlineColor"))
                        e.SetColor("_OutlineColor", color[1]);
                break;
            default:
                foreach (Material e in mesh.materials)
                    if (e.HasProperty("_OutlineColor"))
                        e.SetColor("_OutlineColor", color[2]);
                break;
        }
    }
}