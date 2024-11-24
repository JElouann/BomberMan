using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool HasToUsePriorityColor;
    public Color PriorityColor;
    public AStarTheOneAndOnly ParentGraph;
    public List<Node> Neighbours;

    // the node that opened this one
    public Node PrecedentNode;

    public int Rank;

    // base si pas touché, open ou closed
    public NodeStateEnum State;

    public bool IsFiller; //

    private void Start()
    {
        if (IsFiller) return;
        ParentGraph.Nodes.Add(this);
        StartCoroutine(CheckNeighbours());
    }


    public IEnumerator CheckNeighbours()
    {
        yield return new WaitForSeconds(0);
        int layer_mask = LayerMask.GetMask("Node");
        TryGetComponent(out Collider2D ownCollider);

        RaycastHit2D topHit = Physics2D.Raycast(this.transform.position, Vector2.up, 1.5f, layer_mask);
        if (topHit.collider != null)
        {
            if (topHit.collider.isTrigger && topHit.collider != ownCollider)
            {
                topHit.collider.TryGetComponent(out Node node);
                this.Neighbours.Add(node);
            }
        }

        RaycastHit2D botHit = Physics2D.Raycast(this.transform.position, Vector2.down, 1.5f, layer_mask);
        if(botHit.collider != null)
        {
            if (botHit.collider.isTrigger && botHit.collider != ownCollider)
            {
                botHit.collider.TryGetComponent(out Node node);
                this.Neighbours.Add(node);
            }
        }

        RaycastHit2D leftHit = Physics2D.Raycast(this.transform.position, Vector2.left, 1.5f, layer_mask);
        if(leftHit.collider != null)
        {
            if (leftHit.collider.isTrigger && leftHit.collider != ownCollider)
            {
                leftHit.collider.TryGetComponent(out Node node);
                this.Neighbours.Add(node);
            }
        }
        
        RaycastHit2D rightHit = Physics2D.Raycast(this.transform.position, Vector2.right, 1.5f, layer_mask);
        if(rightHit.collider != null)
        {
            if (rightHit.collider.isTrigger && rightHit.collider != ownCollider)
            {
                rightHit.collider.TryGetComponent(out Node node);
                this.Neighbours.Add(node);
            }
        }
    }

    public float GetF()
    {
        // récupérer le g pour obtenir le f
        return GetDistanceFromTarget(ParentGraph.End) + Rank;
    }

    private float GetDistanceFromTarget(Node target)
    {
        return Vector3.Distance(this.transform.position, target.transform.position);
    }

    #region Utilities
    private void OnDrawGizmos()
    {
        if(IsFiller | ParentGraph == null) return;

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(this.transform.position, Vector2.up);
        //Gizmos.DrawRay(this.transform.position, Vector2.down);
        //Gizmos.DrawRay(this.transform.position, Vector2.left);
        //Gizmos.DrawRay(this.transform.position, Vector2.right);

        if (ParentGraph.End == this)
        {
            Gizmos.color = Color.red;
        }
        else if (ParentGraph.Start == this)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = ColorChange();
        }

        if (HasToUsePriorityColor) Gizmos.color = PriorityColor;

        Gizmos.DrawSphere(this.transform.position, 0.5f);
        foreach (Node node in Neighbours)
        {
            if (node == null | ParentGraph.DrawLines == false) break;
            Gizmos.color = ParentGraph.LineColor;
            Gizmos.DrawLine(this.transform.position, node.transform.position);
        }
    }

    private void OnValidate()
    {
        foreach(Node node in Neighbours)
        {
            if (node == null | node.Neighbours.Contains(this))
            {
                break;
            }
            node.Neighbours.Add(this);
        }
    }
    #endregion
     
    public void Open(Node opener, List<Node> whichPath)
    {
        // opener = the node that opened this
        PrecedentNode = opener;
        State = NodeStateEnum.Open;
        
        Rank = whichPath.IndexOf(this);
    }

    public void Close()
    {
        State = NodeStateEnum.Closed;
    }

    public Color ColorChange()
    {
        switch(State)
        {
            case NodeStateEnum.Closed:
                return ParentGraph.ClosedNeighbour;
            case NodeStateEnum.Open:
                return ParentGraph.OpenedNeighbour;

            default:
                return ParentGraph.BaseColor;
        }
    }
}