using System;
using TMPro;
using UnityEngine;

[Serializable]
public class CardView
{
    [SerializeField] private SpriteRenderer art, outline;
    [SerializeField] private TMP_Text titleText, descriptionText, manaText, attackText, healthText;

    public void UpdateArtSprite(CardData data)
    {
        art.sprite = data.Art;
    }

    public void UpdateAllText(CardData data)
    {
        titleText.text = data.Title;
        descriptionText.text = data.Description;
        manaText.text = data.Mana.ToString();
        attackText.text = data.Attack.ToString();
        healthText.text = data.Health.ToString();
    }

    public void UpdateCharacteristics(CardData data)
    {
        manaText.text = data.Mana.ToString();
        attackText.text = data.Attack.ToString();
        healthText.text = data.Health.ToString();
    }

    public void UpdateMana(CardData data)
    {
        manaText.color = data.Mana <= 0 ? Color.red : Color.white;
        manaText.text = data.Mana.ToString();
    }

    public void UpdateAttack(CardData data)
    {
        attackText.color = data.Attack <= 0 ? Color.red : Color.white;
        attackText.text = data.Attack.ToString();
    }

    public void UpdateHealth(CardData data)
    {
        healthText.color = data.Health <= 0 ? Color.red : Color.white;
        healthText.text = data.Health.ToString();
    }

    public void Chosen(bool val)
    {
        outline.gameObject.SetActive(val);
    }
}