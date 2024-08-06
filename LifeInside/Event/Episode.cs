
using System;
using System.IO.Enumeration;
using UnityEngine;

public class Episode
{
    public int level;  // 유아, 노년..
    
    public string name;
    public string location; // todo: enum
    public float probability ;

    public int happyGain;
    public int happyCost;

    public int moneyGain;
    public int moneyCost; // todo: 현재 1만원 등으로 입력되고 있음, 엑셀에서 숫자로 변경하기 to int

    public int fatigueGain;
    public int fatigueCost;

    public int timeCost;

    public string respawnPos;
    public bool defaultEpi;

    public string fileName;

    public bool hasMoreEvent;

    public Episode(int level_, string name_, string location_, float probability_, int happyGain_, int happyCost_, int moneyGain_, int moneyCost_, int fatigueGain_, int fatigueCost_, int timeCost_, string respawnPos_, bool defaultEpi_, string fileName_, bool hasMoreEvt_)
    {
        level = level_; 

        name        = name_;
        location    = location_;
        probability = probability_;

        happyGain = happyGain_;
        happyCost = happyCost_;

        moneyGain = moneyGain_;
        moneyCost = moneyCost_;

        fatigueGain = fatigueGain_;
        fatigueCost = fatigueCost_;

        timeCost   = timeCost_;

        respawnPos = respawnPos_;

        defaultEpi = defaultEpi_;

        fileName   = fileName_;

        hasMoreEvent = hasMoreEvt_;
    }


    public static Episode FromCsvLine(string csvLine)
    {
        try
        {
            if ( csvLine == "" ){ return null; } // 맨 마지막 빈 줄 처리

            string[] fields = csvLine.Split(',');
            
            return new Episode(
                level_      :   int.Parse(fields[0]), 
                name_       :             fields[1], 
                location_   :             fields[2], 
                probability_: float.Parse(fields[3]),
                happyGain_  :   int.Parse(fields[4]),
                happyCost_  :   int.Parse(fields[5]),
                moneyGain_  :   int.Parse(fields[6]),
                moneyCost_  :   int.Parse(fields[7]),
                fatigueGain_:   int.Parse(fields[8]),
                fatigueCost_:   int.Parse(fields[9]),
                timeCost_   :   int.Parse(fields[10]),
                respawnPos_ :             fields[11],
                defaultEpi_ :   int.Parse(fields[12]) == 1 ? true : false,
                fileName_   :             fields[13],
                hasMoreEvt_ :   int.Parse(fields[14]) == 1 ? true : false
            );            
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to parse CSV line: {csvLine} Error: {e}");
            return null;
        }
    }


    public override string ToString()
    {
        string str = "";

        //str = level + " "+name + " " + location + " " + probability + " " + happyGain + " " + happyCost + " " + moneyGain + " " + moneyCost + " " + fatigueGain + " " + fatigueCost + " " + timeCost;
        str = $"{level} {name} {location} {probability} {happyGain} {happyCost} {moneyGain} {moneyCost} {fatigueGain} {fatigueCost} {timeCost}";

        return str;
    }
}