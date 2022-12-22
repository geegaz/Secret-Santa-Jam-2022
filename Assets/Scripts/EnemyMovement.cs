using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform _enemyVisual;
    [SerializeField] private Animator _enemyAnimation;

    [Header("Target")]
    [SerializeField, ReadOnly] private Transform _target;

    private NavMeshAgent _navMeshAgent;

    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
}
