using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match.Skills
{
    public class WindSkill : MonoBehaviour
    {
        public GameObject windEffect;
        private float _windEffectDuration = 5f;

        private float _distanceClamp = 5f;
        private ItemSpawner _itemSpawner;

        private void OnEnable()
        {
            GameEvents.OnWindSkillUsed += OnWindSkillUsed;
        }

        private void OnDisable()
        {
            GameEvents.OnWindSkillUsed -= OnWindSkillUsed;
        }

        public void Initialize(ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
            windEffect.SetActive(false);
        }

        private void OnWindSkillUsed()
        {
            var items = _itemSpawner.GetItems();
            if (items == null || items.Count == 0)
                return;

            windEffect.SetActive(true);

            var centerPosition = transform.position;

            foreach (var item in items)
            {
                DOTween.Kill(item.transform);

                item.selfRigidbody.isKinematic = true;
                item.SetCollidersActive(false);

                var position = item.transform.position;
                position.y = centerPosition.y;
                var direction = (position - centerPosition);
                direction *= Random.Range(1f, 2f);
                direction = Vector3.ClampMagnitude(direction, _distanceClamp);
                item.transform.position = centerPosition + direction;

                var rotation = 360 * _windEffectDuration + Random.Range(-90, 90);
                float t = 0;
                float lastT = 0;
                DOTween.To(() => t, x => t = x, 1, _windEffectDuration)
                    .SetEase(Ease.InOutCubic)
                    .OnUpdate(() =>
                    {
                        var delta = t - lastT;
                        item.transform.RotateAround(centerPosition, Vector3.up, delta * rotation);

                        //wind merkezine ve dışında doğru rastgele hareket
                        var distance = item.transform.position - centerPosition;
                        if (t < 0.5f && Random.value < 0.1f)
                        {
                            distance *= 0.99f;
                            item.transform.position = centerPosition + distance;
                        }
                        else if (t > 0.5f && t < 0.9f && Random.value < 0.1f)
                        {
                            distance *= 1.01f;
                            item.transform.position = centerPosition + distance;
                        }

                        lastT = t;
                    })
                    .OnComplete(() =>
                    {
                        item.selfRigidbody.isKinematic = false;
                        item.SetCollidersActive(true);
                    });
            }

            DOVirtual.DelayedCall(_windEffectDuration, () => { windEffect.SetActive(false); });
        }
    }
}