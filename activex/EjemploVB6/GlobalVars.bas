Attribute VB_Name = "GlobalVars"
Public x As GenesisOCX.GenesisCF

Public Function PrepareGenesis(pName As String) As GenesisOCX.GenesisCF

Set x = CreateObject("GenesisCF")
If Not x.Open(pName) Then
    MsgBox "no puedo abrir el port"
End If

Set PrepareGenesis = x

End Function



