using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    [SerializeField]
    private Tweener _moveTween;

    public void MoveTowards(Vector3 worldLocalPosition, float dist, Vector3 worldDir)
    {
        transform.forward = worldDir;
        _moveTween = transform.DOLocalMove(worldLocalPosition, dist);
    }

    public Vector3 ProjectVectorToPlane(Vector3 vec, Vector3 planeNormal)
    {
        return vec - planeNormal.normalized * Vector3.Dot(planeNormal.normalized, vec);
    }
}