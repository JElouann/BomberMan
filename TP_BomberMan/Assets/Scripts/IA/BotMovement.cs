using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private BotAnimation _animation;

    private PlayerHold _hold;

    List<Node> path;

    // the node we're currently overlapping
    public Node CurrentNode;

    // the node which we're currently going toward
    public Node TargetNode;

    private float _previousDirX;

    private void Start()
    {
        _hold = TryGetComponent(out PlayerHold hold) ? hold : null;
        _animation = TryGetComponent(out BotAnimation animation) ? animation : null;
    }

    private void Update()
    {
        if (TargetNode == null | AStarTheOneAndOnly.Instance.Path.Count <= 0) return;
        Vector3 dir = AStarTheOneAndOnly.Instance.Path[0].transform.position - this.transform.position;
        Vector3 velo = dir.normalized * Time.deltaTime * _speed;
        this.transform.Translate(velo);

        if (dir.x != 0 && Mathf.Sign(dir.normalized.x) != Mathf.Sign(_previousDirX))
        {
            _previousDirX = Mathf.Sign(dir.normalized.x);
            _animation.FlipSpriteX();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Node node))
        {
            CurrentNode = node;
            
            if(CurrentNode == TargetNode)
            {
                print("arrivé");
                TargetNode = null;
                return;
            }
            AStarTheOneAndOnly.Instance.FindPath(CurrentNode, TargetNode);
        }
    }

    public void SetTarget(Node newTarget)
    {
        TargetNode = newTarget;
        AStarTheOneAndOnly.Instance.FindPath(CurrentNode, TargetNode);
    }

    //// detects if bot is blocked by obstacle
    //// if it's the case, has to look for a bomb to destroy it
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.layer == 6 && _rb.velocity == Vector2.zero)
    //    {
    //        if (_hold.BombSlot.transform.childCount <= 0)
    //        {
    //            print("besoin d'une bombe");
    //            GetToClosestBomb();
    //        }
    //        else
    //        {
    //            _hold.DropBomb();
    //        }
    //    }
    //}

    //private void GetToClosestBomb()
    //{
    //    List<BombBehaviour> bombs = BombManager.Instance.OnFieldBombs;
    //    BombBehaviour targetBomb = bombs[0];
    //    foreach (BombBehaviour bomb in bombs)
    //    {
    //        if (Vector3.Distance(bomb.transform.position, this.transform.position) < Vector3.Distance(targetBomb.transform.position, this.transform.position)) targetBomb = bomb;
    //    }
    //    //_targetNode = Graph.Instance.Nodes[(Graph.Instance.Nodes.Count - 1) / 2];
    //    PushToTarget = BombManager.Instance.GetRandomBomb().Node;
    //    UpdatePath();
    //}
}
