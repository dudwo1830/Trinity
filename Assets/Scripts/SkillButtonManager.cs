using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonManager : MonoBehaviour
{
    public static SkillButtonManager Instance { get; set; }

    public Image curtain;
    public Button buttonPrefab;
    public Transform skillButtonTransform;
    public Transform upgradeButtonTransform;

    private List<Button> skillButtons = new List<Button>();
    private List<Button> upgradeButtons = new List<Button>();
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

        upgradeButtonTransform.gameObject.SetActive(false);
        curtain.gameObject.SetActive(false);
    }

    public void CreateSkillButton(SkillData data)
    {
        var button = Instantiate(buttonPrefab, skillButtonTransform);
        button.gameObject.name = data.Name;
        foreach(var item in button.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch (item.gameObject.name)
            {
                case "Name":
                    item.text = $"{data.Name} +{data.Level}";
                    break;
                case "Description":
                    item.text = data.GetDescription();
                    break;
                case "Summary":
                    item.text = $"{data.Attribute}\n{data.Type} / {data.Amount}";
                    break;
            }
        }
        button.onClick.AddListener(() => 
        {
            BattleSystem.Instance.OnSkillButton(data);
        });

        skillButtons.Add(button);
    }

    public void CreateUpgradeButton(SkillData data)
    {
        var button = Instantiate(buttonPrefab, upgradeButtonTransform);
        button.gameObject.name = data.Name;
        foreach (var item in button.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch(item.gameObject.name)
            {
                case "Name":
                    item.text = $"{data.Name} <color=green>+{data.Level + 1}</color>";
                    break;
                case "Description":
                    item.text = data.GetDescription();
                    break;
                case "Summary":
                    item.text = $"{data.Amount} <color=green>+{data.UpgradeAmount}</color>";
                    break;
            }
        }
        button.onClick.AddListener(() =>
        {
            data.LevelUp();
            UpdateButton();
            curtain.gameObject.SetActive(false);
            upgradeButtonTransform.gameObject.SetActive(false);
            BattleSystem.Instance.PlayerUpgrade();
        });
        upgradeButtons.Add(button);
    }

    public void UpdateButton()
    {
        var skillTable = DataTableManager.GetTable<SkillTable>();
        foreach (var button in skillButtons)
        {
            var data = skillTable.GetSkill(button.name);
            foreach (var item in button.GetComponentsInChildren<TextMeshProUGUI>())
            {
                switch (item.gameObject.name)
                {
                    case "Name":
                        item.text = $"{data.Name} +{data.Level}";
                        break;
                    case "Description":
                        item.text = data.GetDescription();
                        break;
                    case "Summary":
                        item.text = $"{data.Attribute}\n{data.Type} / {data.Amount}";
                        break;
                }
            }
        }
        foreach (var button in upgradeButtons)
        {
            var data = skillTable.GetSkill(button.name);
            foreach (var item in button.GetComponentsInChildren<TextMeshProUGUI>())
            {
                switch (item.gameObject.name)
                {
                    case "Name":
                        item.text = $"{data.Name} <color=green>+{data.Level + 1}</color>";
                        break;
                    case "Description":
                        item.text = data.GetDescription();
                        break;
                    case "Summary":
                        item.text = $"{data.Amount} <color=green>+{data.UpgradeAmount}</color>";
                        break;
                }
            }
        }
    }

    public void SelectUpgrade()
    {
        curtain.gameObject.SetActive(true);
        upgradeButtonTransform.gameObject.SetActive(true);
    }

    public void ResetAllSkill()
    {
        var skillTable = DataTableManager.GetTable<SkillTable>();
        skillTable.ResetAllSkill();
        UpdateButton();
    }
}
