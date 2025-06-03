//Adapted From: SquashAndStretch.cs
//Original Author: ChristinaCreatesGames
//GitHub Repo Link: https://github.com/Maraakis/ChristinaCreatesGames/blob/main/Squash%20and%20Stretch%20Animation%20with%20Code/SquashAndStretch.cs 

using UnityEngine;
using System;
using System.Collections;
using System.Numerics;


public class SquashAndStretch : MonoBehaviour
{
    [Header("Squash and Stretch Core")]
    [SerializeField] private Transform transformToAffect;
    [SerializeField] private SquashAndStretchAxis axisToAffect = SquashAndStretchAxis.Y;
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.25f;
    [SerializeField] private bool canBeOverwritten;
    [SerializeField] private bool playOnStart; //may not need

    [Flags]
    public enum SquashAndStretchAxis
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
        
    }

    [Header("Animation Settings")]
    [SerializeField] private float initialScale = 1f;
    [SerializeField] private float maximumScale = 1.3f;
    [SerializeField] private bool resetToInitialScaleAfterAnimation = true;

    [SerializeField]
    private AnimationCurve squashAndStretchCurve = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.25f, 1f),
        new Keyframe(1f, 0f)
    );

    private Coroutine _squashAndStrechCoroutine;
    private UnityEngine.Vector3 _initialVectorScale;
    private bool affectX => (axisToAffect & SquashAndStretchAxis.X) != 0;
    private bool affectY => (axisToAffect & SquashAndStretchAxis.Y) != 0;
    private bool affectZ => (axisToAffect & SquashAndStretchAxis.Z) != 0;

    void Awake()
    {
        if (transformToAffect == null)
            transformToAffect = transform;

        _initialVectorScale = transformToAffect.localScale;

    }
    void Start()
    {
        if (playOnStart)
            CheckForAndStartCoroutine();
    }

    public void PlaySquashAndStretchEffect()
    {
        CheckForAndStartCoroutine();
     }

    private void CheckForAndStartCoroutine()
    {
        if (_squashAndStrechCoroutine != null)
        {
            StopCoroutine(_squashAndStrechCoroutine);
            if (resetToInitialScaleAfterAnimation)
                transform.localScale = _initialVectorScale;
        }

        _squashAndStrechCoroutine = StartCoroutine(routine: SquashAndStretchEffect());
    }

    private IEnumerator SquashAndStretchEffect()
    {
        float elapsedTime = 0;
        UnityEngine.Vector3 originalScale = _initialVectorScale;
        UnityEngine.Vector3 modifiedScale = originalScale;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float curvePosition = elapsedTime / animationDuration;
            float curveValue = squashAndStretchCurve.Evaluate(curvePosition);
            float remappedValue = initialScale + (curveValue * (maximumScale - initialScale));

            //Ensures no division by 0 can occur
            float minimumThreshold = 0.0001f;
            if (Mathf.Abs(remappedValue) < minimumThreshold)
                remappedValue = minimumThreshold;

            if (affectX)
                modifiedScale.x = originalScale.x * remappedValue;
            else
                modifiedScale.x = originalScale.x / remappedValue;

            if (affectY)
                modifiedScale.y = originalScale.y * remappedValue;
            else
                modifiedScale.y = originalScale.y / remappedValue;

            if (affectZ)
                modifiedScale.z = originalScale.z * remappedValue;
            else
                modifiedScale.z = originalScale.z / remappedValue;

            transformToAffect.localScale = modifiedScale;

            yield return null;
        }

        if (resetToInitialScaleAfterAnimation)
            transformToAffect.localScale = originalScale;
    }

}
