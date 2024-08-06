using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

public class CSVReader : Singleton<CSVReader>
{
    string csvFileName = "EventObjectTable";

    public void ReadCsvIntoEpisodes(List<Episode> episodes)
    {
        var textAsset = Resources.Load<TextAsset>(csvFileName);
        if (textAsset == null)
        {
            Debug.LogError($"Failed to load CSV file: {csvFileName}");
        }
        
        ParseCsv(textAsset, episodes);
    }

    private void ParseCsv(TextAsset textAsset, List<Episode> episodes)
    {
        string[] lines = textAsset.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 첫줄 제외
        {
            var episode = Episode.FromCsvLine(lines[i]);
            if (episode != null)
            {
                episodes.Add(episode);
            }
        }
    }
}
