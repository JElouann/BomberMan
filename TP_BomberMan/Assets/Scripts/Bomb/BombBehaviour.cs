using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    public Collider2D Collider;
    public Rigidbody2D Rb;
    public SpriteRenderer SpriteRenderer;
    private Animator _animator;

    private void Start()
    {
        Collider = this.TryGetComponent(out Collider2D collider) ? collider : null;
        Rb = this.TryGetComponent(out Rigidbody2D rb) ? rb : null;
        SpriteRenderer = this.TryGetComponent(out SpriteRenderer spriteRenderer) ? spriteRenderer : null;
        _animator = this.TryGetComponent(out Animator animator) ? animator : null;
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
        RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector2.up, 1);
        RaycastHit2D botHit = Physics2D.Raycast(transform.position, Vector2.down, 1);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, Vector2.left, 1);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, Vector2.right, 1);

        if(topHit.collider != null)
        {
            if (topHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
        }
        if (botHit.collider != null)
        {
            if (botHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
        }
        if (leftHit.collider != null)
        {
            if (leftHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
        }
        if (rightHit.collider != null)
        {
            if (rightHit.collider.TryGetComponent(out Obstacle obstacle))
            {
                StartCoroutine(obstacle.Destroy());
            }
        }
    }

    public void StartBombCooldown()
    {
        StartCoroutine(ExplodeIn(_cooldown));
    }

    private IEnumerator ExplodeIn(float seconds)
    {
        _animator.SetBool("PreExplode", true);
        yield return new WaitForSeconds(seconds);
        this.transform.DOScale(2, 0.1f);
        _animator.SetBool("PreExplode", false);
        _animator.SetBool("Exploding", true);
        this.transform.Rotate(Vector3.forward * 45);
        
        Explode();
        
        yield return new WaitForSeconds(0.55f);
        SpriteRenderer.DOFade(0, 0.5f);
    }

    public IEnumerator OnDrop()
    {
        this.Collider.enabled = false;
        StartBombCooldown();
        yield return new WaitForSeconds(0.4f);
        this.SpriteRenderer.sortingOrder = 9;
    }
}
