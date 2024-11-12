using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] private int _bombNumber;
    [SerializeField] private GameObject _bombPrefab;
    public Queue<GameObject> Bombs { get; set; } = new();

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
            GameObject newBomb = Instantiate(_bombPrefab);
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
        return bomb;
    }
}
