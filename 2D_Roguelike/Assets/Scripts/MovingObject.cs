using System.Collections;
using UnityEngine;

/// <summary>
/// 移動する物体の基礎クラス
/// </summary>
public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    private float inverseMoveTime;      // moveTimeを計算するための変数

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        inverseMoveTime = 1f / moveTime;
    }

    /// <summary>
    /// 移動可能かを判断するメソッド　可能な場合はSmoothMovementへ移行する
    /// </summary>
    /// <param name="xDir">x方向移動量</param>
    /// <param name="yDir">ｙ方向移動量</param>
    /// <param name="hit">衝突判定情報</param>
    /// <returns></returns>
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;             // 現在地
        Vector2 end = start + new Vector2(xDir, yDir);  // 移動先

        //自身のColliderを無効にし、Linecastで自分自身を判定しないようにする
        boxCollider.enabled = false;

        //現在地と目的地との間にblockingLayerのついたオブジェクトが無いか判定する
        hit = Physics2D.Linecast(start, end, blockingLayer);

        //Colliderを有効に戻します。
        boxCollider.enabled = true;

        //何も無ければSmoothMovementメソッドを呼んでオブジェクトの移動処理する
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMoveMent(end));
            return true;
        }

        return false;
    }

    /// <summary>
    /// スムーズに目的地へ移動するCoroutine
    /// </summary>
    /// <param name="end">目的地</param>
    /// <returns>Coroutine</returns>
    protected IEnumerator SmoothMoveMent(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    /// <summary>
    /// 移動を試みる
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponet = hit.transform.GetComponent<T>();

        if (!canMove && hitComponet != null)
        {
            OnCantMove(hitComponet);
        }
    }

    /// <summary>
    /// 移動できない時の処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Component"></param>
    protected abstract void OnCantMove<T>(T Component) where T : Component;
}