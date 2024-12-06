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

            currentObject = null;
            currentItem.gameObject.SetActive(false);
            otherItem.gameObject.SetActive(false);
            return true;
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
            var pos = currentObject.transform.position;
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

            currentObject.transform.position = transform.position;
        }
    }
}