﻿using System.Text.RegularExpressions;

namespace VoiceGradeApi.Util;

public class TextParser
{
    public List<string> ParseData(string allData)
    {
        allData = ConvertToNumbers(allData.ToLower());
        List<string> separatedElements = allData.Trim().Split("\n").Where(x => x.Trim().Length != 1).ToList(); 
        return separatedElements;
    }

    private static string ConvertToNumbers(string originalString)
    {
        //Dictionary to convert words to numbers
        Dictionary<string, string> numberTable = new Dictionary<string, string>
        {
            {"один", "1"}, {"кол", "1"}, {"единичка", "1"},
            {"два", "2"}, {"двойка", "2"}, {"двоечка", "2"}, {"неудовлетворительно", "2"},
            {"три", "3"}, {"тройка", "3"}, {"троечка", "3"}, {"удовлетворительно", "3"},
            {"четыре", "4"}, {"четверка", "4"}, {"четверочка", "4"}, {"хорошо", "4"},
            {"пять", "5"}, {"пятерка", "5"}, {"пятерочка", "5"}, {"опять", "5"}, {"отлично", "5"},
            {"шесть", "6"}, {"семь", "7"}, {"восемь", "8"}, {"девять", "9"},
            {"десять", "10"}, {"одиннадцать", "11"}, {"двенадцать", "12"}
        };
        
        //Replace words with numbers and separate pupils with newline symbol
        foreach (KeyValuePair<string, string> pair in numberTable)
        {
            Regex rgx = new Regex(@$"\s*{pair.Key}\s*");
            originalString = rgx.Replace(originalString, " " + pair.Value + "\n");
        }

        return originalString.ToUpper();
    }
}