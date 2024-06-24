using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAbility : MonoBehaviour
{
    public List<RarityInfo> rarityAbility = new List<RarityInfo>();
    Dictionary<RariryEnum, int> _weight;
    public Player _player;

    private void Awake()
    {
        _weight = new Dictionary<RariryEnum, int>();

        for (int i = 0; i < rarityAbility.Count; i++)
        {
            _weight[rarityAbility[i].rarity] = rarityAbility[i].weight;
        }
    }
    public RariryEnum GetRandomItem()
    {
        return MyRandoms.Roulette(_weight);
    }
    // Utilizando la rareza para colocar un nuevo material
    public void SetRandomItem()
    {
        switch (GetRandomItem())
        {
            case RariryEnum.C:
                _player.speed += 5;
                Debug.Log("Comun");
                break;
            case RariryEnum.R:
                _player.TotalCooldown -= 0.2f;
                Debug.Log("Rara");
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SetRandomItem();
        }
    }
}
