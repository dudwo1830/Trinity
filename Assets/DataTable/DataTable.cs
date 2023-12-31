﻿using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public abstract class DataTable
{
    protected string filePath = string.Empty;

    public abstract void Load();

    public string GetExtensionLower()
    {
        return filePath.Split('.')[^1].ToLower();
    }

    public string GetExtensionUpper() 
    { 
        return filePath.Split(".")[^1].ToUpper(); 
    }
}
