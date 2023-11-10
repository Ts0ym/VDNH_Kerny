using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DelayExtention
{
    public static void DelayAction(this MonoBehaviour monoBeh, float delay, Action action)
    {
        monoBeh.StartCoroutine(DelayCoroutine(delay, action));
    }

    public static IEnumerator DelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
