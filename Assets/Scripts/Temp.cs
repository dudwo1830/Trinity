using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        var str = DataTableManager.GetTable<StringTable>().GetString("YOU DIE");
        Debug.Log(str);
    }

    void Update()
    {
        
    }
}
