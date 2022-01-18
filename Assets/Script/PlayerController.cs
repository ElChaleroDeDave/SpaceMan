using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 6f; //Fuerza(Force) de salto(jump)
    public float RuningSpeed = 2f; //Velecidad(Speed) de Jugador(Player)(Corres)

    Rigidbody2D rigidBody;//Referencia de Rigidbody2D(Unity)
    Animator animator;//Referencia de animator(Unity)
    Vector3 startPosition;//Un vector3(Vector de tres dimensiones) StartPosition

    const string STATE_ALIVE = "isAlive"; //
    const string STATE_ON_THE_GROUND = "isOnTheGround"; //

    private int healthPoints, manaPoint;

    public const int INITIAL_HEALTH = 100,
        INITIAL_MANA = 15, MAX_HEALTH = 200, MAX_MANA = 30,
        MIN_HEALTH = 10, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5;
    public const float SUPERJUMP_FORCE = 1.5f;


    public LayerMask groundMask; //Un LayerMask llamado groundMask(basicamente un layer para el suelo) publica

    //un void que se ejecuta al despertar
    private void Awake() 
    {
        rigidBody = GetComponent<Rigidbody2D>();//rigidbody que agarra el rigidbody de unity(Segun entendi)
        animator = GetComponent<Animator>();//animator que agarra el animator de unity(Segun entendi)
    }

    // Inicia cuando se ejecuta el juego
    void Start()
    {
        startPosition = this.transform.position;


    }
    //un void public del StartGame(Inicio de el juego)
    public void StartGame()
    {
        animator.SetBool(STATE_ALIVE, true);//Llama al animator y le dice que STATE ALIVE es true(Verdadero)
        animator.SetBool(STATE_ON_THE_GROUND, true);//Llama al animator y le dice que STATE ON THE GROUND es true

        Invoke("RestartPosition", 0.2f);//un invoke(El invoke le da un un tiempo) Para el "RestartPosition"

        healthPoints = INITIAL_HEALTH;
        manaPoint = INITIAL_MANA;              
    }
    //un void public del RestartPosition(Restaurar posicion)
    void RestartPosition()
    {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    //se acutualiza constantemente
    void Update()
    {
        if (Input.GetButtonDown("Jump"))//atajo para el jump
        {
            jump(false);
        }
        animator.SetBool(STATE_ON_THE_GROUND, IsTouchinginTheGround());
        if (Input.GetButtonDown("Superjump"))
        {
            jump(true);
        }
                

        Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.red);
        
    }
    //igual a el update pero en esta caso se utiliza para no ver retraso en las physics(fisicas)
    void FixedUpdate()
    {
        if(GameManager.sharedInstance.currentGameState == GameState.inGame)
            {
            if (rigidBody.velocity.x < RuningSpeed)
        {
            rigidBody.velocity = new Vector2(RuningSpeed, rigidBody.velocity.y);
        }
            }
        else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }
    //un void para el jump
    void jump(bool superjump)
    {
        float jumpForceFactor = jumpForce;

        if (superjump && manaPoint >= SUPERJUMP_COST)
        {
            manaPoint -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
            {
            if (IsTouchinginTheGround())
        {
            rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
        }
            }
    }
    //un bool para saber si el player esta tocando el suelo
    bool IsTouchinginTheGround()
    {
        if (Physics2D.Raycast(this.transform.position,
            Vector2.down, 1.5f,
            groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    //un void publico para Die(la muerte de el player)
    public void Die()
    {
        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore",0f);
        if (travelledDistance > previousMaxDistance)
        {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if(this.healthPoints >= MAX_HEALTH)
        {
            this.healthPoints = MAX_HEALTH;
        }
    }

    public void CollectMana(int points)
    {
        this.manaPoint += points;
        if (this.manaPoint >= MAX_MANA)
        {
            this.manaPoint = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }

    public int GetMana()
    {
        return manaPoint;
    }

    public float GetTravelledDistance()
    {
        return this.transform.position.x - startPosition.x;
    }

}
