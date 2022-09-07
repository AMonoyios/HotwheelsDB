using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This ideally needs to be moved to a server.
/// </summary>
public class GameManagerDataConteroller : MonoBehaviour
{
    [SerializeField]
    private bool forceDevMode = false;

    private void Awake()
    {
        GameManager.ForceDevMode(forceDevMode);
    }
}
