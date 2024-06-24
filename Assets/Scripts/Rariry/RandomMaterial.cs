using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    public List<RarityInfo> rarityMaterials = new List<RarityInfo>();
    public Material bodyMateral;
    public List<Material> materials = new List<Material>();
    Dictionary<RariryEnum, int> _weight;
    private MeshRenderer myMaterial;

    private void Awake()
    {
        myMaterial = GetComponent<MeshRenderer>();
        _weight = new Dictionary<RariryEnum, int>();

        for (int i = 0; i < rarityMaterials.Count; i++)
        {
            _weight[rarityMaterials[i].rarity] = rarityMaterials[i].weight;
        }
    }
    //private void Update()
    //{
    //    rarityMaterials[1].weight++;
    //}

    public RariryEnum GetRandomMaterial()
    {
        return MyRandoms.Roulette(_weight);
    }

    // Utilizando la rareza para colocar un nuevo material
    public void SetRandomMaterial()
    {
        switch (GetRandomMaterial())
        {
            case RariryEnum.C:
                myMaterial.material = materials[0];
                Debug.Log("Comun");
                break;
            case RariryEnum.R:
                myMaterial.material = materials[1];
                Debug.Log("Rara");
                break;
            case RariryEnum.UR:
                myMaterial.material = materials[2];
                Debug.Log("UltraRara");
                break;
        }
    }
}
