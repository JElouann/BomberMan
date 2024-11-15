using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private bool _finished;

    #region Visual

    // Colors used of gizmos in Editor
    [Header("Gizmos")]
    public Color BaseColor;
    public Color ClosedNeighbour;
    public Color OpenedNeighbour;
    public Color AnalyzedPathStepColor;
    public Color SelectedPathStep;
    public Color LineColor;

    public bool DrawLines;
    #endregion

    // Singleton
    #region Singleton
    private static Graph _instance;

    public static Graph Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Graph");
                _instance = go.AddComponent<Graph>();
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

    // Liste de tous les nodes
    public List<Node> Nodes;

    // Node de départ / arrivée
    public Node StartNode;
    public Node EndNode;

    // Current observed node
    public Node CurrentNode;

    public int NumOfIteration;

    public List<Node> OpenedNeighbours;
    public List<float> NeighboursFValues;

    public int GetStepNumberOfNode(List<Node> path, Node node)
    {
        return path.IndexOf(node);
    }

    public Stack<Node> GetPath(Node start, Node end)
    {
        NumOfIteration = 0;
        StartNode = start;
        EndNode = end;

        Stack<Node> path = new Stack<Node>();
        
        foreach(Node node in Travel())
        {
            path.Push(node);
        }

        return path;
    }

    /// <summary>
    /// Return a list of nodes that represents the travel from the Start node to the End node.
    /// </summary>
    /// <returns></returns>
    public List<Node> Travel()
    {
        // initialiser le return
        List<Node> path = new List<Node>();

        // initialiser la méthode
        ResetNodes();
        OpenedNeighbours.Clear();
        NeighboursFValues.Clear();
        CurrentNode = StartNode;
        OpenedNeighbours.Add(CurrentNode);

        while (!_finished)
        {
            NumOfIteration++;
            ProcessNode(CurrentNode);
            CurrentNode = OpenedNeighbours[NeighboursFValues.IndexOf(NeighboursFValues.Min())];

            //Debug.Log($"New current node : {OpenedNeighbours[NeighboursFValues.IndexOf(NeighboursFValues.Min())]} with f : {NeighboursFValues.Min()}");
        }

        path.Add(EndNode);
        path.Add(CurrentNode);
        while (CurrentNode.PrecedentNode != null)
        {
            CurrentNode = CurrentNode.PrecedentNode;
            path.Add(CurrentNode);
        }

        //path.Reverse();
        return path;
    }

    /// <summary>
    /// Prend en paramètre un Node pour le fermer puis ouvrir et ajouter ses voisins aux listes, ou bien terminer la fonction Analyze si Node est l'arrivée recherchée.
    /// </summary>
    /// <param name="nodeToProcess"></param>
    public void ProcessNode(Node nodeToProcess)
    {
        // On ferme ce noeud
        CurrentNode.State = NodeStateEnum.Closed;

        //S'il est déjà dans les voisins ouverts, on le retire ainsi que son f
        if (OpenedNeighbours.Contains(CurrentNode))
        {
            OpenedNeighbours.Remove(CurrentNode);
            NeighboursFValues.Remove(CurrentNode.GetF());
        }

        // Puis on vérifie les voisins de ce noeud
        foreach (Node neighbour in nodeToProcess.Neighbours)
        {
            // si le voisin est l'arrivée, on stop tout
            if (neighbour == EndNode)
            {
                neighbour.Open(CurrentNode);
                int neighbourRank = 0;
                Node checker = CurrentNode;
                while (checker.PrecedentNode != null)
                {
                    neighbourRank++;
                    checker = checker.PrecedentNode;
                }

                neighbour.Rank = neighbourRank;
                OpenedNeighbours.Add(neighbour);
                NeighboursFValues.Add(neighbour.GetF());
                _finished = true;
            }

            // sinon, si le voisin n'est pas fermé ou si c'est le seul, on l'ajoute à la liste des voisins ouverts
            else if (neighbour.State != NodeStateEnum.Closed | CurrentNode.Neighbours.Count <= 1)
            {
                // on ouvre ses voisins et on assigne le noeud actuel comme noeud précédent
                neighbour.Open(CurrentNode);

                int neighbourRank = 0;
                Node checker = CurrentNode;
                while(checker.PrecedentNode != null)
                {
                    neighbourRank++;
                    checker = checker.PrecedentNode;
                }

                neighbour.Rank = neighbourRank;

                // on l'ajoute à la liste des voisins ouverts ainsi que son f
                OpenedNeighbours.Add(neighbour);
                NeighboursFValues.Add(neighbour.GetF());                
            }
        }
    }

    public void ResetNodes()
    {
        foreach(Node node in Nodes)
        {
            node.State = NodeStateEnum.Base;
            node.HasToUsePriorityColor = false;
            node.PrecedentNode = null;
            node.Rank = 0;
        }
    }
}