using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVManager
{
    //private int numberOfParams = 4;

    private static string reportDirectoryName = "CSV_Reports";
    private static string reportFileName = "report.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = new string[8] {
        "TimeStamp",
        "Student_name",
        "Level_Num",
        "PlayerMoves", 
        "ErrorsMade",
        "Hints_count",
        "totalSolveTime",
        "instructReadingTime"
    };

#region Interactions

    public static void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();

        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                    finalString += reportSeparator;

                finalString += strings[i];
            }

            sw.WriteLine(finalString);
        }
    }

    // Creates a new report after we verified there is a directory but there is NO file.
    public static void CreateReport()
    {
        VerifyDirectory();  
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i=0; i < reportHeaders.Length; i++)
            {
                if (finalString != "")
                    finalString += reportSeparator;

                finalString += reportHeaders[i];
            }

            sw.WriteLine(finalString);

        }
    }

#endregion

#region Operartions

    static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }
    static void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
            CreateReport();
    }

#endregion

 #region Queries

    static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName;
    }
    static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }

 #endregion

}
