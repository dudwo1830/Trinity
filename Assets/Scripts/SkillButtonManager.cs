using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonManager : MonoBehaviour
{
    public static SkillButtonManager Instance { get; set; }

    public Button buttonPrefab;
    private Button[] buttons;
    public Transform buttonContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Instance is already");
            Destroy(gameObject);
        }

        buttons = GetComponentsInChildren<Button>();
    }

    public void CreateSkillButton(SkillData data)
    {
        var button = Instantiate(buttonPrefab, buttonContainer);
        foreach(var item in button.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch (item.gameObject.name)
            {
                case "Name":
                    item.text = data.Name;
                    break;
                case "Description":
                    item.text = data.Description;
                    break;
                case "Summary":
                    item.text = $"{data.Attribute}\n{data.Type} / {data.Amount}";
                    break;
            }
        }
        button.onClick.AddListener(() => {
            BattleSystem.Instance.OnSkillButton(data);
        });
    }
}
