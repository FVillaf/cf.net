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

Private Sub Command1_Click()

If PrepareGenesis(tbPort.Text) Then
    
    ' Abre el ticket
    x.execute (x.ticket.tkabrir)
    
    ' Prepara y envia un item simle
    x.ticket.tkitem.input.descrip = "ACEITE PATITO LATA"
    x.ticket.tkitem.input.cantidad = 3
    x.ticket.tkitem.input.unitario = 13.5
    x.ticket.tkitem.input.tasaiva = 2100
    x.execute (x.ticket.tkitem)
    
    ' Agrega un medio de pago
    x.ticket.tkpago.input.codigo = 7
    x.ticket.tkpago.input.monto = 100
    x.execute (x.ticket.tkpago)
    
    ' Cierra la operacion
    x.execute (x.ticket.tkcerrar)
    
    ' Cierra el canal de comunicacion (opcional)
    x.Close
    
End If
End Sub

Private Sub Command2_Click()
If PrepareGenesis(tbPort.Text) Then
    
    ' Abre el ticket factura con datos de comprador
    x.ticketfactura.tfabrir.input.nomcliente_1 = "Pescado SRL"
    x.ticketfactura.tfabrir.input.direccliente_1 = "Solorzano 950"
    x.ticketfactura.tfabrir.input.direccliente_2 = "Salta Capital"
    x.ticketfactura.tfabrir.input.tipodoc = Asc("T")
    x.ticketfactura.tfabrir.input.nrodoc = "20168993278"
    x.ticketfactura.tfabrir.input.respiva = Asc("I")
    x.execute (x.ticketfactura.tfabrir)
    
    ' Prepara y envia un item simle
    x.ticketfactura.tfitem.input.descrip = "ACEITE PATITO LATA"
    x.ticketfactura.tfitem.input.cantidad = 3
    x.ticketfactura.tfitem.input.unitario = 13.5
    x.ticketfactura.tfitem.input.tasaiva = 2100
    x.execute (x.ticketfactura.tfitem)
    
    ' Agrega un medio de pago
    x.ticketfactura.tfpago.input.codigo = 7
    x.ticketfactura.tfpago.input.monto = 100
    x.execute (x.ticketfactura.tfpago)
    
    ' Cierra la operacion
    x.execute (x.ticketfactura.tfcerrar)
    
    ' Cierra el canal de comunicacion (opcional)
    x.Close
    
End If

End Sub

Private Sub Command3_Click()
    
If PrepareGenesis(tbPort.Text) Then
    
    ' Abre el ticket factura con datos de comprador
    x.ticketfactura.tfabrir.input.nomcliente_1 = "Alberto Fer"
    x.ticketfactura.tfabrir.input.direccliente_1 = "Rivadavia 1"
    x.ticketfactura.tfabrir.input.direccliente_2 = "CABA"
    x.ticketfactura.tfabrir.input.tipodoc = Asc("T")
    x.ticketfactura.tfabrir.input.nrodoc = "20168993278"
    x.ticketfactura.tfabrir.input.respiva = Asc("M")
    x.execute (x.ticketfactura.tfabrir)
    
    ' Prepara y envia un item simle
    x.ticketfactura.tfitem.input.descrip = "ACEITE PATITO LATA"
    x.ticketfactura.tfitem.input.cantidad = 3
    x.ticketfactura.tfitem.input.unitario = 13.5
    x.ticketfactura.tfitem.input.tasaiva = 2100
    x.execute (x.ticketfactura.tfitem)
    
    ' Agrega un medio de pago
    x.ticketfactura.tfpago.input.codigo = 7
    x.ticketfactura.tfpago.input.monto = 100
    x.execute (x.ticketfactura.tfpago)
    
    ' Cierra la operacion
    x.execute (x.ticketfactura.tfcerrar)
    
    ' Cierra el canal de comunicacion (opcional)
    x.Close
    
End If

End Sub

Private Sub Command4_Click()

If PrepareGenesis(tbPort.Text) Then
    
    ' Emite un cierre diario ZETA
    x.execute (x.jornadafiscal.jorzeta)
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
If PrepareGenesis(tbPort.Text) Then

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
