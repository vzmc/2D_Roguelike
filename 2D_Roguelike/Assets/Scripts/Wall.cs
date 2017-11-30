﻿using UnityEngine;

public class Wall : MonoBehaviour
    public int hp = 3;

    private SpriteRenderer spriteRenderer;

    private void Awake()

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="loss"></param>
    public void DamageWall(int loss)
        hp -= loss;
        {
            gameObject.SetActive(false);    //Wallスプライトの表示を消す
        }
    }