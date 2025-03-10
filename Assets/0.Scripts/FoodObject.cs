using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] AudioClip eatSound;
    public int AmountGranted = 10;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        
        //음식 증가
        GameManager.Instance.ChangeFood(AmountGranted);
        GameManager.Instance.PlaySound(eatSound);
    }
}
