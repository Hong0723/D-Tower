using UnityEngine;

public class EnemyDirection : MonoBehaviour
{
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void LookUp()
    {
        sr.sprite = upSprite;
    }

    public void LookDown()
    {
        sr.sprite = downSprite;
    }

    public void LookLeft()
    {
        sr.sprite = leftSprite;
    }

    public void LookRight()
    {
        sr.sprite = rightSprite;
    }
}
