Attribute VB_Name = "GlobalVars"
Public x As GenesisOCX.GenesisOCX

Public Function PrepareGenesis(pName As String) As GenesisOCX.GenesisOCX

Set x = CreateObject("GenesisOCX")
If Not x.open(pName) Then
    MsgBox "no puedo abrir el port"
End If

Set PrepareGenesis = x
End Function



