using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeMapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _map;

    [SerializeField] private GameObject _test;
    [SerializeField] private GameObject _null;
    [SerializeField] private GameObject _parent;

    [SerializeField] private AStarTheOneAndOnly nodeParent;

    public List<List<Node>> Nodes = new();

    private void Start()
    {
        BoundsInt bounds = _map.cellBounds;
        TileBase[] allTiles = _map.GetTilesBlock(bounds);

        // transformer ça en listes de listes => facilitera la mise en place des neighbours de chaque node


        for (int x = 0; x < bounds.size.x; x++)
        {
            List<Node> nodes = new();
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    GameObject test = Instantiate(_test, _parent.transform);
                    test.transform.localPosition = new Vector3(x, y, 0);
                    test.TryGetComponent(out Node node);
                    node.ParentGraph = nodeParent;
                    node.IsFiller = false;
                    nodes.Add(node);
                }
                else
                {
                    GameObject test = Instantiate(_null, _parent.transform);
                    test.transform.localPosition = new Vector3(x, y, 0);
                    test.TryGetComponent(out Node node);
                    node.IsFiller = true;
                    nodes.Add(node);
                }
            }
            Nodes.Add(nodes);
        }

        for(int x = 0; x < Nodes.Count; x++)
        {
            for (int y = 0; y < Nodes[x].Count; y++)
            {
                Node currentNode = Nodes[x][y];
                if (currentNode.gameObject.name == "NULL") return;

                //CheckNeighbour(currentNode);
            }
        }
    }

    private void CheckNeighbour(Node current)
    {
        int layer_mask = LayerMask.GetMask("Node");

        RaycastHit2D topHit = Physics2D.Raycast(current.gameObject.transform.position, Vector2.up, 1, layer_mask);
        RaycastHit2D botHit = Physics2D.Raycast(current.gameObject.transform.position, Vector2.down, 1, layer_mask);
        RaycastHit2D leftHit = Physics2D.Raycast(current.gameObject.transform.position, Vector2.left, 1, layer_mask);
        RaycastHit2D rightHit = Physics2D.Raycast(current.gameObject.transform.position, Vector2.right, 1, layer_mask);
        
        //List<RaycastHit2D> hits = new() { topHit, botHit, leftHit, rightHit };

        //foreach(RaycastHit2D hit in hits)
        //{
        //    print(hit.collider);
        //    if (hit.collider == null) return;
        //    hit.collider.TryGetComponent(out Node node);
        //    current.Neighbours.Add(node);
        //    print(hit.collider.gameObject.layer);
        //}

        if(topHit.collider ==  null)
        {
            print(topHit.collider.gameObject.layer);
        }
    }
}
