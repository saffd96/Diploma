using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private int spritePerFrame = 6;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool destroyOnEnd = false;

    private int index = 0;
    private Image image;
    private int frame = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (!loop && index == sprites.Length) return;

        frame++;

        if (frame < spritePerFrame) return;

        image.sprite = sprites[index];
        frame = 0;
        index++;

        if (index < sprites.Length) return;

        if (loop) index = 0;
        if (destroyOnEnd) Destroy(gameObject);
    }
}
