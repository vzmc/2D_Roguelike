using UnityEngine;

public class Wall : MonoBehaviour{    public Sprite dmgSprite;
    public int hp = 3;

    private SpriteRenderer spriteRenderer;

    private void Awake()    {        spriteRenderer = GetComponent<SpriteRenderer>();    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="loss"></param>
    public void DamageWall(int loss)    {        spriteRenderer.sprite = dmgSprite;  //ダメージスプライトに差し替え
        hp -= loss;        if (hp <= 0)
        {
            gameObject.SetActive(false);    //Wallスプライトの表示を消す
        }
    }}