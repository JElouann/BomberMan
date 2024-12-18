using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{   
    private Animator _animator;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private void Start()
    {
        _animator = TryGetComponent(out Animator animator) ? animator : null;    
        _rb = TryGetComponent(out Rigidbody2D rb) ? rb : null;
        _collider = TryGetComponent(out Collider2D collider) ? collider : null;
    }

    public IEnumerator Destroy()
    {
        _rb.simulated = false;
        _collider.enabled = false;
        this.transform.DOJump(transform.position, 0.3f, 1, 0.3f);
        yield return new WaitForSeconds(0.3f);
        _animator.SetBool("IsDestroyed", true);
    }
}
