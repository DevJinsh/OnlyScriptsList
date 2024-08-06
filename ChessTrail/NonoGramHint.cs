using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NonoGramHint : MonoBehaviour
{
    //노노그램 힌트 계산
    private int verCount; // False가 나오면 lineCount.Add(count)
	private int horCount;

    private List<int> verticalLineCount;
    private List<int> horizontalLineCount;

    private List<List<int>> verticalLines = new List<List<int>>();
    private List<List<int>> horizontalLines = new List<List<int>>();

    private List<TMP_Text> _verticalHints;
    private List<TMP_Text> _horizontalHints;

    //노노그램 힌트 표시
    [Header("Prefabs")]
    public GameObject verticalHintPrefabs;
    public GameObject horizontalHintPrefabs;
    [Header("Parent")]
    public GameObject verticalHints;
    public GameObject horizontalHints;

    void CalcurateHint(Stage stage)
	{
        int cellCount = (int)Mathf.Sqrt(stage.Map.Length);
        horCount = 0;
		verCount = 0;
        for (int i = 0; i < cellCount; i++)
		{
            horizontalLineCount = new List<int>();
            verticalLineCount = new List<int>();
            for (int j = 0; j < cellCount; j++)
			{
				//수평라인
				if (stage.Map[i, j] > 0)
				{
					horCount++;
                    if(j == cellCount - 1)
                    {
                        horizontalLineCount.Add(horCount);
                        horCount = 0;
                    }
				}
				else if(horCount != 0)
				{
					horizontalLineCount.Add(horCount);
					horCount = 0;
				}
                // 수직라인
                if (stage.Map[j, i] > 0)
                {
                    verCount++;
                    if (j == cellCount - 1)
                    {
                        verticalLineCount.Add(verCount);
                        verCount = 0;
                    }
                }
                else if (verCount != 0)
                {
                    verticalLineCount.Add(verCount);
                    verCount = 0;
                }
            }
            if(horizontalLineCount.Count == 0)
            {
                horizontalLineCount.Add(0);
            }
            if (verticalLineCount.Count == 0)
            {
                verticalLineCount.Add(0);
            }
            horizontalLines.Add(horizontalLineCount);
            verticalLines.Add(verticalLineCount);
        }
	}

    public void CreateHint(Stage stage)
    {
        horizontalLines.Clear();
        verticalLines.Clear();
        CalcurateHint(stage);
        initHintSpace();
        for(int i = 0; i < GameManager.Instance.tileManager.cellCount; i++)
        {
            var verobj = Instantiate(verticalHintPrefabs, verticalHints.transform);
            var horobj = Instantiate(horizontalHintPrefabs, horizontalHints.transform);
            foreach (var hint in verticalLines[i])
            {
                verobj.GetComponentInChildren<TMP_Text>().text += hint;
            }
            foreach (var hint in horizontalLines[i])
            {
                horobj.GetComponentInChildren<TMP_Text>().text += (hint + " ");
            }
        }
    }

    void initHintSpace()
    {
        int cellCount = GameManager.Instance.tileManager.cellCount;
        verticalHints.GetComponent<GridLayoutGroup>().constraintCount = cellCount;
        horizontalHints.GetComponent<GridLayoutGroup>().constraintCount = cellCount;
        float cellsize = GameManager.Instance.tileManager.nonoPanel.GetComponent<RectTransform>().sizeDelta.x / cellCount;
        verticalHints.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellsize, verticalHints.GetComponent<GridLayoutGroup>().cellSize.y);
        horizontalHints.GetComponent<GridLayoutGroup>().cellSize = new Vector2(horizontalHints.GetComponent<GridLayoutGroup>().cellSize.x, cellsize);
    }
}
