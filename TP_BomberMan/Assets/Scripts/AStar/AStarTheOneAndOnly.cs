using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AStarTheOneAndOnly : MonoBehaviour
{
    [Header("Pathfinding setup")]
    public Node Start;
    public Node End;

    [Header("Every Nodes in the graph")]
    private List<List<Node>> _everyPathes = new();
    public List<Node> _openedNodes = new();

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

    private void Reset()
    {
        foreach(Node node in Nodes)
        {
            node.State = NodeStateEnum.Base;
        }
        _openedNodes.Clear();
        _receptaclePath.Clear();
        _everyPathes.Clear();
    }


    public async void Travel()
    {
        Reset();
        ProcessNode(Start);
        _receptaclePath.Add(Start);
        AddPathes(Start);
        while (!_openedNodes.Contains(End))
        {
            // reset at each iteration
            List<Node> path = new();
            path = _receptaclePath;

            _receptaclePath = GetLowerFPath();
            
            _currentNode = _receptaclePath[_receptaclePath.Count - 1];
            // ferme ce noeud et ouvre ses voisins
            ProcessNode(_currentNode);
            // crée chemin avec son noeud le moins f en dernier
            AddPathes(_currentNode);
            await Task.Delay(600);
        }
    }

    private void ProcessNode(Node node)
    {
        node.Close();
        foreach (Node neighbour in node.Neighbours)
        {
            if (neighbour == End)
            {
                print("DETECTED END");
                _openedNodes.Add(neighbour);
            }
            if (neighbour.State != NodeStateEnum.Closed)
            {
                neighbour.Open(node, _receptaclePath);
                _openedNodes.Add(neighbour);
            }
        }
    }

    private void AddPathes(Node forWhichNode)
    {
        Node bestNeighbour = GetLowerFNode(forWhichNode.Neighbours);
        _receptaclePath.Add(bestNeighbour);
        _everyPathes.Add(_receptaclePath);
    }

    private Node GetLowerFNode(List<Node> nodes)
    {
        Node bestFNode = nodes[0];
        foreach(Node node in nodes)
        {
            if(bestFNode == null)
            {
                bestFNode = node;
            }
            else if(node.GetF() < bestFNode.GetF())
            {
                bestFNode = node;
            }
        }
        return bestFNode;
    }

    private List<Node> GetLowerFPath(List<List<Node>> ignoredPathes = null)
    {
        // initialize lists
        List<Node> bestPath = new();
        bestPath = _receptaclePath;
        List<List<Node>> equalBestPathes = new();

        // iterate over every pathes to get lower pathes
        foreach(List<Node> path in _everyPathes)
        {
            // cancel current iteration if path has to be ignored
            if(ignoredPathes != null)
            {
                if (ignoredPathes.Contains(path)) break;
            }

            // or switch the current receptacle path if his f is bigger than current iteration
            else if (path[path.Count - 1].GetF() < bestPath[bestPath.Count - 1].GetF())
            {
                print($"best path changed from {bestPath} to {path}");
                bestPath = path;
            }
        }
        
        return bestPath;
    }
}
