using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacoonController : MonoBehaviour
{
    public GameManager gameManager;
    public ItemQuantityController itemQuantity;
    public SnakePotController snakePotCtl;
    public float maxSpeed;
    public float jumpPower;

    public bool isLadder;
    public float Climbspeed = 6;

    //Audio
    public AudioClip audioJump;
    public AudioClip audiogetFood;
    public AudioClip audiogetBox;
    public AudioClip audioDie;
    AudioSource audioSource;

    static bool isJumping = false;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
      //Jump
      if(Input.GetButtonDown("Jump") && !isJumping) 
      {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        isJumping = true;
            PlaySound("jump");
      }

      //Stop Speed
      if(Input.GetButtonUp("Horizontal"))
        rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

      //Direction Sprite
      if(Input.GetButton("Horizontal"))
        spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

      //Animation
      if(Mathf.Abs(rigid.velocity.x) < 0.3)
        anim.SetBool("isWalking", false);
      else
        anim.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if(rigid.velocity.x > maxSpeed) //Right Max Speed
          rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed * (-1)) //Left Max Speed
          rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if(rigid.velocity.y < 0) {
          Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
          RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
          if(rayHit.collider != null) {
            if(rayHit.distance < 0.5f)
              isJumping = false;
          }
        }

        if (isLadder)
        {
            anim.SetBool("isClimbing", true);

            float ver = Input.GetAxis("Vertical");
            isJumping = false;
            rigid.gravityScale = 0;
            rigid.velocity = new Vector2(rigid.velocity.x, ver * Climbspeed);
        }
        else
        {
            anim.SetBool("isClimbing", false);

            rigid.gravityScale = 6f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      if(collision.gameObject.tag == "Enemy") {
          Debug.Log("RacoonController.cs - OnCollisionEnter2D, 너구리가 적에 닿아 스테이지가 재시작됩니다.");
            PlaySound("die");
            gameManager.restartStage();
      }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

      // 너구리가 음식에 닿았을 때
      if (collision.gameObject.tag == "Item") 
      {
        bool isChickenLeg = collision.gameObject.name.Contains("ChickenLeg");
        bool isWatermelon = collision.gameObject.name.Contains("Watermelon");
        bool isKiwi = collision.gameObject.name.Contains("Kiwi");
        bool isMeat = collision.gameObject.name.Contains("meat");
        bool isEgg = collision.gameObject.name.Contains("egg");
        //아이템 수량 UI 활성화
        itemQuantity.gameObject.SetActive(true);

        //획득한 음식 수량 +1, 음식 스택량 업데이트
        itemQuantity.ItemCount++;
        itemQuantity.UpdateItemQuantity();

            // 사운드
            PlaySound("getFood");
        if(isChickenLeg) 
        {
          Debug.Log("치킨을 하나 획득하셨습니다!");

          //치킨 하나당 100점 추가
          gameManager.stagePoint += 100;
        }

        if(isWatermelon) 
        {
          Debug.Log("수박을 하나 획득하셨습니다!");

          //수박 하나당 150점 추가
          gameManager.stagePoint += 150;
        }

        if(isKiwi) 
        {
          Debug.Log("키위를 하나 획득하셨습니다!");

          //키위 하나당 200점 추가
          gameManager.stagePoint += 200;
        }
        if (isMeat)
        {
             Debug.Log("고기를 하나 획득하셨습니다!");

             //달걀 하나당 300점 추가
             gameManager.stagePoint += 300;
        }
        if (isEgg)
        {
             Debug.Log("달걀을 하나 획득하셨습니다!");

             //달걀 하나당 400점 추가
             gameManager.stagePoint += 400;
        }
      }

        if (collision.gameObject.tag == "Pot") {
          bool isSnakePot = collision.gameObject.name.Contains("SnakePot");
          bool isBonusPot = collision.gameObject.name.Contains("BonusPot");

            // 사운드
            PlaySound("getBox");
          if(isSnakePot) 
          {
            Debug.Log("뱀이 있는 항아리를 열었습니다!");
            
            //3초동안 무적 타이밍
            OnInvincible();
          }

          if(isBonusPot) {
            Debug.Log("보너스 점수가 있는 항아리를 열었습니다!");

            //보너스 점수가 있는 항아리 하나당 500점 추가
            gameManager.stagePoint += 500;
          }
        }

        if(!collision.gameObject.name.Contains("Game Manager") 
          && !collision.gameObject.name.Contains("Ladder"))
          {
            collision.gameObject.SetActive(false);
          }

        // 사다리
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
      }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
        }
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "jump":
                audioSource.clip = audioJump;
                break;
            case "getFood":
                audioSource.clip = audiogetFood;
                break;
            case "getBox":
                audioSource.clip = audiogetBox;
                break;
            case "die":
                audioSource.clip = audioDie;
                break;
        }
        audioSource.Play();
    }
    void OnInvincible()
    {
      gameObject.layer = 11;
      spriteRenderer.color = new Color(1, 1, 1, 0.4f);
      Invoke("OffInvincible", 3);
    }

    void OffInvincible()
    {
      gameObject.layer = 10;
      spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
      //Sprite Alpha
      spriteRenderer.color = new Color(1, 1, 1, 0.4f);
      //Sprite Flip Y
      spriteRenderer.flipY = true;
      //Collider Disable
      capsuleCollider.enabled = false;
      //Die Effect Jump
      rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        
    }

    public void VelocityZero()
    {
      rigid.velocity = Vector2.zero;
    }

}
