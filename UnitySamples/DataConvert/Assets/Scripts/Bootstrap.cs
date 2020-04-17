using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        LocalGameData lgd = new LocalGameData();
        lgd.LoadData();
    }
}
