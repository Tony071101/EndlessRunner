using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterItemUI : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI buyBtnTxt;
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button selectBtn;

    private CharacterInfo characterInfo;
    private CharacterSaveData characterSaveData;

    public void SetUp(CharacterInfo info, CharacterSaveData saveData) {
        characterInfo = info;
        characterSaveData = saveData;

        nameTxt.text = info.displayName;
        buyBtnTxt.text = info.price.ToString();
        avatarImage.sprite = info.avatar;
        
        UpdateUI();
    }

    public void UpdateUI() {
        buyBtn.gameObject.SetActive(!characterSaveData.isUnlocked);
        selectBtn.gameObject.SetActive(characterSaveData.isUnlocked);
    }

    public void OnBuyClicked() {
        if(CharacterManager.Instance.UnlockCharacter(characterInfo.characterId)) {
            UpdateUI();
        }
    }

    public void OnSelectClicked() {
        CharacterManager.Instance.EquipCharacter(characterInfo.characterId);
        UpdateUI();
        GameObject playerRoot = GameObject.FindGameObjectWithTag("Player");
        if (playerRoot == null)
        {
            Debug.LogError("Player not found in scene!");
            return;
        }
        List<GameObject> childrenToDestroy = new List<GameObject>();
        foreach (Transform child in playerRoot.transform)
        {
            if (child.name != "Feet")
            {
                childrenToDestroy.Add(child.gameObject);
            }
        }
        foreach (var childGO in childrenToDestroy)
        {
            Destroy(childGO);
        }
        if (characterInfo.prefab != null)
        {
            GameObject newChar = Instantiate(characterInfo.prefab, playerRoot.transform);
            newChar.transform.localPosition = Vector3.zero;

            playerRoot.GetComponent<MonoBehaviour>().StartCoroutine(RefreshAfterFrame(playerRoot));
        }
        else
        {
            Debug.LogError("CharacterInfo prefab is null!");
        }
    }

    private IEnumerator RefreshAfterFrame(GameObject playerRoot)
    {
        yield return null;  // đợi một frame

        PlayerMovement pm = playerRoot.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.RefreshCharacterReferences();
        }
    }
}
