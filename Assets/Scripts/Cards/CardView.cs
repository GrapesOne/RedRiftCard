using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

[Serializable]
public class CardView
{
    [SerializeField] private SpriteRenderer art, outline;
    [SerializeField] private TMP_Text titleText, descriptionText, manaText, attackText, healthText;

    [SerializeField] private float updateTextTime = 0.5f;
    [SerializeField] private Ease updateTextEase = Ease.Linear;

    public void UpdateArtSprite(CardData data)
    {
        art.sprite = data.Art;
    }

    public void UpdateAllText(CardData data)
    {
        titleText.text = data.Title;
        descriptionText.text = data.Description;
        manaText.DOTextValueChange(data.Mana, updateTextTime).SetEase(updateTextEase);
        attackText.DOTextValueChange(data.Attack, updateTextTime).SetEase(updateTextEase);
        healthText.DOTextValueChange(data.Health, updateTextTime).SetEase(updateTextEase);
    }

    public void UpdateCharacteristics(CardData data)
    {
        manaText.DOTextValueChange(data.Mana, updateTextTime).SetEase(updateTextEase);
        attackText.DOTextValueChange(data.Attack, updateTextTime).SetEase(updateTextEase);
        healthText.DOTextValueChange(data.Health, updateTextTime).SetEase(updateTextEase);
    }

    public void UpdateMana(CardData data)
    {
        manaText.color = data.Mana <= 0 ? Color.red : Color.white;
        manaText.DOTextValueChange(data.Mana, updateTextTime).SetEase(updateTextEase);
    }

    public void UpdateAttack(CardData data)
    {
        attackText.color = data.Attack <= 0 ? Color.red : Color.white;
        attackText.DOTextValueChange(data.Attack, updateTextTime).SetEase(updateTextEase);
    }

    public void UpdateHealth(CardData data)
    {
        healthText.color = data.Health <= 0 ? Color.red : Color.white;
        healthText.DOTextValueChange(data.Health, updateTextTime).SetEase(updateTextEase);
    }

    public void Chosen(bool val)
    {
        outline.gameObject.SetActive(val);
    }
}