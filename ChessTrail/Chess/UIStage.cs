using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStage : MonoBehaviour
{
    private Stage _baseStage;
    
    public Image img;
    public GameObject perfectClearSpriteObj;
    public GameObject defaultClearSpriteObj;

    public void Init(Stage baseStage)
    {
        _baseStage = baseStage;
        img.sprite = _baseStage.Sprite;
        // [TODO] base Stage 정보에 맞게 설정 
    }
    
    /// <summary>
    /// 만약 클리어가 되는 경우 Broadcast Message로 호출
    /// </summary>
    public void UpdateInfo()
    {
        if(_baseStage.PerfectClear)
        {
            perfectClearSpriteObj.SetActive(true);
            defaultClearSpriteObj.SetActive(false);
        }
        else if (_baseStage.IsClear)
        {
            defaultClearSpriteObj.SetActive(true);
        }
    }
    
    public void B_Click()
    {
        GameManager.Instance.SelectStage(_baseStage);
    }
}