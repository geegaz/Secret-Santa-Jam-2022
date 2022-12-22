using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterVisual : MonoBehaviour
{
    // TODO: Change trail time when attacking
    public void StartTrail() {
        Debug.Log("Started Trail");
    }

    public void StopTrail() {
        Debug.Log("Stopped Trail");
    }
}
