VERSION 5.00
Begin VB.Form MainForm 
   BackColor       =   &H00FFC0C0&
   Caption         =   "Prueba CFNG Genesis"
   ClientHeight    =   4395
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   4560
   LinkTopic       =   "Form1"
   ScaleHeight     =   4395
   ScaleWidth      =   4560
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command5 
      Caption         =   "Probar"
      Height          =   375
      Left            =   3480
      TabIndex        =   8
      Top             =   120
      Width           =   735
   End
   Begin VB.TextBox tbPort 
      Appearance      =   0  'Flat
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   960
      TabIndex        =   7
      Text            =   "serial=com1:115200"
      Top             =   120
      Width           =   2415
   End
   Begin VB.CommandButton Command4 
      Caption         =   "Zeta"
      Enabled         =   0   'False
      Height          =   495
      Left            =   1200
      TabIndex        =   5
      Top             =   3360
      Width           =   2295
   End
   Begin VB.CommandButton Command3 
      Caption         =   "Ticket Factura 'B'"
      Enabled         =   0   'False
      Height          =   495
      Left            =   1200
      TabIndex        =   4
      Top             =   2760
      Width           =   2295
   End
   Begin VB.CommandButton Command2 
      Caption         =   "Ticket Factura 'A'"
      Enabled         =   0   'False
      Height          =   495
      Left            =   1200
      TabIndex        =   3
      Top             =   2160
      Width           =   2295
   End
   Begin VB.Timer timer 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   240
      Top             =   3840
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Ticket a CF"
      Enabled         =   0   'False
      Height          =   495
      Left            =   1200
      TabIndex        =   0
      Top             =   1560
      Width           =   2295
   End
   Begin VB.Label lbOcupado 
      BackStyle       =   0  'Transparent
      Caption         =   "Impresor Ocupado"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   375
      Left            =   960
      TabIndex        =   10
      Top             =   1080
      Visible         =   0   'False
      Width           =   2775
   End
   Begin VB.Label lbFaltaPapel 
      BackStyle       =   0  'Transparent
      Caption         =   "Falta Papel"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   14.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   375
      Left            =   1440
      TabIndex        =   9
      Top             =   3960
      Visible         =   0   'False
      Width           =   1815
   End
   Begin VB.Label Label2 
      AutoSize        =   -1  'True
      BackColor       =   &H00FFC0C0&
      Caption         =   "Port:"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   255
      Left            =   360
      TabIndex        =   6
      Top             =   120
      Width           =   495
   End
   Begin VB.Label lSerie 
      AutoSize        =   -1  'True
      BackColor       =   &H00FFC0C0&
      Caption         =   "(No detectado)"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   240
      Left            =   1800
      TabIndex        =   2
      Top             =   720
      Width           =   1275
   End
   Begin VB.Label Label1 
      AutoSize        =   -1  'True
      BackColor       =   &H00FFC0C0&
      Caption         =   "Genesis Serie:"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   240
      Left            =   360
      TabIndex        =   1
      Top             =   720
      Width           =   1365
   End
End
Attribute VB_Name = "MainForm"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim listo As Boolean
Dim WithEvents x As GenesisOCX.GenesisOCX
Attribute x.VB_VarHelpID = -1

Private Sub x_FaltaPapel()
    lbFaltaPapel.Visible = True
    lbOcupado.Visible = False
    DoEvents
End Sub

Private Sub x_Ocupado()
    lbOcupado.Visible = True
    DoEvents
End Sub

Private Sub x_LimpiarEventos()
    lbFaltaPapel.Visible = False
    lbOcupado.Visible = False
    DoEvents
End Sub

Private Sub Command1_Click()
Set x = PrepareGenesis(tbPort.Text)
If x.IsOpened Then
    
    ' Abre el ticket
    x.Execute (x.Ticket.tkabrir)
    
    ' Prepara y envia un item simle
    x.Ticket.tkitem.Input.Descrip = "ACEITE PATITO LATA"
    x.Ticket.tkitem.Input.Cantidad = 3
    x.Ticket.tkitem.Input.Unitario = 13.5
    x.Ticket.tkitem.Input.TasaIVA = 2100
    x.Execute (x.Ticket.tkitem)
    
    ' Agrega un medio de pago
    x.Ticket.tkpago.Input.Codigo = 7
    x.Ticket.tkpago.Input.Monto = 100
    x.Execute (x.Ticket.tkpago)
    
    ' Cierra la operacion
    x.Execute (x.Ticket.tkcerrar)
    
    ' Cierra el canal de comunicacion (opcional)
    x.Close
    
End If
End Sub

Private Sub Command2_Click()
Set x = PrepareGenesis(tbPort.Text)
If x.IsOpened Then
    
    ' Abre el ticket factura con datos de comprador
    x.TicketFactura.tfabrir.Input.NomCliente_1 = "Pescado SRL"
    x.TicketFactura.tfabrir.Input.DirecCliente_1 = "Solorzano 950"
    x.TicketFactura.tfabrir.Input.DirecCliente_2 = "Salta Capital"
    x.TicketFactura.tfabrir.Input.TipoDoc = Asc("T")
    x.TicketFactura.tfabrir.Input.NroDoc = "20168993278"
    x.TicketFactura.tfabrir.Input.RespIVA = Asc("I")
    x.Execute (x.TicketFactura.tfabrir)
    
    ' Prepara y envia un item simle
    x.TicketFactura.tfitem.Input.Descrip = "ACEITE PATITO LATA"
    x.TicketFactura.tfitem.Input.Cantidad = 3
    x.TicketFactura.tfitem.Input.Unitario = 13.5
    x.TicketFactura.tfitem.Input.TasaIVA = 2100
    x.Execute (x.TicketFactura.tfitem)
    
    ' Agrega un medio de pago
    x.TicketFactura.tfpago.Input.Codigo = 7
    x.TicketFactura.tfpago.Input.Monto = 100
    x.Execute (x.TicketFactura.tfpago)
    
    ' Cierra la operacion
    x.Execute (x.TicketFactura.tfcerrar)
    
    ' Cierra el canal de comunicacion (opcional)
    x.Close
    
End If

End Sub

Private Sub Command3_Click()
    
Set x = PrepareGenesis(tbPort.Text)
If x.IsOpened Then
    
    ' Abre el ticket factura con datos de comprador
    x.TicketFactura.tfabrir.Input.NomCliente_1 = "Alberto Fer"
    x.TicketFactura.tfabrir.Input.DirecCliente_1 = "Rivadavia 1"
    x.TicketFactura.tfabrir.Input.DirecCliente_2 = "CABA"
    x.TicketFactura.tfabrir.Input.TipoDoc = Asc("T")
    x.TicketFactura.tfabrir.Input.NroDoc = "20168993278"
    x.TicketFactura.tfabrir.Input.RespIVA = Asc("M")
    x.Execute (x.TicketFactura.tfabrir)
    
    ' Prepara y envia un item simle
    x.TicketFactura.tfitem.Input.Descrip = "ACEITE PATITO LATA"
    x.TicketFactura.tfitem.Input.Cantidad = 3
    x.TicketFactura.tfitem.Input.Unitario = 13.5
    x.TicketFactura.tfitem.Input.TasaIVA = 2100
    x.Execute (x.TicketFactura.tfitem)
    
    ' Agrega un medio de pago
    x.TicketFactura.tfpago.Input.Codigo = 7
    x.TicketFactura.tfpago.Input.Monto = 100
    x.Execute (x.TicketFactura.tfpago)
    
    ' Cierra la operacion
    x.Execute (x.TicketFactura.tfcerrar)
    
    ' Cierra el canal de comunicacion (opcional)
    x.Close
    
End If

End Sub

Private Sub Command4_Click()

Set x = PrepareGenesis(tbPort.Text)
If x.IsOpened Then
    
    ' Emite un cierre diario ZETA
    x.Execute (x.JornadaFiscal.jorzeta)
    x.Close
    
End If

End Sub

Private Sub Command5_Click()
Command1.Enabled = False
Command2.Enabled = False
Command3.Enabled = False
Command4.Enabled = False
lSerie.Caption = "Espere. Probando..."
timer.Enabled = True

End Sub

Private Sub timer_Timer()

timer.Enabled = False
Set x = PrepareGenesis(tbPort.Text)
If x.IsOpened Then

   lSerie.Caption = x.GetSerial()
   listo = True
   
   Command1.Enabled = True
   Command2.Enabled = True
   Command3.Enabled = True
   Command4.Enabled = True
   
   x.Close

Else

   lSerie.Caption = "Error abriendo port"

End If

End Sub
