using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    
    [Header("All character infos (ScriptableObjects)")]
    public List<CharacterInfo> allCharacters;

    public List<CharacterSaveData> saveDataList { get; private set; }
    private GameManager gameManager;

    private void Awake() {
        if(Instance == null) Instance = this;  
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void LoadFromGameData(Data data)
    {
        saveDataList = data.characterStates ?? new List<CharacterSaveData>();
    }

    public CharacterInfo GetEquippedCharacter()
    {
        var equippedId = saveDataList.FirstOrDefault(c => c.isEquipped)?.characterId;
        return allCharacters.Find(c => c.characterId == equippedId);
    }

    public bool UnlockCharacter(string characterId) {
        var character = allCharacters.Find(c => c.characterId == characterId);
        var saveData = saveDataList.Find(c => c.characterId == characterId);

        if(character != null && saveData != null && !saveData.isUnlocked && gameManager.Data.totalRings >= character.price) {
            gameManager.Data.totalRings -= character.price;
            saveData.isUnlocked = true;
            return true;
        }

        return false;
    }

    public void EquipCharacter(string characterId) {
        foreach(var c in saveDataList) c.isEquipped = false;
        var saveData = saveDataList.Find(c => c.characterId == characterId);
        if(saveData != null && saveData.isUnlocked) {
            saveData.isEquipped = true;
        }
    }

    public List<CharacterInfo> GetAllCharacters() => allCharacters;
}
