using System;
using UnityEngine;

public class ExitLvl : MonoBehaviour
{
    public static event Action OnExitLvlCollision;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(Tags.Player)) return;

        OnExitLvlCollision?.Invoke();
    }
}
