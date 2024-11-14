using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    public Vector3 pos;

    [SerializeField] private float _speed;

    private Rigidbody2D _rb;
    private PlayerAnimation _animation;

    // the node we're currently overlapping
    private Node _currentNode;
    // the node which we're currently going toward
    public Node _targetNode;

    // the stack from which we pick the target node
    private Stack<Node> _path;

    // quand position = target position
        // (fait avec collider)

    // target position = target position suivante dans Stack de node



    private void Start()
    {
        _rb = TryGetComponent(out Rigidbody2D rb) ? rb : null;
        _animation = TryGetComponent(out PlayerAnimation animation) ? animation : null;
        UpdatePath();
    }

    private void UpdatePath()
    { 
        _path = Graph.Instance.GetPath(_currentNode, _targetNode);
    }

    private void FixedUpdate()
    {
        Vector3 dir = _targetNode.transform.position - this.transform.position;

        _rb.velocity = dir.normalized * _speed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Node node))
        {
            _currentNode = node;
            if (node != _targetNode) return;
            _targetNode = _path.Pop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Node node))
        {
            _currentNode = null;
        }
    }
}
