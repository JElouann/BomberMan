using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    public CircleCollider2D Collider;
    public Rigidbody2D Rb;
    public SpriteRenderer SpriteRenderer;
    public Animator Animator;

    public Node Node;

    private void Awake()
    {
        Collider = this.TryGetComponent(out CircleCollider2D collider) ? collider : null;
        Rb = this.TryGetComponent(out Rigidbody2D rb) ? rb : null;
        SpriteRenderer = this.TryGetComponent(out SpriteRenderer spriteRenderer) ? spriteRenderer : null;
        Animator = this.TryGetComponent(out Animator animator) ? animator : null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.TryGetComponent(out PlayerHold pHold);
        if (pHold.BombSlot.transform.childCount > 0) return;

        this.transform.SetParent(pHold.BombSlot.transform);
        this.transform.DOLocalJump(Vector3.zero, 0.7f, 1, 0.4f);
        SpriteRenderer.sortingOrder = 11;
    }

    public void Explode()
    {
        Camera.main.DOShakePosition(0.5f, 1f, 13);
        RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector2.up, 6);
        RaycastHit2D botHit = Physics2D.Raycast(transform.position, Vector2.down, 6);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 6);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 6);

        if(topHit.collider != null)
        {
            if (topHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
            if (topHit.collider.TryGetComponent(out Sheep sheep))
            {
                sheep.Damage();
            }
        }
        if (botHit.collider != null)
        {
            if (botHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
            if (botHit.collider.TryGetComponent(out Sheep sheep))
            {
                sheep.Damage();
            }
        }
        if (leftHit.collider != null)
        {
            if (leftHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
            if (leftHit.collider.TryGetComponent(out Sheep sheep))
            {
                sheep.Damage();
            }
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
            if (rightHit.collider.TryGetComponent(out Sheep sheep))
            {
                sheep.Damage();
            }
        }
    }

    public void StartBombCooldown()
    {
        StartCoroutine(ExplodeIn(_cooldown));
    }

    private IEnumerator ExplodeIn(float seconds)
    {
        Animator.SetBool("PreExplode", true);
        yield return new WaitForSeconds(seconds);
        this.transform.DOScale(2, 0.1f);
        Animator.SetBool("PreExplode", false);
        Animator.SetBool("Exploding", true);
        this.transform.Rotate(Vector3.forward * 45);
        
        Explode();
        
        yield return new WaitForSeconds(0.55f);
        SpriteRenderer.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        BombManager.Instance.AddToQueue(this.gameObject);
    }

    public IEnumerator OnDrop()
    {
        this.Collider.enabled = false;
        StartBombCooldown();
        yield return new WaitForSeconds(0.4f);
        this.SpriteRenderer.sortingOrder = 9;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Node node))
        {
            Node = node;
        }
    }
}
