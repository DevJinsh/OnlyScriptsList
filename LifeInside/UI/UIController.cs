using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region PublicVariables
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI fatigueText;
    public TextMeshProUGUI moneyText;


    public TextMeshProUGUI dayText;
    public TextMeshProUGUI gameTimeText;
    public TextMeshProUGUI levelText;

    public Slider happySlider;
    public Slider fatigueSlider;

    public TextMeshProUGUI talkerText;
    public TextMeshProUGUI contentText;


    public GameObject dialogueWindow;
    public GameObject selectEventWindow;
    public GameObject interactionInfo;
    

    public GameObject mapPanel;
    public GameObject tutorialPanel;
    public GameObject tutorialPrevPanel;


    public UIEffect uIEffect;
    public Reaper reaper;



    #endregion

    #region PrivateVariables
    private float money;
    private float gameTime;
    private float hour;
    private float day;
    private string level;

#endregion

#region PublicMethods

    public void SetHappySlider(float value_)
    {
        happySlider.value = value_;
    }

    public void SetFatigueSlider(float value_)
    {
        fatigueSlider.value = value_;
    }

    public void SetMoneyText(int money_)
    {
        moneyText.text = money_.ToString();
    }


    public void EnableWindow()
    {
        if( !dialogueWindow.activeSelf ){
            dialogueWindow.SetActive(true);
        }
    }

    public void DisableWindow()
    {
        if( dialogueWindow.activeSelf ){
            dialogueWindow.SetActive(false);
        }
    }

    public void MapToggle()
    {
        mapPanel.SetActive( !mapPanel.activeSelf );
    }


    public void EnableTutorial()
    {
        Destroy(FindAnyObjectByType<MainMenu>().gameObject);
        if ( !tutorialPanel.activeSelf ){
            tutorialPrevPanel.SetActive(false);
            tutorialPanel.SetActive(true);
        }
    }
    public void DisableTutorial()
    {
        if( tutorialPanel.activeSelf ){
            tutorialPanel.SetActive(false);

            GameManager.I.StartMainGame();
        }
    }


    public void SetEpisodeInfo(Episode episode, bool canAffordMoeny, bool canAffordTime )
    {
        TMP_Text[] text = interactionInfo.GetComponentsInChildren<TMP_Text>();
        text[0].text = episode.location;
        text[1].text = episode.happyGain   > 0 ? "행복도+" : "행복도-";
        text[2].text = episode.fatigueGain > 0 ? "피로도+" : "피로도-";
        if (episode.moneyGain > 0)
        {
            text[3].text = "돈+";
        }
        else
        {
            if (canAffordMoeny)
            {
                text[3].text = (episode.moneyCost / 10000).ToString() + "만원 필요";
            }
            else
            {
                text[3].text = ((episode.moneyCost - GameManager.I.money)/10000).ToString() + "만원 부족";
            }
        }

        if ( canAffordTime ) //1시간 전에 막아야 분까지 포함되어 막을 수 있음
        {
            text[4].text =  "시간 부족";
        }
        else
        {
            text[4].text = episode.timeCost.ToString() + "시간 소요";
        }
    }

    public void SetEmptyInfo(string location)
    {
        TMP_Text[] text = interactionInfo.GetComponentsInChildren<TMP_Text>();
        text[0].text = location;
        text[1].text = GameManager.I.playerController.GetLevelStr();
        text[2].text = "이용불가";
        text[3].text = "";
        text[4].text = "";

    }





#endregion

#region PrivateMethods
    void Start()
    {
    }
    void FixedUpdate()
    {
        money     = GameManager.I.money;
        gameTime  = GameTime.I.gameTime;
        hour      = GameTime.curHour;
        day       = GameTime.I.day;
        gameTime  = Mathf.Floor(gameTime * 100f);
        level     = GameManager.I.playerController.GetLevelStr();


        if(moneyText != null){
            moneyText.text     = money.ToString();
        }
        if(gameTimeText != null){
            gameTimeText.text  = hour.ToString() + " : " + gameTime.ToString();
        }
        if(dayText != null){
            dayText.text  = "Day " + day.ToString();
        }

        if(levelText != null){
            levelText.text  = level;
        }

    }


    #endregion

}
