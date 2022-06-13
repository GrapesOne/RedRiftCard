using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AbstractCardHolder : MonoBehaviour 
{
    protected LinkedList<CardController> cardsInHolder = new LinkedList<CardController>();
    [SerializeField] protected Transform cardsHolder;
    public abstract bool TakeCard(CardController card);
    public abstract bool RemoveCard(CardController card);
    
    public abstract bool SilentRemoveCard(CardController card);
    protected abstract void UpdateCardsPosition();
    public abstract Task ForEachCard(Action<CardController> action);
}
