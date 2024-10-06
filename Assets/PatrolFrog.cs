using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Firefly
{
    public class PatrolFrog : MonoBehaviour
    {   
        [Tooltip("Frog will go to and from this position")]
        [SerializeField] private Transform target;
        [SerializeField] float rotateTime = 1f;
        [SerializeField] float moveTime = 2f;


        private void Start()
        {
            Sequence mySequence = DOTween.Sequence();

            float targetAngle = Vector2.SignedAngle(Vector2.up, target.position - transform.position); 
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);

            mySequence.Append(transform.DOMove(target.position, moveTime));
            mySequence.Append(transform.DORotate(new Vector3(0, 0, targetAngle + 180), rotateTime));
            mySequence.Append(transform.DOMove(transform.position, moveTime));
            mySequence.Append(transform.DORotate(new Vector3(0, 0, targetAngle), rotateTime));
            mySequence.SetLoops(-1, LoopType.Restart);
            mySequence.Play();
        }
    }
}