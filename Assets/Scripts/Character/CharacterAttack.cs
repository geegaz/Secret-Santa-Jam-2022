using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[RequireComponent(typeof(Collider))]
public class CharacterAttack : MonoBehaviour
{
    public GameObject weaponObject;
    public WeaponDataSO weaponData;

    public bool canAttack {
        get { return _cooldown <= 0.0f; }
    }

    [SerializeField] private float _attackCooldown;
    [SerializeField, ReadOnly] private float _cooldown = 0.0f;

    private Collider _attackTrigger;
    private Coroutine _attackCoroutine;

    private void Awake() {
        _attackTrigger = GetComponent<Collider>();
        _attackTrigger.isTrigger = true;
    }

    public void Attack() {
        if (_attackCoroutine != null) StopCoroutine(_attackCoroutine);
        _attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    private void OnTriggerEnter(Collider other) {
        // TODO: Detect things entering the attack
    }

    private IEnumerator AttackCoroutine() {
        _cooldown = _attackCooldown;
        while (_cooldown > 0.0f) {
            

            _cooldown -= Time.deltaTime;
            yield return null;
        }
        
    }
}
