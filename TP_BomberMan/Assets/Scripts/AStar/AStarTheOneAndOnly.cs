using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStarTheOneAndOnly : MonoBehaviour
{
    [Header("Pathfinding setup")]
    public Node Start;
    public Node End;

    [Header("Every Nodes in the graph")]
    private List<List<Node>> _everyPathes = new();
    public List<Node> OpenedNodes = new();
    public List<Node> ClosedNodes = new();

    #region Visual

    // Colors used of gizmos in Editor
    [Header("Visual")]
    public Color BaseColor;
    public Color ClosedNeighbour;
    public Color OpenedNeighbour;
    public Color SelectedPathStep;
    public Color LineColor;

    public bool DrawLines;
    #endregion

    public List<Node> Nodes;
    private Node _currentNode;
    private List<Node> _receptaclePath = new();
    
    public int NumOfIteration;

    public List<Node> Path;

    // Singleton
    #region Singleton
    private static AStarTheOneAndOnly _instance;

    public static AStarTheOneAndOnly Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("AStar");
                _instance = go.AddComponent<AStarTheOneAndOnly>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            Debug.Log($"<b><color=#{Random.ColorHSV(0f, 1f, 1f, 1f, 0f, 0f, 1f, 1f).ToHexString()}>{this.GetType()}</color> instance <color=#eb624d>destroyed</color></b>");
        }
        else
        {
            _instance = this;
            Debug.Log($"<b><color=#{Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f, 1f, 1f).ToHexString()}>{this.GetType()}</color> instance <color=#58ed7d>created</color></b>");
        }
    }
    #endregion

    public void FindPath(Node startNode, Node targetNode)
    {
        foreach(Node node in Nodes)
        {
            node.gCost = 0;
            node.hCost = 0;
        }
        List<Node> openNodes = new List<Node>();
        List<Node> closeNodes = new List<Node>();
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].fCost < currentNode.fCost || openNodes[i].fCost == currentNode.fCost && openNodes[i].hCost < currentNode.hCost)
                {
                    currentNode = openNodes[i];
                }
            }

            openNodes.Remove(currentNode);
            closeNodes.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbour in currentNode.Neighbours)
            {
                if (closeNodes.Contains(neighbour))
                {
                    continue;
                }

                float newMovementCostToNeighbour = currentNode.gCost + /*GetStepNumber(neighbour)*/1; //
                if (newMovementCostToNeighbour < currentNode.gCost || !openNodes.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = Vector3.Distance(neighbour.transform.position, targetNode.transform.position);
                    neighbour.PrecedentNode = currentNode;

                    if (!openNodes.Contains(neighbour))
                    {
                        openNodes.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode.HasToUsePriorityColor = true;
            currentNode = currentNode.PrecedentNode;
        }
        path.Reverse();

        Path = path;
    }
}
