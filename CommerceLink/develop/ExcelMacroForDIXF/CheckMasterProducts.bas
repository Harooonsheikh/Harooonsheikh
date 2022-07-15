Attribute VB_Name = "Module2"
'***** initailaizing Global variables ******
Dim MasterIds() As String
Dim myIdsElements As Integer

Public Sub CheckMasterProducts()
Attribute CheckMasterProducts.VB_ProcData.VB_Invoke_Func = " \n14"

Dim RowNdx As Long
Dim ColNdx As Integer
Dim StartRow As Long
Dim EndRow As Long
Dim StartCol As Integer
Dim EndCol As Integer

Dim MasterIdIndex As Integer
Dim MasterId As String

Dim SizeColIndex As Integer
Dim BoxColIndex As Integer
Dim DispositionColIndex As Integer

Dim MasterFound As Boolean

myIndexElements = 0
myIdsElements = 0

With ActiveSheet.UsedRange
    StartRow = .Cells(1).Row
    StartCol = .Cells(1).Column
    EndRow = .Cells(.Cells.Count).Row
    EndCol = .Cells(.Cells.Count).Column
End With

'***** Reading Master required fields indexes *****
For ColNdx = StartCol To EndCol
    'getting index of master ids column
    If Cells(1, ColNdx).Value = "ProductNumber" Then
        MasterIdIndex = ColNdx
    End If
    
    'getting index of master required fields columns
    If Cells(1, ColNdx).Value = "Size" Then
        SizeColIndex = ColNdx
    End If
    If Cells(1, ColNdx).Value = "Box" Then
        BoxColIndex = ColNdx
    End If
    If Cells(1, ColNdx).Value = "Disposition" Then
        DispositionColIndex = ColNdx
    End If
Next ColNdx

'***** Reading Master Ids into list *****
For RowNdx = StartRow To EndRow
    If RowNdx > 1 Then
        MasterId = Cells(RowNdx, MasterIdIndex).Value
        If MasterId <> "" Then
        
            If (Not SearchMasterIdFromMasterArray(MasterId)) Then
                AddMasterIdsToArray (MasterId)
            End If
        
        End If
    End If
Next RowNdx

'***** Reading Rows to find master *****
If myIdsElements > 0 Then
            
    Do While i <= UBound(MasterIds)
        MasterId = MasterIds(i)
        
        MasterFound = False
                
        'Checking this master from complete file
        For RowNdx = StartRow To EndRow
            If RowNdx > 1 Then
                If Cells(RowNdx, MasterIdIndex).Value = MasterId And Cells(RowNdx, SizeColIndex).Value = "" And Cells(RowNdx, DispositionColIndex).Value = "" And Cells(RowNdx, BoxColIndex).Value = "" Then
                    MasterFound = True
                    GoTo MasterExist
                End If
            End If
        Next RowNdx
        
        If MasterFound Then
            'do noting & check next master id
        Else
            MsgBox ("Master Product " & MasterId & " does not exist in file.")
            GoTo ExitMacro
        End If
        
MasterExist:
        i = i + 1
    Loop

End If

MsgBox ("All Master Products exist in file.")

ExitMacro:
Application.ScreenUpdating = True

End Sub


'***** storing Master Ids in array *****
Public Sub AddMasterIdsToArray(ByVal stringToAdd As String)
        ReDim Preserve MasterIds(myIdsElements)
        MasterIds(myIdsElements) = stringToAdd
        myIdsElements = myIdsElements + 1
End Sub

'***** search Master Id from array *****
Public Function SearchMasterIdFromMasterArray(ByVal MasterId As String) As Boolean

    Dim i As Integer
    Dim found As Boolean
            
    If myIdsElements > 0 Then
            
        Do While i <= UBound(MasterIds) And Not found
            If (MasterIds(i) = MasterId) Then
                found = True
            Else
                i = i + 1
            End If
        Loop
    
    End If
    
    SearchMasterIdFromMasterArray = found
    
End Function
