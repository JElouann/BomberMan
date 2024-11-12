using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHold : MonoBehaviour
{
    [field: SerializeField]
    public GameObject BombSlot { get; private set; }

    private void Update()
    {
        
    }

    public void DropBomb(InputAction.CallbackContext context)
    {
        if (!context.performed | BombSlot.transform.childCount <= 0) return;
        print("dropped");
        GameObject bomb = BombSlot.transform.GetChild(0).gameObject;
        bomb.TryGetComponent(out BombBehaviour bombBehaviour);
        StartCoroutine(bombBehaviour.OnDrop());
        bomb.transform.parent = null;
        bomb.transform.DOJump(transform.position, 0.4f, 1, 0.4f);
    }
}
