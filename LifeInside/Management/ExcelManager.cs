using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;

using System.IO;
using System.Data;

public class ExcelManager : MonoBehaviour
{

    public void initExcelMan(List<Episode> episodeAll_, Dictionary<string, string> evtNameFileDic_)
    {
        Debug.Log("init Excel Man");

        GetData(episodeAll_, evtNameFileDic_);
    }


    private void GetData(List<Episode> episodeAll_, Dictionary<string, string> evtNameFileDic_)
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "EventObjectTable.xlsx"); // EventObjectTable.xlsx

        DataSet   result = ReadExcelFile(filePath);

        DataTable sheet0 = result.Tables[0];        // 시트0 아기
        DataTable sheet1 = result.Tables[1];        // 시트1 어린이
        DataTable sheet2 = result.Tables[2];        // 시트2 청년
        DataTable sheet3 = result.Tables[3];        // 시트3 중년
        DataTable sheet4 = result.Tables[4];        // 시트4 노년

        DataTable sheet5 = result.Tables[5];        // 시트5 컷신데이터


        ReadTableFromSheet(episodeAll_, sheet0, 0);  //"아기");
        ReadTableFromSheet(episodeAll_, sheet1, 1); //"어린이");
        ReadTableFromSheet(episodeAll_, sheet2, 2); //"청년");
        ReadTableFromSheet(episodeAll_, sheet3, 3); //"중년");
        ReadTableFromSheet(episodeAll_, sheet4, 4); //"노년");

        ReadTableFromCutsceneSheet( evtNameFileDic_, sheet5 );

    }

    void ReadTableFromSheet(List<Episode> episodeAll_, DataTable sheet, int level)
    {
        int cnt = 0;
        // sheet.Rows.Count // 17 중간 빈 라인 포함. 데이터가 있는 끝까지.
        for (int i = 2; i < sheet.Rows.Count; i++) // i=2 첫줄, 두번째줄 생략
        {
            if (sheet.Rows[i][1].ToString() == "") break; // 이벤트이름 빈칸이면 그 아래 정보는  가비지

            // Episode episode = new Episode(sheet.Rows[i][1].ToString(), sheet.Rows[i][2].ToString(), float.Parse(sheet.Rows[i][3].ToString()), int.Parse(sheet.Rows[i][4].ToString()), int.Parse(sheet.Rows[i][5].ToString()),
            //                                 int.Parse(sheet.Rows[i][6].ToString()), int.Parse(sheet.Rows[i][7].ToString()), int.Parse(sheet.Rows[i][8].ToString()), int.Parse(sheet.Rows[i][9].ToString()), int.Parse(sheet.Rows[i][10].ToString()), level, sheet.Rows[i][11].ToString(), int.Parse(sheet.Rows[i][12].ToString() ) );


            //todo
            //Episode episode = new Episode( , , , , , ,);

            //episodeAll_.Add( episode );

            ++cnt;
        }
        Debug.Log( " episode all Created : " + level + " " + cnt);
    }

    void ReadTableFromCutsceneSheet(Dictionary<string, string> evtNameFileDic_, DataTable sheet)
    {
        int cnt = 0;

        // sheet.Rows.Count // 17 중간 빈 라인 포함. 데이터가 있는 끝까지.
        for (int i = 2; i < sheet.Rows.Count; i++) // i=2 첫 줄과 두번째 줄 생략
        {
            if (sheet.Rows[i][1].ToString() == "") break; // 이벤트이름 빈칸이면 그 아래 정보는  가비지

            // evtNameFileDic.Add(evtName, jsonFile);
            //evtNameFileDic_.Add( sheet.Rows[i][1].ToString(), sheet.Rows[i][2].ToString()); 


            //evtNameFileDic_.Add(  , ); 
        
            ++cnt;
        }

        Debug.Log("evTNameFileDic Created : " + cnt);
    }

    DataSet ReadExcelFile(string filePath)
    {
        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        //UseHeaderRow = true
                    }
                });
                return result;
            }
        }
    }
}