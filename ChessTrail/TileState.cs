using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileState : MonoBehaviour
{
    [Header("Sprite")]
    public Sprite defaultSprite;
    public Sprite correctSprite;
    public Sprite wrongSprite;
    public Sprite selectableSprite;
    public Sprite disableSprite;
    public Image currentImage;
    
    private State _currentState;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            switch (_currentState)
            {
                /*case State.Default:
                    currentImage.sprite = defaultSprite;
                    break;*/
                case State.Correct:
                    currentImage.sprite = correctSprite;
                    break;
                case State.Wrong:
                    currentImage.sprite = wrongSprite;
                    break;
                case State.Disable:
                    currentImage.sprite = disableSprite;
                    break;
            }
        }
    }


    public enum State
    {
        Default,
        Correct,
        Wrong,
        CanMove,
        Disable
    }
}
