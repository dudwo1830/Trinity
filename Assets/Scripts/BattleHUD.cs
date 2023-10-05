using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Slider enemyHealthSlider;

    private Button[] buttons;

    private void Start()
    {
        gameObject.SetActive(false);
        buttons = gameObject.GetComponents<Button>();
    }

    public void SetEnemyHp(LivingEntity enemy)
    {
        enemyHealthSlider.minValue = 0f;
        enemyHealthSlider.maxValue = enemy.startingHealth;
        enemyHealthSlider.value = enemy.Health;
    }

    public void SetSkill(List<TempSkill> list)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            button.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = list[i].name;
            button.onClick.AddListener(() => {
                //if (BattleSystem.state != BattleState.PLAYERTURN)
                //{
                //    return;
                //}

                //playerAction = tempSkills[0];
                //Debug.Log($"PlayerTurn: Player is {playerAction.name}");
                //Battle();
            });
        }
    }
}
