using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;

public class WriteFeedback : MonoBehaviour
{
    public readonly string sheetID = "1K9KHWcWWCRfK7PO6JZf_6XYIDD2CPgqO8XDx1ns4ZbI";
    public readonly string associatedWorksheet = "CellShooter Feedback";


    private void Start()
    {
        Debug.Log("shart");
        SpreadsheetManager.Append(new GSTU_Search(sheetID, associatedWorksheet, "A2"), new ValueRange("5"), null);
    }
}
