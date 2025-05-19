using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "Character/Info", order = 0)]
public class CharacterInfo : ScriptableObject
{
    public string characterId;
    public string displayName;
    public Sprite avatar;
    public int price;
    public GameObject prefab;
}
