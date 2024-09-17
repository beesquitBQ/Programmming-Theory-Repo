using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorExtensions
{
    public static IEnumerator WaitForAnimatorState(this Animator animator, string stateName)
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) ||
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
    }
}