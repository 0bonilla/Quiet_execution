using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    public List<RarityInfo> rarityItems = new List<RarityInfo>();
    public List<GameObject> Items = new List<GameObject>();
    Dictionary<RariryEnum, int> _weight;
    [SerializeField] private GameObject AttackWeapon;
    public Player _player;

    private void Awake()
    {
        _weight = new Dictionary<RariryEnum, int>();

        for (int i = 0; i < rarityItems.Count; i++)
        {
            _weight[rarityItems[i].rarity] = rarityItems[i].weight;
        }
    }
    public RariryEnum GetRandomItem()
    {
        return MyRandoms.Roulette(_weight);
    }

    public void LessRare()
    {
        rarityItems[2].weight += 1;
    }
    // Utilizando la rareza para colocar un nuevo material
    public void SetRandomItem()
    {
        switch (GetRandomItem())
        {
            case RariryEnum.C:
                _player.Daddy = Items[0].gameObject;
                Debug.Log("Comun");
                break;
            case RariryEnum.R:
                _player.Daddy = Items[1].gameObject;
                Debug.Log("Rara");
                break;
            case RariryEnum.UR:
                _player.Daddy = Items[2].gameObject;
                Debug.Log("UltraRara");
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            SetRandomItem();
        }
    }
}
