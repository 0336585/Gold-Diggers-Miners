using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapPixel 
{
    [SerializeField] private Color pixel;
    [SerializeField] private GameObject gameObject;

    public Color GetPixel { get { return pixel; } }

    public GameObject GetGameObject { get { return gameObject; } }
}
