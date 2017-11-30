using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager.instance.playerFoodPoints;
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    private void Update()
    {
        
    }

    protected override void OnCantMove<T>(T Component)
    {
        //throw new System.NotImplementedException();
    }
}
