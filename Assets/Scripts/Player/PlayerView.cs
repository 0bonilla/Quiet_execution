using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour, IPlayerView
{
    public GameObject body;
    private RandomMaterial randomMaterial;
    protected virtual void Awake()
    {
        randomMaterial = GetComponent<RandomMaterial>();
    }
    private void Start()
    {
        // Randomizado skin
        randomMaterial.SetRandomMaterial();
    }
}
