using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match
{
    public class MatchArea : MonoBehaviour
    {
        public GameObject currentObject;

        private readonly string objectTag = "Moveable";
        private Coroutine placeObjectCoroutine;

        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _leftObjectPlacement;
        [SerializeField] private Transform _rightObjectPlacement;

        private readonly int _openLidHash = Animator.StringToHash("OpenLid");
        private readonly int _closeLidHash = Animator.StringToHash("CloseLid");

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == null || other.attachedRigidbody.CompareTag(objectTag) == false)
                return;

            if (other.gameObject == currentObject)
                return;

            if (currentObject == null)
            {
                SetCurrentObject(other);
            }
            else
            {
                if (ChechMatch(other))
                    return;

                other.attachedRigidbody.AddForce(Vector3.up * 15 + Vector3.forward * 15f, ForceMode.Impulse);
            }
        }

        private bool ChechMatch(Collider other)
        {
            var currentItem = currentObject.GetComponent<Item>();
            var otherItem = other.attachedRigidbody.gameObject.GetComponent<Item>();
            if (!currentItem.IsMatching(otherItem))
                return false;

            if (placeObjectCoroutine != null)
            {
                StopCoroutine(placeObjectCoroutine);
            }

            other.attachedRigidbody.isKinematic = true;
            StartCoroutine(MatchCoroutine(otherItem));
            return true;
        }

        private IEnumerator MatchCoroutine(Item otherItem)
        {
            float openDuration = 0.5f;
            float closeDuration = 0.5f;
            float objectMovementDuration = 1f;

            yield return null;
            var currentItem = currentObject.GetComponent<Item>();

            //iki objeyi de yerine yerleştir
            otherItem.transform.position = _rightObjectPlacement.position;
            otherItem.transform.rotation = _rightObjectPlacement.rotation;

            //kapak açılma animasyonunu başlat
            _animator.SetTrigger(_openLidHash);

            yield return new WaitForSeconds(openDuration);
            //objeleri merkeze al, aşağıya doğru kaydır

            float timer = 0f;
            var currentPos = currentItem.transform.position;
            var otherPos = otherItem.transform.position;
            Vector3 targetPos = (currentItem.transform.position + otherItem.transform.position) / 2f;

            while (timer < objectMovementDuration)
            {
                currentItem.transform.position = Vector3.Lerp(currentPos, targetPos, timer / objectMovementDuration);
                otherItem.transform.position = Vector3.Lerp(otherPos, targetPos, timer / objectMovementDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            currentItem.transform.position = targetPos;
            otherItem.transform.position = targetPos;

            timer = 0f;
            currentPos = targetPos;
            targetPos = targetPos + Vector3.down * 2f;
            while (timer < objectMovementDuration)
            {
                currentItem.transform.position = Vector3.Lerp(currentPos, targetPos, timer / objectMovementDuration);
                otherItem.transform.position = Vector3.Lerp(currentPos, targetPos, timer / objectMovementDuration);
                timer += Time.deltaTime;
                yield return null;
            }


            //kapatma animasyonunu başlat
            _animator.SetTrigger(_closeLidHash);
            yield return new WaitForSeconds(closeDuration);

            //objeleri yok et  
            currentObject = null;
            if (currentItem != null && otherItem != null)
            {
                currentItem.gameObject.SetActive(false);
                otherItem.gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody == null || other.attachedRigidbody.CompareTag(objectTag) == false)
                return;
            if (other.attachedRigidbody.gameObject == currentObject)
            {
                if (placeObjectCoroutine != null)
                {
                    StopCoroutine(placeObjectCoroutine);
                }

                currentObject = null;
            }
        }

        private void SetCurrentObject(Collider other)
        {
            other.attachedRigidbody.isKinematic = true;
            currentObject = other.attachedRigidbody.gameObject;
            if (placeObjectCoroutine != null)
            {
                StopCoroutine(placeObjectCoroutine);
            }

            placeObjectCoroutine = StartCoroutine(PlaceCurrentObject());
        }

        private IEnumerator PlaceCurrentObject()
        {
            yield return null;
            currentObject.transform.position = _leftObjectPlacement.position;
            currentObject.transform.rotation = _leftObjectPlacement.rotation;

            /*var pos = currentObject.transform.position;
            var targetPos = transform.position;
            float moveDuration = 3f;
            float timer = 0;
            while (timer < moveDuration)
            {
                if (currentObject == null)
                    yield break;

                currentObject.transform.position = Vector3.Lerp(pos, targetPos, timer / moveDuration);
                currentObject.transform.Rotate(Vector3.up, 180f * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }

            currentObject.transform.position = transform.position;*/
        }
    }
}