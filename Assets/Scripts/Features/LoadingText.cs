using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    [SerializeField] private float changeRate;
    private float timer;
    private int dotCount;
    private Text text;
    private string dots;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > changeRate)
        {
            ChangeLoadingText();
            dotCount++;

            if (dotCount == 4)
            {
                dotCount = 0;
            }
            timer = 0;
        }

    }

    private void ChangeLoadingText()
    {
        dots = String.Concat(Enumerable.Repeat(".", dotCount));
        text.text = $"Loading{dots}";
    }
}
