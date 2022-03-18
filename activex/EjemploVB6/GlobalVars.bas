Attribute VB_Name = "GlobalVars"
Public x As Object

Public Function PrepareGenesis(pName As String) As Boolean

Set x = CreateObject("moretti.fiscalproto")
If Not x.open(pName) Then
    MsgBox "no puedo abrir el port"
    PrepareGenesis = False
Else
    PrepareGenesis = True
End If
End Function


