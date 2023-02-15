using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeItem{
    Characters,
    Weapon
}

public class Shop : MonoBehaviour
{
    public Transform CharactersContainer;
    public Transform WeaponsContainer;

    private void Awake() {
        PlayerData playerData = PlayerData.Load();
        List<int> charactersOwned = playerData.characters;
        int selectedCharacter = playerData.selectedCharacter;
        List<int> weaponsOwned = playerData.weapons;
        int selectedWeapon = playerData.selectedWeapon;

    }
}
