using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineEx : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(CoEx());
    }

    IEnumerator CoEx()
    {
        Debug.Log("Start Coroutine");
        yield return new WaitForSeconds(1f);
        Debug.Log("Wait One Frame");
        yield return null;
        Debug.Log("End Coroutine");
    }
}
