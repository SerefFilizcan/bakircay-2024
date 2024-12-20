using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Match.View
{
    public class UIController : MonoBehaviour
    {
        public TMP_Text scoreText;
        public Image objectFillImage;
        public Button windSkillButton;

        private string _scoreTextFormat = "Score: {0}";

        private ItemSpawner _itemSpawner;

        private int _score = 0;

        public void Initialize(ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
            _score = 0;

            SetScoreUI();
            objectFillImage.fillAmount = 0;
            windSkillButton.interactable = true;

            GameEvents.OnItemMatched += OnItemMatched;
        }

        private void OnDestroy()
        {
            GameEvents.OnItemMatched -= OnItemMatched;
        }


        private void OnItemMatched(ItemData data)
        {
            Debug.LogWarning(data.itemName);
            _score += data.itemScore;
            SetScoreUI();
            SetFillUI();
        }


        private void SetScoreUI()
        {
            scoreText.text = string.Format(_scoreTextFormat, _score);
        }

        private void SetFillUI()
        {
            objectFillImage.fillAmount = 1 - (_itemSpawner.CurrentItemCount / (float)_itemSpawner.SpawnedItemCount);
        }


        public void OnWindSkillButtonClick()
        {
            windSkillButton.interactable = false;
        }
    }
}