using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Beautify.Universal;

public class AnimationManager : MonoBehaviour
{
    public CinemachineVirtualCamera introCam;
    public Image fadeInOutPanel;
    public GameObject worldUI;
    public GameObject SkipKeyBoardUI;
    public GameObject SkipGamepadUI;
    public Animator skipAnimator;
    public bool canSkip = false;
    private float startSkipTime;
    private string playingAnimName;
    private RigBuilder _rigBuilder;
    private AnimationEventFunction _animationEventFunction;
    private LetterboxManager _letterboxManager;
    private Animator _playerAnimator;
    private Indicator _indicator;


    private Coroutine _resumeRigCoroutine;
    #region LifeCycle
    public void Init()
    {
        _indicator = FindAnyObjectByType<Indicator>();
        _rigBuilder = GameManager.Instance.playerMovementManager.GetComponent<RigBuilder>();
        _playerAnimator = GameManager.Instance.playerMovementManager.GetComponent<Animator>();
        _letterboxManager = FindAnyObjectByType<LetterboxManager>();
        _animationEventFunction = new AnimationEventFunction();
    }

    void Update()
    {
        _rigBuilder.layers[0].active = !GameManager.Instance.playerMovementManager.isGravityArea;
        _rigBuilder.layers[1].active = GameManager.Instance.playerMovementManager.isGravityArea;
        CheckGravityZone(GameManager.Instance.playerMovementManager.isGravityArea);
        _playerAnimator.SetBool("isGround", GameManager.Instance.playerMovementManager.isGround);
    }
    #endregion
    #region CutScene
    public void Intro()
    {
        SetLBox(0.5f);
        worldUI.SetActive(false);
        introCam.Priority = 12;
        GameManager.Instance.playerMovementManager.pointerPos = Vector3.right;
        _indicator.gameObject.SetActive(false);
        _animationEventFunction.StopPlayerMove();
        CancelRigWeight();
        SetIntroTrigger();
        GameManager.Instance.playerMovementManager.StopFallingCoroutine();
        StartCoroutine(EndCutScene());
    }


    private IEnumerator EndCutScene()
    {
        yield return new WaitForSeconds(1f);
        introCam.Priority = 10;
        yield return new WaitForSeconds(16f);
        ResumeRigWeight(2f);
        yield return new WaitForSeconds(2f);
        _indicator.gameObject.SetActive(true);
        _animationEventFunction.StartPlayerMove(false);
        worldUI.SetActive(true);
        float fade = 1f;
        float time = 2f;
        Color colorTmp;
        foreach (Image image in worldUI.transform.GetComponentsInChildren<Image>())
        {
            colorTmp = image.color;
            colorTmp.a = 0f;
            image.color = colorTmp;
            image.DOFade(fade, time).SetEase(Ease.InCirc);
        }
        foreach (TextMeshProUGUI tmp in worldUI.transform.GetComponentsInChildren<TextMeshProUGUI>())
        {
            colorTmp = tmp.color;
            colorTmp.a = 0f;
            tmp.color = colorTmp;
            tmp.DOFade(fade, time).SetEase(Ease.InCirc); ;
        }
    }

    public void TurnPlayer(float time)
    {
        StartCoroutine(AdjustRigWeightCouroutine(_rigBuilder.layers[2].rig, 0f, 1f, time));
    }
    public void TurnPlayerReverse()
    {
        StartCoroutine(AdjustRigWeightCouroutine(_rigBuilder.layers[2].rig, 1f, 0f, 0.5f));
    }
    public void ReleaseArmIK()
    {
        StartCoroutine(AdjustRigWeightCouroutine(_rigBuilder.layers[0].rig, 1f, 0f, 1f));
    }
    private IEnumerator AdjustRigWeightCouroutine(Rig rig, float startWeightValue, float endWeightValue, float time)
    {
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            rig.weight = Mathf.Lerp(startWeightValue, endWeightValue, currentTime / time);
            yield return null;
        }
    }
    #endregion
    #region Movement
    public void Move(float moveSpeed, bool isMove)
    {
        _playerAnimator.SetBool("isMove", isMove);
        _playerAnimator.SetFloat("Move", moveSpeed);
    }
    public void Reverse(bool isReverse)
    {
        _playerAnimator.SetFloat("isReverse", isReverse ? 1 : 0);
    }
    public void Jump()
    {
        _playerAnimator.SetTrigger("Jump");
    }
    public void Fall()
    {
        _playerAnimator.SetTrigger("Fall");
    }
    public void Land()
    {
        _playerAnimator.SetTrigger("Land");
    }
    public void Climb()
    {
        CancelRigWeight();
        _playerAnimator.SetTrigger("Climb");
    }
    public void ClimbEnd()
    {
        ResumeRigWeight(0.3f);
        _playerAnimator.SetTrigger("ClimbEnd");
    }
    public void EnterGravityZone()
    {
        _playerAnimator.SetTrigger("EnterGravityZone");
    }
    public void CheckGravityZone(bool isGravityZone)
    {
        _playerAnimator.SetBool("IsGravityZone", isGravityZone);
    }
    #endregion
    #region SubFunction
    public void EnableRootMotion()
    {
        _playerAnimator.applyRootMotion = true;
    }
    public void DisableRootMotion()
    {
        _playerAnimator.applyRootMotion = false;
    }
    public void CancelRigWeight()
    {
        if (_resumeRigCoroutine != null)
            StopCoroutine(_resumeRigCoroutine);
        _rigBuilder.layers[0].rig.weight = 0;
    }
    public void ResumeRigWeight(float time)
    {
        if (_resumeRigCoroutine != null)
            StopCoroutine(_resumeRigCoroutine);
        _resumeRigCoroutine = StartCoroutine(ResumeRigCoroutine(time));
    }
    private IEnumerator ResumeRigCoroutine(float time)
    {
        float currentTime = 0f;
        while (currentTime < time)
        {
            yield return null;
            currentTime += Time.deltaTime;
            _rigBuilder.layers[0].rig.weight = Mathf.Lerp(0f, 1f, currentTime / time);
        }
        _resumeRigCoroutine = null;
    }
    #endregion
    #region Production
    public void SetIntroTrigger()
    {
        _playerAnimator.SetTrigger("IntroTrigger");
    }
    public void SetLBox(float value)
    {
        _letterboxManager.lbox = value;
        BeautifySettings.settings.frameBandVerticalSize.value = value;
    }

    public void StartLerp00()
    {
        _letterboxManager.StartLerp00();
        FadeIn();
    }

    public void FadeIn()
    {
        fadeInOutPanel.color = new Color(0, 0, 0, 1);
        fadeInOutPanel.DOFade(0.0f, 2f).SetEase(Ease.InCirc);
    }
    public void Fadeout()
    {
        fadeInOutPanel.color = new Color(0, 0, 0, 0);
        fadeInOutPanel.DOFade(1f, 2f).SetEase(Ease.OutCirc);
    }
    public void SkipAnimation()
    {
        //skipAnimator = GetComponent<Animator>();
        if (skipAnimator != null && skipAnimator.GetCurrentAnimatorStateInfo(0).length * skipAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < startSkipTime && SteamIntegration.IsUnlockedAchievement("Ending"))
        {
            SetOffAllSkipUI();
            FadeIn();
            if (skipAnimator.name == "Title")
            {
                SoundManager.instance.TitleSoundSKip();
            }
            skipAnimator.PlayInFixedTime(playingAnimName, 0, startSkipTime);
            canSkip = false;
            skipAnimator = null;
        }
    }
    public void SetSkipAnimator(Animator animator)
    {
        skipAnimator = animator;
    }
    public void SetSkipStartTime(float time)
    {
        startSkipTime = time;
    }
    public void SetCanSkipTrue()
    {
        if (SteamIntegration.IsUnlockedAchievement("Ending"))
        {
            if (GameManager.Instance.CurrentDevice == GameManager.Device.PC)
            {
                SkipGamepadUI.SetActive(false);
                SkipKeyBoardUI.SetActive(true);
            }
            else if (GameManager.Instance.CurrentDevice == GameManager.Device.GamePad)
            {
                SkipGamepadUI.SetActive(true);
                SkipKeyBoardUI.SetActive(false);
            }
            canSkip = true;
        }
    }
    public void SetPlayingAnimName(string name)
    {
        playingAnimName = name;
    }

    public void SetOffAllSkipUI()
    {
        SkipGamepadUI.SetActive(false);
        SkipKeyBoardUI.SetActive(false);
    }
    #endregion
}
