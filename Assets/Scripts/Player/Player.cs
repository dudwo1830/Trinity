using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerUI;

    private void Awake()
    {
        playerUI.SetActive(false);
    }
}
