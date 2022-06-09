using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationStartTime : MonoBehaviour
{
    public string animationName;

    Animator animator;
    private void Start() {
        animator = GetComponent<Animator>();
        animator.Play(animationName, 0, Random.Range(0, 1f));
    }
}
