Attribute VB_Name = "Module1"
'***** initailaizing Global variables ******
Dim MandatoryFieldsName() As String
Dim myStringElements As Integer

Dim MandatoryFields() As Integer
Dim myIntegerElements As Integer

Public Sub ExportToTextFileWithMandatoryCheck()

'***** Storing Mandatroy fields into array *****
myStringElements = 0
AddElementToStringArray ("Catalog")
AddElementToStringArray ("CategoryHierarchyName")
AddElementToStringArray ("EcoResCategory_Name")
AddElementToStringArray ("ProductNumber")
AddElementToStringArray ("adjustableBaseSuitable")
AddElementToStringArray ("certiPUR")
AddElementToStringArray ("coolingGel")
AddElementToStringArray ("customDescription")
AddElementToStringArray ("ecoFriendly")
AddElementToStringArray ("hypoAllergenic")
AddElementToStringArray ("isMFICore")
AddElementToStringArray ("isParcelable")
AddElementToStringArray ("lowMotionTransfer")
AddElementToStringArray ("mattressHeight")
AddElementToStringArray ("minOrderQuantity")
AddElementToStringArray ("organic")
AddElementToStringArray ("pageDescription")
AddElementToStringArray ("pageTitle")
AddElementToStringArray ("pageKeywords")
AddElementToStringArray ("pdpWarrantySidebar")
AddElementToStringArray ("productFeatures1")
AddElementToStringArray ("productFeatures2")
AddElementToStringArray ("productFeatures3")
AddElementToStringArray ("searchable")
AddElementToStringArray ("searchPlacement")
AddElementToStringArray ("searchRank")
AddElementToStringArray ("ship_method")
AddElementToStringArray ("showPDPSizeHelp")
AddElementToStringArray ("sitemap_changefrequency")
AddElementToStringArray ("siteMap_Included_flag")
AddElementToStringArray ("siteMap_Priority")
AddElementToStringArray ("ventilated")
AddElementToStringArray ("zone_code")
AddElementToStringArray ("taxClassID")
AddElementToStringArray ("shortDescription")

'***** initailaizing local variables *****
Dim WholeLine As String
Dim FNum As Integer
Dim RowNdx As Long
Dim ColNdx As Integer
Dim StartRow As Long
Dim EndRow As Long
Dim StartCol As Integer
Dim EndCol As Integer
Dim CellValue As String
Dim ErrorFlag As Boolean
ErrorFlag = False

Dim file As Variant
Dim fileName As String

myIntegerElements = 0

   
Application.ScreenUpdating = False
On Error GoTo EndMacro:
FNum = FreeFile

'***** Creating File *****
fileName = Left(ActiveWorkbook.FullName, InStrRev(ActiveWorkbook.FullName, ".") - 1) & "-ReadyToImport.txt"
file = Left(ActiveWorkbook.FullName, InStrRev(ActiveWorkbook.FullName, ".") - 1) & "-ReadyToImport.txt"
Set file = CreateObject("Scripting.FileSystemObject").CreateTextFile(file, True, True)

With ActiveSheet.UsedRange
    StartRow = .Cells(1).Row
    StartCol = .Cells(1).Column
    EndRow = .Cells(.Cells.Count).Row
    EndCol = .Cells(.Cells.Count).Column
End With

'***** Reading Madatory fields *****
For ColNdx = StartCol To EndCol
    If SearchMandatoryFields(Cells(1, ColNdx).Value) Then
        AddElementToIntegerArray (ColNdx)
    End If
Next ColNdx

'***** Reading data *****
For RowNdx = StartRow To EndRow

    WholeLine = ""
    For ColNdx = StartCol To EndCol
    
        If Cells(RowNdx, ColNdx).Value = "" Then
            If SearchIndexFromMandatoryFieldsArray(ColNdx) Then
                MsgBox ("The mandatory field value is missing on column " & Cells(1, ColNdx).Value & " Row No " & RowNdx & ". Unable to export file with this ERROR!")
                ErrorFlag = True
                GoTo ExitMacro
            Else
                CellValue = Chr(34) & Chr(34)
                WholeLine = WholeLine & vbTab
            End If
        Else
           CellValue = Cells(RowNdx, ColNdx).Value
           WholeLine = WholeLine & CellValue & vbTab
        End If
                
    Next ColNdx
    WholeLine = Left(WholeLine, Len(WholeLine) - Len(vbTab))
    
    file.WriteLine WholeLine
    
Next RowNdx

'***** Error *****
EndMacro:
On Error GoTo 0
Application.ScreenUpdating = True
file.Close

'***** Exit with no file generation *****
ExitMacro:
Application.ScreenUpdating = True
file.Close
If ErrorFlag Then
    RemoveFile (fileName)
End If

End Sub

'***** Delete created file *****
Public Sub RemoveFile(ByVal fileName As String)
    'Check that file exists
    If Len(Dir$(fileName)) > 0 Then
        'First remove readonly attribute, if set
        SetAttr fileName, vbNormal
        'Then delete the file
         Kill fileName
    End If
End Sub

'***** storing column index in array *****
Public Sub AddElementToIntegerArray(ByVal integerToAdd As Integer)
        ReDim Preserve MandatoryFields(myIntegerElements)
        MandatoryFields(myIntegerElements) = integerToAdd
        myIntegerElements = myIntegerElements + 1
End Sub

'***** storing column name in array *****
Public Sub AddElementToStringArray(ByVal stringToAdd As String)
        ReDim Preserve MandatoryFieldsName(myStringElements)
        MandatoryFieldsName(myStringElements) = stringToAdd
        myStringElements = myStringElements + 1
End Sub

'***** search column index from array *****
Public Function SearchIndexFromMandatoryFieldsArray(ByVal searchTerm As Integer) As Boolean

    Dim i As Integer
    Dim found As Boolean
    
    Do While i <= UBound(MandatoryFields) And Not found
        If (MandatoryFields(i) = searchTerm) Then
            found = True
        Else
            i = i + 1
        End If
    Loop
    
    SearchIndexFromMandatoryFieldsArray = found
    
End Function

'***** search column name from array *****
Public Function SearchMandatoryFields(ByVal searchTerm As String) As Boolean

    Dim i As Integer
    Dim found As Boolean
    
    Do While i <= UBound(MandatoryFieldsName) And Not found
        If (MandatoryFieldsName(i) = searchTerm) Then
            found = True
            GoTo BreakPoint
        Else
            i = i + 1
        End If
    Loop
    
BreakPoint:
    SearchMandatoryFields = found
    
End Function




