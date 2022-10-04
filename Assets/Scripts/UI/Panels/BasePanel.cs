/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [Header("Base Panel properties")]
    [SerializeField]
    private bool allowMultipleInstances;
    public bool AllowMultipleInstances { get { return allowMultipleInstances; }}
}
