using UnityEngine;
using UnityEngine.InputSystem;


public class Inputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public bool select;
    public bool escape;
    public bool run;
    public bool testKey;
    public bool map;
    public bool interact;

    private bool isLockInput    = false;
    
    public PlayerController pController;

    void Start()
    {
        pController = GetComponent<PlayerController>();
    }

	public void OnMove(InputValue value)
	{
        if( isLockInput ){ GameManager.I.playerController.animator.SetBool("isMove", false); return; }

        move = value.Get<Vector2>();
	}
    
	public void OnSelect(InputValue value)
	{
		select = value.isPressed;
	}
    public void OnEscape(InputValue value)
	{
		escape = value.isPressed;
	}
    public void OnRun(InputValue value)
	{
		run = value.isPressed;
	}

    public void OnInteract(InputValue value)
    {
        if( isLockInput ){ return; }

        interact = value.isPressed;
        if(interact)
        {
            pController.Interact();
        }
    }

    public void OnTestKey(InputValue value)
    {
        testKey = value.isPressed;
        if(testKey)
        {

        }
    }

    public void OnMap(InputValue value)
    {
        map = value.isPressed;
        Debug.Log("value.isPressed " + value.isPressed);
        if(map)
        {
            Debug.Log("map" + map);

            GameManager.I.uIController.MapToggle();
        }
    }


    public void LockInput()
    {
        isLockInput = true;
        Debug.Log("LockInput");
    }

    public void UnLockInput()
    {
        isLockInput = false;
        Debug.Log("UnLockInput");
    }
}

