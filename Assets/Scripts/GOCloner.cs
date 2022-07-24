using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GOCloner : MonoBehaviour
{
    [SerializeField] private int count = 100;
    [SerializeField] [CanBeNull] private GameObject go;
    [SerializeField] [CanBeNull] private GameObject root;

    private void Start()
    {
        Debug.Log("Start");
        if (go && root)
        {
            // while (root.transform.childCount > 0)
            // {
            //     DestroyImmediate(root.transform.GetChild(0));
            // }

            for (int i = 0; i < count; i++)
            {
                var newNode = Instantiate(go, root.transform);
            }
        }
    }
}