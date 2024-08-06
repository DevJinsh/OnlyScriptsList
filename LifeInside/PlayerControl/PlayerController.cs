using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Inputs inputManager; // todo start 에서 해주기
    public EventObject touchedEvtObj;
    public Animator animator;
    
    public List<GameObject> playerLevel;

    private float moveSpeed        = 10f;
    private const float walkSpeed  = 10f;
    private const float runSpeed   = 13f;
    public int level;

    public float deathProb = 0f;
    public bool isDeath;

    public Transform resetPoint;


    void Start()
    {
        inputManager = GetComponent<Inputs>();
        animator     = GetComponent<Animator>();
        level = 1;
        animator.SetInteger("Person", level);
    }

    void FixedUpdate()
    {
            if (inputManager.move != Vector2.zero)
            {
                Move();

                
                animator.SetBool("isMove", true);
                if(inputManager.move.x < 0){ // 좌우 방향 전환
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }else if(inputManager.move.x > 0){
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else
            {
                animator.SetBool("isMove", false);
            }
    }


    public void LevelChanged()
    {
        touchedEvtObj = null;
    }


    void Move()
    {
        if ( inputManager.run ){ moveSpeed = runSpeed;  }
        else{                    moveSpeed = walkSpeed; }

        Vector2 moveVelocity = inputManager.move.normalized * moveSpeed;
        transform.position += new Vector3(moveVelocity.x, moveVelocity.y, 0) * Time.deltaTime;
    }

    public void Interact()
    {
        Debug.Log("Interact");
        

        if( inputManager.move != Vector2.zero){ return;}
        if( !touchedEvtObj ){ return; }


        
        //if(level ) // todo 레벨 확인 필요

        GameManager.I.eventManager.EventStart(touchedEvtObj);
    }


    public void ChangeCharacterRandom()
    {
        if (GameTime.I.GetDay() < 4)
        {
            level++;
        }
        else        
        {
            int randomNumber = Random.Range(0, 5); //0 ~ 4
            level = randomNumber;
        }

        
       
        Debug.Log("현재 캐릭터 : " + level);
        animator.SetInteger("Person", level);
    }

    public void ResetPos()
    {
        if(resetPoint != null){
            Debug.Log(" 캐릭터의 위치가 처음으로 돌아갔다" );
            transform.position = resetPoint.position;
        }else{
            Debug.Log(" resetPoint is null" );
        }
    }


    public void IncrementDeathProb()
    {
        if(deathProb == 0f) //날마다 죽음 확률 3% 상승 // 돌연사확률
        {
            deathProb = 3f;
        }
        else
        {
            deathProb *= 1.5f;
        }
    }



    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Interaction"))
        {
            touchedEvtObj = collider.GetComponent<EventObject>();
            touchedEvtObj.JudgeInfo();
            touchedEvtObj.isTouching = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.CompareTag("Interaction"))
        {
            touchedEvtObj = collider.GetComponent<EventObject>();
            GameManager.I.uIController.interactionInfo.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, touchedEvtObj.transform.position);
            touchedEvtObj.ShowInfo();
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if(collider.CompareTag("Interaction"))
        {
            if( touchedEvtObj != null){
                touchedEvtObj.isTouching = false;
                touchedEvtObj.HideInfo();
                touchedEvtObj = null;
            }
        }
    }


    public string GetLevelStr(){
        return LevelToStr(this.level);
    }

    static string LevelToStr(int level_)
    {
        switch(level_)
        {
            case 0:
            return "유아";
            case 1:
            return "초등학생";
            case 2:
            return "청년";
            case 3:
            return "중년";
            case 4:
            return "노인";
            default:
            return "";
        }
    }
}
