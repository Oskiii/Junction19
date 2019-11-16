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

    public void MoveTowards(Vector3 worldLocalPosition, float dist)
    {
        _moveTween = transform.DOLocalMove(worldLocalPosition, dist);
    }
}
