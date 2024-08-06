using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private GameObject _scoreBoard;

    private int _totalScore;

    private TMP_Text _time;
    private TMP_Text _keyItem;
    private TMP_Text _score;
    private TMP_Text _level;

    private void Start()
    {
        _scoreBoard = gameObject.FindChild<Transform>("ScoreBoard").gameObject;
        _time = _scoreBoard.gameObject.FindChild<TMP_Text>("Time");
        _keyItem = _scoreBoard.gameObject.FindChild<TMP_Text>("KeyItem");
        _score = _scoreBoard.gameObject.FindChild<TMP_Text>("Score");
        _level = _scoreBoard.gameObject.FindChild<TMP_Text>("Level");
    }
    public void Resulting(float time, int keyItem)
    {
        _totalScore = 0;
        _time.text = "Clear Time: " + (int)(time / 60f) + ":" + (int)(time % 60f);
        _keyItem.text = "Key Item : " + keyItem + "/10";
        int timeScore = 0;
        if ((int)time / 60f <= 5) timeScore = 30000;
        else if ((int)time / 60f <= 7) timeScore = 20000;
        else if ((int)time / 60f <= 10) timeScore = 10000;
        _totalScore = (keyItem * 2000 + timeScore);
        _score.text = "Score : " + _totalScore;

        if(_totalScore >= 40000)
            _level.text = "A";
        else if(_totalScore >= 30000)
            _level.text = "B";
        else if (_totalScore >= 20000)
            _level.text = "C";
        else if (_totalScore >= 10000)
            _level.text = "D";
        else
            _level.text = "E";
    }
}
