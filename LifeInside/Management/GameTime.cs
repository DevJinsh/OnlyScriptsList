using UnityEngine;
using System;

public class GameTime : Singleton<GameTime>
{
    public bool timeStop = false; // 이벤트 중 일때는 시간 정지

    public float gameTime;
    public float day;

    public const int startHour = 8;
    public const int endHour = 22;

    public static float curHour;

    DayNightSystem2D dayNightSystem;

    public static event Action hourChangedEvent;

    void Start()
    {
        day = 1;
        curHour = startHour;

        dayNightSystem = FindAnyObjectByType<DayNightSystem2D>();


        hourChangedEvent += AdjustLight;
    }

    void FixedUpdate()
    {
        if (!GameManager.I.playerController.isDeath)
        {
            TimeIncrease();
            CheckDayEnd();
        }
       
    }

    public void CostTime(int hour_)
    {
        curHour += hour_;
        hourChangedEvent?.Invoke();
    }

    private void TimeIncrease()
    {
        if (!timeStop) // 이벤트 중일때는 시간 정지
        {
            gameTime += Time.fixedDeltaTime / 33.3f;

            if (gameTime >= 0.6f)
            {
                gameTime = 0;
                curHour++;
                hourChangedEvent?.Invoke();
                AddFatiguePerHour();
                SubHappyPerHour();
            }
        }
    }


    private void DayIncrease()
    {
        day++;
        Debug.Log($"다음날이 되었습니다. Day {day} 시작 | DayIncrease() ");
    }


    private void CheckDayEnd()
    {
        if (curHour >= endHour)
        {
            Debug.Log($"하루가 다 지났다 : Day {day} 끝 | CheckDayEnd()");

            StopTime();
            GameManager.I.inputManager.LockInput();

            if (day >= 5)
            {
                if ( GameManager.I.CheckIfDeath() ){ return; }
            }
            if (day >= 4)
            {
                GameManager.I.uIController.reaper.EnableReaper();
                GameManager.I.playerController.IncrementDeathProb();
            }

            GameManager.I.playerController.ChangeCharacterRandom();
            ResetDay();
            DayIncrease();

            FlowTime();
            GameManager.I.inputManager.UnLockInput();

        }
    }

    private void AddFatiguePerHour(){
        GameManager.I.AddCharacterResource(0, _fatigue: 5, 0); // fatigue   += 5;
    }

    private void SubHappyPerHour()
    {
        GameManager.I.SubCharacterResource(_happy: 5,   0, 0); // happiness -= 5;
    }

    private void AdjustLight() // curHour 8~22
    {
        if      (curHour < 10){ dayNightSystem.dayCycle = DayCycles.Sunrise; }
        else if (curHour < 14){ dayNightSystem.dayCycle = DayCycles.Day;      }
        else if (curHour < 17){ dayNightSystem.dayCycle = DayCycles.Sunset;   }
        else if (curHour < 20){ dayNightSystem.dayCycle = DayCycles.Night;    }
        else if (curHour < 22){ dayNightSystem.dayCycle = DayCycles.Midnight; }
    }

    public float GetDay()
    {
        return day;
    }

    private void ResetDay()
    {
        Debug.Log("ResetDay");
        GameManager.I.playerController.ResetPos();
        
        curHour = 8;
        hourChangedEvent?.Invoke();
    }


    public void FlowTime()
    {
        Debug.Log("FlowTime");
        timeStop = false;
    }
    public void StopTime()
    {
        Debug.Log("StopTime");
        timeStop = true;
    }
}
