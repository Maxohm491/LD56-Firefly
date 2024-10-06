using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Firefly
{
    public class PatrolFrog : MonoBehaviour
    {   
        [Tooltip("Frog will go to and from this position")]
        [SerializeField] private Transform target;
        [SerializeField] float rotateTime = 1f;
        [SerializeField] float moveTime = 2f;
        private Animator _anim;

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
        }


        private void Start()
        {
            StartCoroutine(Patrol());

            //Sequence mySequence = DOTween.Sequence();

            //float targetAngle = Vector2.SignedAngle(Vector2.up, target.position - transform.position); 
            //transform.rotation = Quaternion.Euler(0, 0, targetAngle);

            //mySequence.Append(
            //    transform.DOMove(target.position, moveTime)
            //        .OnPlay(delegate
            //        {
            //            _anim.SetBool("jump", true);
            //            Debug.Log("jump");
            //        })
            //        .OnComplete(delegate
            //        {
            //            _anim.SetBool("jump", false);
            //            Debug.Log("jump stop");
            //        }));
            //mySequence.Append(transform.DORotate(new Vector3(0, 0, targetAngle + 180), rotateTime));
            //mySequence.Append(
            //    transform.DOMove(transform.position, moveTime)
            //        .OnPlay(delegate
            //        {
            //            _anim.SetBool("jump", true);
            //            Debug.Log("jump");
            //        })
            //        .OnComplete(delegate
            //        {
            //            _anim.SetBool("jump", false);
            //            Debug.Log("jump stop");
            //        }));
            //mySequence.Append(transform.DORotate(new Vector3(0, 0, targetAngle), rotateTime));
            //mySequence.SetLoops(-1, LoopType.Restart);
            //mySequence.Play();
        }

        IEnumerator Patrol()
        {
            float targetAngle = Vector2.SignedAngle(Vector2.up, target.position - transform.position);
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            Vector3 start = transform.position;
            Vector3 end = target.position;

            while (true)
            {
                _anim.SetBool("jump", true);
                yield return transform.DOMove(end, moveTime).SetEase(Ease.Linear).WaitForCompletion();
                _anim.SetBool("jump", false);
                yield return transform.DORotate(new Vector3(0, 0, targetAngle + 180), rotateTime).WaitForCompletion();
                _anim.SetBool("jump", true);
                yield return transform.DOMove(start, moveTime).SetEase(Ease.Linear).WaitForCompletion();
                _anim.SetBool("jump", false);
                yield return transform.DORotate(new Vector3(0, 0, targetAngle), rotateTime).WaitForCompletion();
            }
        }
    }
}