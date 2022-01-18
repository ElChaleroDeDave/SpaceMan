using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType//enum de los tres tipos de collectables 
{
    healthPotion,
    manaPotion,
    money
}


public class Collectable : MonoBehaviour
{

    public CollectableType type = CollectableType.money;

    private SpriteRenderer sprite;
    private CircleCollider2D itemCollider;

    bool hasBeenCollected = false;

    public int value = 1;//Valor el  collected

    GameObject player;

    public void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Show()
    {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide()
    {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    
    void Collect()
    {
        Hide();
        hasBeenCollected = true;

        switch (this.type)
        {
            case CollectableType.money://la colleccion es dinero
                GameManager.sharedInstance.collectObject(this);
                break;
            case CollectableType.healthPotion://la colleccion es vida
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                Debug.Log("cOLLEct");
                break;
            case CollectableType.manaPotion://la colleccion es mana
                player.GetComponent<PlayerController>().CollectMana(this.value);
                break;
        }
    }

    //cuando collisiones con un collectable
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Collect();
            Debug.Log("moneda");
        }
    }

}
