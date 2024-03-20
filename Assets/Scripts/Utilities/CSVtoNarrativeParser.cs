using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using System;

public class CSVtoNarrativeParser : MonoBehaviour
{

    /* #Guide to using csv _> C# asset:
string[][] grid = CsvParser2.Parse(csv.text);
after that you can get every data you need.
grid.length = number of rows;
grid[0].length = number of columns; */
    public TextAsset csv;
    public string[][] grid;   
    public string resultText;

    private bool rowSet = false;
    private string tempRow;
    private string tempColumn;
    private bool columnSet = false;
    void Start()
    {
        grid = CsvParser2.Parse(csv.text);
        //ReadInCSV();

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ReadInCSV()
    {
        Debug.Log("this is a csv read-in test. Cell a4 should hold " + grid[3][3]);
        //for on rows
        for (int r = 0; r < grid.Length; r++)
        {
            //for loop for events
                //events are the first column so grid[0]
            for (int c = 0; c < grid[0].Length; c++)
            {
                Debug.Log("Cell at column " + c + " row " + r + " contains " + grid[r][c]);
            }
        }
         
    }
    //TODO: need to figure out how this will be written into guided
        //maybe edit event switch to send to this function and have the function call the switches from guided bath? From new classes?
    public void CellContentsCall(int row, int column)
    {
        Debug.Log(grid[row][column]);
        resultText = grid[row][column];
    }
    
    //this functions pulls the input from the input fields and casts thme into strings
    public void LockInput(TMP_InputField input)
    {
        if (input.text.Length > 0)
        {
            Debug.Log(input.text + " has been entered");
            if(input.name == "RowInputField")
            {
                SetRow(input.text);
            }
            if(input.name == "ColumnInputField")
            {
                SetColumn(input.text);
            }
            
        }
        else if (input.text.Length == 0)
        {
            Debug.Log(input.name + " Input Empty");
        }
    }
//this function sets a temporary string using the row number
    public void SetRow(string row)
    {
        Debug.Log("Row set at " + tempRow);
        rowSet = true;
        tempRow = row;
        if(columnSet == true)
        Debug.Log("Row set at " + tempRow);
        {
            AssembleLocation(tempRow, tempColumn);
        }

    }

    public void SetColumn(string column)
    {
        Debug.Log("Column set at " + column);
        columnSet = true;
        tempColumn = column;
        if(rowSet == true)
        Debug.Log("Column set at " + tempColumn);
        {
            AssembleLocation(tempRow, tempColumn);
        }
    }

    void AssembleLocation(string row, string column)
    {
        Debug.Log(row + " " + column);
        var rowInt = Convert.ToInt32(row);
        var columnInt = Convert.ToInt32(column);
        CellContentsCall(rowInt, columnInt);
    }

    
}
