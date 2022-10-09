/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SW.Logger;
using UnityEngine;
using SW.UI;
using SW.Android;

public sealed class SplashPanel : MonoBehaviour
{
    [SerializeField]
    private float minDelay = 5.0f;

    private void Awake()
    {
        StartCoroutine(Init(() => GameManager.Instance.LoadScene(SceneRepo.Main)));
    }

    private IEnumerator Init(Action onComlete)
    {
        Stopwatch watch = Stopwatch.StartNew();

        // Init all necessary managers etc
        AndroidNotifications.InitializeAndroidNotificationCenter();

        watch.Stop();
        float initDelay = watch.ElapsedMilliseconds * 0.001f;

        float diff = 0.0f;
        if (initDelay < minDelay)
        {
            diff = minDelay - initDelay;
        }

        yield return new WaitForSeconds(diff);

        onComlete.Invoke();
    }
}
