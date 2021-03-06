Attribute VB_Name = "Module1"
Sub ExportToTextFile()

Dim WholeLine As String
Dim FNum As Integer
Dim RowNdx As Long
Dim ColNdx As Integer
Dim StartRow As Long
Dim EndRow As Long
Dim StartCol As Integer
Dim EndCol As Integer
Dim CellValue As String

Dim file As Variant
   
Application.ScreenUpdating = False
On Error GoTo EndMacro:
FNum = FreeFile

file = Left(ActiveWorkbook.FullName, InStrRev(ActiveWorkbook.FullName, ".") - 1) & "-ReadyToImport.txt"
Set file = CreateObject("Scripting.FileSystemObject").CreateTextFile(file, True, True)

With ActiveSheet.UsedRange
    StartRow = .Cells(1).Row
    StartCol = .Cells(1).Column
    EndRow = .Cells(.Cells.Count).Row
    EndCol = .Cells(.Cells.Count).Column
End With

For RowNdx = StartRow To EndRow

    WholeLine = ""
    For ColNdx = StartCol To EndCol
    
        If Cells(RowNdx, ColNdx).Value = "" Then
            CellValue = Chr(34) & Chr(34)
            WholeLine = WholeLine & vbTab
        Else
           CellValue = Cells(RowNdx, ColNdx).Value
           WholeLine = WholeLine & CellValue & vbTab
        End If
                
    Next ColNdx
    WholeLine = Left(WholeLine, Len(WholeLine) - Len(vbTab))
    
    file.WriteLine WholeLine
    
Next RowNdx

EndMacro:
On Error GoTo 0
Application.ScreenUpdating = True
file.Close

End Sub

