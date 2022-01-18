using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //un void privado OnTrigger(El trigger esta ON) Enter(Al entrar) 2D(2 dimensiones ya que el juego es 2D) 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")//collider 2D collision(esta colisionando el collision "Player")
        {
            PlayerController controller = 
                collision.GetComponent<PlayerController>();
            controller.Die();
        }
    }
}
