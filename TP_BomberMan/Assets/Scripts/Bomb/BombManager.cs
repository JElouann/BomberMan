using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] private int _bombNumber;
    [SerializeField] private GameObject _bombPrefab;
    public Queue<GameObject> Bombs { get; set; } = new();

    public List<BombBehaviour> OnFieldBombs { get; set; } = new();

    private List<Vector3> _spawnPos = new() { new(-0.5f, -0.5f, 0), new(-0.5f, 0.5f, 0), new(0.5f, 0.5f), new(0.5f, -0.5f)};

    [SerializeField] private GameObject _parent;

    // Singleton
    #region Singleton
    private static BombManager _instance;

    public static BombManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Bomb Manager");
                _instance = go.AddComponent<BombManager>();
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

    private void Start()
    {
        for (int i = 0; i < _bombNumber; i++)
        {
            GameObject newBomb = Instantiate(_bombPrefab, _parent.transform);
            newBomb.TryGetComponent(out BombBehaviour behaviour);
            OnFieldBombs.Add(behaviour);
            newBomb.transform.position = GetSpawnPosition();
            Bombs.Enqueue(newBomb);
        }
    }

    public void AddToQueue(GameObject bomb)
    {

    }

    public GameObject RemoveFromQueue()
    {
        GameObject bomb = Bombs.Dequeue();
        bomb.TryGetComponent(out BombBehaviour behaviour);
        behaviour.Collider.enabled = true;
        behaviour.SpriteRenderer.sortingOrder = 11;
        bomb.transform.position = GetSpawnPosition();
        OnFieldBombs.Add(behaviour);
        return bomb;
    }

    private Vector3 GetSpawnPosition()
    {
        List<Vector3> list = _spawnPos;
        foreach(BombBehaviour bomb in OnFieldBombs)
        {
            if (list.Contains(bomb.transform.position))
            {
                list.Remove(bomb.transform.position);
            }
        }

        return list[Random.Range(0, list.Count - 1)];
    }

    public BombBehaviour GetRandomBomb()
    {
        return OnFieldBombs[Random.Range(0, OnFieldBombs.Count - 1)];
    }
}
