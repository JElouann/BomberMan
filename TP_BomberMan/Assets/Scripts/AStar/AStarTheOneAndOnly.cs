using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

                //float newMovementCostToNeighbour = currentNode.gCost + Vector3.Distance(currentNode.transform.position, neighbour.transform.position);

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

    #region old version

    //private void Reset()
    //{
    //    foreach(Node node in Nodes)
    //    {
    //        node.State = NodeStateEnum.Base;
    //    }
    //    OpenedNodes.Clear();
    //    _receptaclePath.Clear();
    //    _everyPathes.Clear();
    //}

    //public int GetStepNumber(Node node)
    //{
    //    int stepNumber = 1;
    //    Node checker = node;
    //    while (checker.PrecedentNode != null)
    //    {
    //        stepNumber++;
    //        checker = checker.PrecedentNode;
    //    }
    //    return stepNumber;
    //}

    //public async void Travel()
    //{
    //    Reset();
    //    ProcessNode(Start);
    //    _receptaclePath.Add(Start);
    //    AddPathes(Start);

    //    while (!OpenedNodes.Contains(End))
    //    {
    //        // obtenir les noeuds pas ouverts pour les ignorer après
    //        List<List<Node>> notOpen = new();
    //        foreach(List<Node> path in _everyPathes)
    //        {
    //            if (path[path.Count - 1].State != NodeStateEnum.Open)
    //            {
    //                notOpen.Add(path);
    //            }   
    //        }

    //        _receptaclePath = GetLowerFPath(notOpen);
    //        foreach(Node node in _receptaclePath)
    //        {
    //            print(node);
    //        }

    //        _currentNode = _receptaclePath[_receptaclePath.Count - 1];


    //        // ferme ce noeud et ouvre ses voisins
    //        ProcessNode(_currentNode);

    //        // crée chemin avec son noeud le moins f en dernier
    //        AddPathes(_currentNode);

    //        await Task.Delay(600);
    //    }
    //}

    //private void ProcessNode(Node node)
    //{
    //    node.Close();
    //    foreach (Node neighbour in node.Neighbours)
    //    {
    //        if (neighbour == End)
    //        {
    //            print("DETECTED END");
    //            OpenedNodes.Add(neighbour);
    //        }
    //        if (neighbour.State != NodeStateEnum.Closed)
    //        {
    //            neighbour.Open(node, _receptaclePath);
    //        }
    //    }
    //}

    //private void AddPathes(Node forWhichNode)
    //{
    //    Node bestNeighbour = GetLowerFNode(forWhichNode.Neighbours);
    //    _receptaclePath.Add(bestNeighbour);

    //    //_receptaclePath.Add(forWhichNode);
    //    _everyPathes.Add(_receptaclePath);
    //}

    //private Node GetLowerFNode(List<Node> nodes)
    //{
    //    Node bestFNode = nodes[0];
    //    foreach(Node node in nodes)
    //    {
    //        if(bestFNode == null)
    //        {
    //            bestFNode = node;
    //        }
    //        else if(node.GetF() < bestFNode.GetF())
    //        {
    //            bestFNode = node;
    //        }
    //    }
    //    return bestFNode;
    //}

    //private List<Node> GetLowerFPath(List<List<Node>> ignoredPathes = null)
    //{
    //    // initialize lists
    //    List<Node> bestPath = new();
    //    List<List<Node>> equalBestPathes = new();

    //    // iterate over every pathes to get lower pathes
    //    foreach (List<Node> path in _everyPathes)
    //    {
    //        // cancel current iteration if path has to be ignored
    //        if (ignoredPathes != null)
    //        {
    //            if (ignoredPathes.Contains(path)) break;
    //        }
    //        else if (bestPath == null)
    //        {
    //            bestPath = path;
    //        }
    //        // or switch the current receptacle path if his f is bigger than current iteration
    //        else if (path[path.Count - 1].GetF() < bestPath[bestPath.Count - 1].GetF())
    //        {
    //            print($"best path changed from {bestPath} to {path}");
    //            bestPath = path;
    //        }
    //    }

    //    List<List<Node>> equalBest = new();
    //    foreach (List<Node> path in _everyPathes)
    //    {
    //        if(path == bestPath)
    //        {
    //            equalBest.Add(path);
    //        }
    //    }

    //    if (equalBestPathes.Count > 1)
    //    {
    //        bestPath = equalBest[Random.Range(0, equalBestPathes.Count)];
    //    }

    //    print("best path is " +  bestPath + " with " + bestPath[bestPath.Count - 1].GetF());
    //    return bestPath;
    //}

    #endregion
}
