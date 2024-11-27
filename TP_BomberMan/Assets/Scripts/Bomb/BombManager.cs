using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] private int _bombNumber;
    [SerializeField] private int _bombNumberOnField;
    [SerializeField] private GameObject _bombPrefab;
    public Queue<GameObject> Bombs { get; set; } = new();

    public List<BombBehaviour> OnFieldBombs { get; set; } = new();

    private List<Vector3> _spawnPos;

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

    private async void Start()
    {
        for (int i = 0; i < _bombNumber; i++)
        {
            GameObject newBomb = Instantiate(_bombPrefab, _parent.transform);
            newBomb.TryGetComponent(out BombBehaviour behaviour);
            OnFieldBombs.Add(behaviour);
            AddToQueue(newBomb);
        }

        await Task.Delay(100);

        for(int i = 0; i < _bombNumberOnField; i++)
        {
            RemoveFromQueue();
        }
    }
    
    public void AddToQueue(GameObject bomb)
    {
        bomb.TryGetComponent(out BombBehaviour behaviour);
        OnFieldBombs.Remove(behaviour);
        behaviour.Collider.enabled = false;
        behaviour.SpriteRenderer.DOFade(1, 0);
        behaviour.SpriteRenderer.enabled = false;
        behaviour.transform.localScale = Vector3.zero;

        Bombs.Enqueue(bomb);
    }

    public GameObject RemoveFromQueue()
    {
        GameObject bomb = Bombs.Dequeue();
        bomb.TryGetComponent(out BombBehaviour behaviour);
        behaviour.Animator.SetBool("Exploding", false);
        behaviour.Animator.SetBool("PreExplode", true);
        bomb.transform.position = GetSpawnPosition();
        behaviour.Collider.enabled = true;
        behaviour.SpriteRenderer.enabled = true;
        behaviour.SpriteRenderer.sortingOrder = 11;
        behaviour.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic);
        OnFieldBombs.Add(behaviour);
        return bomb;
    }

    private Vector3 GetSpawnPosition()
    {
        print(AStarTheOneAndOnly.Instance.Nodes.Count);
        List<Vector3> spawnPos = new();
        foreach (Node node in AStarTheOneAndOnly.Instance.Nodes)
        {
            spawnPos.Add(node.transform.position);
        }

        foreach (BombBehaviour bomb in OnFieldBombs)
        {
            if (spawnPos.Contains(bomb.transform.position))
            {
                spawnPos.Remove(bomb.transform.position);
            }
        }

        return spawnPos[Random.Range(0, spawnPos.Count)];
    }

    private void Update()
    {
        if (OnFieldBombs.Count < _bombNumberOnField)
        {
            print("need bomb");
            RemoveFromQueue();
        }
    }
}
