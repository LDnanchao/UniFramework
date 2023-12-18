using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LubanKit : MonoBehaviour
{
    public static cfg.Tables lubanTables
    {
        get=>LubanManager.Instance.lubanTables;
    }
}
