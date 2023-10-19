using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameoverText : MonoBehaviour
{
    private void Start()
    {
        var stringTable = DataTableManager.GetTable<StringTable>();
        var win = transform.Find("Win");
        win.Find("Title").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("WinTitle");
        win.Find("Title").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("WinSub");

        win.Find("NextBattleButton").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("NextBattleTitle");
        win.Find("NextBattleButton").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("NextBattleSub");

        win.Find("AddCardButton").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("AddCardTitle");
        win.Find("AddCardButton").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("AddCardSub"); 
        
        win.Find("OnHealButton").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("OnHealTitle");
        win.Find("OnHealButton").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("OnHealSub");

        win.Find("EnforceCardButton").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("EnforceCardTitle");
        win.Find("EnforceCardButton").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("EnforceCardSub");

        win.Find("DeleteCardButton").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("DeleteCardTitle");
        win.Find("DeleteCardButton").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("DeleteCardSub");

        var lose = transform.Find("Lose");
        lose.Find("Title").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("WinTitle");
        lose.Find("Title").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("WinSub");

        lose.Find("RestartButton").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("RestartTitle");
        lose.Find("RestartButton").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("RestartSub");

        lose.Find("QuitButton").Find("Title").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("QuitTitle");
        lose.Find("QuitButton").Find("Sub").gameObject.GetComponent<TextMeshProUGUI>().text = stringTable.GetString("QuitSub");
    }
}
