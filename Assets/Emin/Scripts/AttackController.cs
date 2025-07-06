using System;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    private GoblinAnimationContoller _goblinAnimationController;
    private bool _isAttacking;
    private bool _isDead;

    private void Awake()
    {
        _goblinAnimationController = GetComponent<GoblinAnimationContoller>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _isDead = true;
            _goblinAnimationController.GetDeathAnimation();
            GetComponent<Movement>().enabled = false;
            Camera.main.transform.GetComponent<MouseMovement>().enabled = false;
        }

        if (_isDead)
            return;
        if (Input.GetKeyDown(KeyCode.V))
        {
            _isAttacking = !_isAttacking;
        }
        if (Input.GetMouseButtonDown(0) && !_goblinAnimationController.GetStateInfo(0).IsName("Sprinting Forward Roll"))
        {
            Debug.Log("Attack");
            if (_isAttacking)
                _goblinAnimationController.GetSlashAnimation();
            else
                _goblinAnimationController.GetPunchAnimation();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _goblinAnimationController.GetHitAnimation();
        }

        if (Input.GetKeyDown(KeyCode.R) && !_goblinAnimationController.GetStateInfo(1).IsName("Great Sword Slash") &&
                                            !_goblinAnimationController.GetStateInfo(1).IsName("Cross Punch"))
        {
            Debug.Log("Roll");
            _goblinAnimationController.GetRollAnimation();
        }
    }
}