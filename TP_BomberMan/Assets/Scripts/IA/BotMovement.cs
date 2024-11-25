using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rb;
    private CircleCollider2D _collider;
    private PlayerAnimation _animation;

    private PlayerHold _hold;

    // the node we're currently overlapping
    public Node _currentNode;

    public Node PushToTarget;

    // the node which we're currently going toward
    public Node _targetNode;

    // the stack from which we pick the target node
    private Stack<Node> _path = new();
    private List<Node> path;

    private void Start()
    {
        _rb = TryGetComponent(out Rigidbody2D rb) ? rb : null;
        _collider = TryGetComponent(out CircleCollider2D collider) ? collider : null;
        _hold = TryGetComponent(out PlayerHold hold) ? hold : null;
        _animation = TryGetComponent(out PlayerAnimation animation) ? animation : null;
    }

    private void Update()
    {
        //path = AStarTheOneAndOnly.Instance.Path;
    }

    public void UpdatePath()
    {
        _path.Clear();

        foreach (Node node in _path)
        {
            //node.HasToUsePriorityColor = true;
        }
        _targetNode = _path.Pop();
    }

    //private void FixedUpdate()
    //{
    //    if (_targetNode == null) return;
    //    if (_path.Count == 0 && _currentNode == _targetNode)
    //    {
    //        print("arrivé");
    //        _targetNode = null;
    //        _rb.velocity = Vector2.zero;
    //        return;
    //    }
    //    else if (/*_currentNode == _targetNode*/ (_targetNode.transform.position - _currentNode.transform.position).magnitude <= 0.4f)
    //    {
    //        _targetNode = _path.Pop();
    //    }
    //    else
    //    {
    //        Vector3 dir = _targetNode.transform.position - this.transform.position;
    //        Vector3 velo = dir.normalized * Time.deltaTime * 4;
    //        this.transform.Translate(velo);
    //        //_rb.velocity = dir.normalized * Time.deltaTime * _speed;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Node node))
        {
            AStarTheOneAndOnly.Instance.FindPath(node, AStarTheOneAndOnly.Instance.End);
            AStarTheOneAndOnly.Instance.FindPath(node, AStarTheOneAndOnly.Instance.End);
            //AStarTheOneAndOnly.Instance.Start = _currentNode;
            //if (node == _targetNode && _path.Count > 0)
            //{
            //    print("AAAAAAAAAA");
            //    _targetNode.HasToUsePriorityColor = false;
            //    _targetNode = _path.Pop();
            //    _targetNode.HasToUsePriorityColor = true;
            //}
            //else if (node == _targetNode && _path.Count == 0)
            //{
            //    print("arrivé");
            //    _targetNode = null;
            //    _rb.velocity = Vector3.zero;
            //}
        }
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
