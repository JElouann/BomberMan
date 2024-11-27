using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotAnimation : MonoBehaviour
{
    private BotMovement _botMovement;
    private Animator _animator;
    private SpriteRenderer _spriteR;
    private Rigidbody2D _rb;
    private PlayerHold _pHold;

    private bool _isMoving;
    private bool _isHolding;

    void Start()
    {
        _botMovement = TryGetComponent(out BotMovement botMovement) ? botMovement : null;
        _animator = TryGetComponent(out Animator animator) ? animator : null;
        _rb = TryGetComponent(out Rigidbody2D rb) ? rb : null;
        _pHold = TryGetComponent(out PlayerHold pHold) ? pHold : null;
        _spriteR = TryGetComponent(out SpriteRenderer spriteR) ? spriteR : null;
    }

    public void FlipSpriteX()
    {
        _spriteR.flipX = !_spriteR.flipX;
    }

    private void Update()
    {
        _isMoving = _botMovement.TargetNode != null;
        _isHolding = _pHold.BombSlot.transform.childCount > 0;
        _animator.SetBool("IsMoving", _isMoving);
        _animator.SetBool("IsHolding", _isHolding);
    }
}
