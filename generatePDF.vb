Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.tool.xml
Public Class generatePDF
    Public Property nmArquivo As String
    Public Property binData As Byte()

    Function ClienteouBeneficiario(obj As Object)
        If obj.CD_CLIENTE = obj.CD_BENEFICIARIO Then
            Return True
        Else
            Return False
        End If
    End Function

    Function GerarCss() As String
        Dim codigoCss As String

        codigoCss &= ".container{"
        codigoCss &= "width:100%;"
        codigoCss &= "height:auto;"
        codigoCss &= "}"

        codigoCss &= ".cabecario{"
        codigoCss &= "border:solid;"
        codigoCss &= "padding:5px;"
        codigoCss &= "margin:1px;"
        codigoCss &= "height:40px;"
        codigoCss &= "}"

        codigoCss &= ".cabecarioTb td tr{"
        codigoCss &= "bordersoft-light;"
        codigoCss &= "text-align:center;"
        codigoCss &= "background:red;"
        codigoCss &= "}"

        codigoCss &= ".headerTb{"
        codigoCss &= "background-color: #b0c4de;"
        codigoCss &= "border: 1px;"
        codigoCss &= "border-collapse: collapse;"
        codigoCss &= "border:solid;"
        codigoCss &= "border-color: lightgray;"
        codigoCss &= "border-style: solid;"
        codigoCss &= "color: #003366;"
        codigoCss &= "font-family: Arial, Helvetica, sans-serif;"
        codigoCss &= "font-size: 11pt;"
        codigoCss &= "font-weight: bold;"
        codigoCss &= "height: 35px;"
        codigoCss &= "letter-spacing: 1px;"
        codigoCss &= "margin-bottom: 10px;"
        codigoCss &= "margin-top: 10px;"
        codigoCss &= "text-align: center;"
        codigoCss &= "width: 100%;"
        codigoCss &= "vertical-align: middle;"
        codigoCss &= "padding:10px;"
        codigoCss &= "}"

        codigoCss &= ".labelNegrito {"
        codigoCss &= "color: #003366;"
        codigoCss &= "font-family: Verdana, Helvetica, sans-serif;"
        codigoCss &= "font-size: 11px;"
        codigoCss &= "font-weight: bold;"
        codigoCss &= "height: 3px;"
        codigoCss &= "vertical-align: middle;"
        codigoCss &= " }"

        codigoCss &= ".bodyTb  tr td{"
        codigoCss &= "color: #003366;"
        codigoCss &= "font-family: Verdana, Helvetica, sans-serif;"
        codigoCss &= "font-size: 11px;"
        codigoCss &= "height: 3px;"
        codigoCss &= "vertical-align: middle;"
        codigoCss &= " }"

        codigoCss &= ".lblHoraCabecario {"
        codigoCss &= "font-family: Verdana, Helvetica, sans-serif;"
        codigoCss &= "font-size: 11px;"
        codigoCss &= "vertical-align: middle;"
        codigoCss &= "width: 250px;"
        codigoCss &= " }"



        Return codigoCss
    End Function

    Function gerarErroimpresao(MSG As String)
        Dim html As String
        Dim css As String
        Dim documento As GeraResumoPedido = New GeraResumoPedido()

        html &= "<html>"
        html &= "<head>"
        html &= "</head>"
        html &= "<body>"

        html &= "<div>"
        html &= "<div Class=""centralizar"">"
        html &= "<p Class=""titulo"">"
        html &= " " & MSG & " "
        html &= "</p>"
        html &= "</div>"
        html &= "</div>"
        html &= "</body>"

        css &= ".centralizar{"
        css &= "position: absolute;"
        css &= "top: 50%;"
        css &= "left: 50%;"
        css &= "margin-right: -50%;"
        css &= "transform: translate(-50%, -50%);"
        css &= "border-bottom:solid;"
        css &= "border-color:#b4d400;"
        css &= " }"
        css &= "div p{"
        css &= "font-weight: bold;"
        css &= "font-size:15px;"
        css &= "font-family: Arial, Helvetica, sans-serif;"
        css &= "}"

        documento.binData = HtmlParaPdf(html, css)
        documento.nmArquivo = "Erro Resumo do Pedido.pdf"
        Return documento
    End Function

    Function GerarHtml(LISTA_DADOS As List(Of Object))
        Dim titulo1 As String = "Plataforma"
        Dim titulo2 As String = "Dados do Propietário"
        Dim titulo3 As String = "Cliente"
        Dim titulo4 As String = "Contatos em caso de Roubo/Emergência"
        Dim titulo5 As String = "Detalhes Agendamento"
        Dim titulo6 As String = "Local Instalação"
        Dim titulo7 As String = "Serviços"

        Dim codigoHtml As String
        Dim codigoCss As String
        Dim validacao As Boolean = ClienteouBeneficiario(LISTA_DADOS(8))

        codigoHtml &= "<html>"
        codigoHtml &= "<head>"
        codigoHtml &= "</head>"
        codigoHtml &= "<body>"
        codigoHtml &= "<div class='container'>"
        codigoHtml &= "<!-- container principal que abrange toda a pagina -->"

#Region "Cabeçario do resumo do Pedido"
        codigoHtml &= "<div class='cabecario'>"
        codigoHtml &= "<table class='cabecarioTb'>"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td class=""lblHoraCabecario"">" + Date.Now + "</td>"
        codigoHtml &= "<td>Resumo do Pedido</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</table>"
        codigoHtml &= "</div>"
#End Region

#Region " Tabela dados Plataforma "
        codigoHtml &= "<Table Class=""headerTb"" id = ""tbHeaderPlataforma"">"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td>" & titulo1 & "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a tbHeaderPlataforma-->"

        codigoHtml &= "<Table Class=""bodyTb"" id=""tbBodyPlataforma"">"
        For Each item As Object In LISTA_DADOS(0)
            codigoHtml &= "<tr>"
            codigoHtml &= "<td Class=""labelNegrito"">Placa:</td>"
            codigoHtml &= "<td colspan=""2"">" & validarCampoResumoPedido(item.DS_PLACA) & "</td>"
            codigoHtml &= "<td Class=""labelNegrito"">Chassi:</td>"
            codigoHtml &= "<td colspan=""3"">" & validarCampoResumoPedido(item.DS_CHASSI) & "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "<tr>"
            codigoHtml &= "<td Class=""labelNegrito"" > marca:</td>"
            codigoHtml &= "<td colspan=""2"">" & validarCampoResumoPedido(item.NM_MARCA) & "</td>"
            codigoHtml &= "<td Class=""labelNegrito"">Modelo:</td>"
            codigoHtml &= "<td colspan=""3"">" & validarCampoResumoPedido(item.NM_MODELO) & "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "<tr>"
            codigoHtml &= "<td Class=""labelNegrito"">Cor:</td>"
            codigoHtml &= "<td colspan=""2"">" & validarCampoResumoPedido(item.DS_COR) & " </td>"
            codigoHtml &= "<td Class=""labelNegrito"">Ano/Fabricação:</td>"
            codigoHtml &= "<td>" & validarCampoResumoPedido(item.NU_ANO_FABRICACAO) & "</td>"
            codigoHtml &= "<td Class=""labelNegrito"">Ano/ Modelo:</td>"
            codigoHtml &= "<td>" & validarCampoResumoPedido(item.NU_ANO_MODELO) & "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "<tr>"
            codigoHtml &= "<td Class=""labelNegrito"">Renavam:</td>"
            codigoHtml &= "<td colspan=""2"">" & validarCampoResumoPedido(item.NR_RENAVAM) & "</td>"
            codigoHtml &= "<td Class=""labelNegrito"">Combustivel:</td>"
            codigoHtml &= "<td colspan=""2"">" & validarCampoResumoPedido(item.DS_COMBUSTIVEL) & "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "</table>"
            codigoHtml &= "<!--fecha a tbBodyPlataforma-->"
        Next
#End Region

#Region "Tabela dados propietário"
        codigoHtml &= "<table class=""headerTb"" id=""tbHeaderDadosProp"">"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td>" & titulo2 & "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a tbHeaderDadosDoPropietario-->"

        codigoHtml &= "<table class=""bodyTb"" id=""tbBodyDadosProp"">"
        For Each item As Object In LISTA_DADOS(1)
            codigoHtml &= "<tr>"
            codigoHtml &= "<td align=""left"">"
            codigoHtml &= "<span class=""labelNegrito"" colspan=""2"">Nome/Razão Social:</span>"
            codigoHtml &= "<span id=""lblNomeProprietario"">" & validarCampoResumoPedido(item.NM_PESSOA) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "<td align=""left"">"
            codigoHtml &= "<span class=""labelNegrito"">Tipo do Cliente:</span>"
            codigoHtml &= "<span id=""lblTipoProprietario"">" & validarCampoResumoPedido(item.NM_TIPO_PESSOA) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "<tr>"
            codigoHtml &= "<td align=""left"">"
            codigoHtml &= "<span class=""labelNegrito"">CPF/CNPJ:</span>"
            codigoHtml &= "<span id=""lblCpfProprietario"">" & validarCampoResumoPedido(item.NR_CNPJ_CPF) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "<tr>"
            codigoHtml &= "<td align=""left"">"
            codigoHtml &= "<span class=""labelNegrito"">Sexo:</span>"
            codigoHtml &= "<span id=""lblSexo"">" & validarCampoResumoPedido(item.SEXO) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "<td align=""left"">"
            codigoHtml &= "<span class=""labelNegrito"">Data de Nascimento:</span>"
            codigoHtml &= "<span id=""lblDtNascimento"">" & validarCampoResumoPedido(item.DT_NASCIMENTO) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "</tr>"
        Next

        codigoHtml &= "<tr>"
        codigoHtml &= "<td colspan=""3"">"
        codigoHtml &= "<table border=""0"">"
        For Each item As Object In LISTA_DADOS(2)
            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Endereço:</td>"
            codigoHtml &= "<td colspan=""5"" class=""label"">" & validarCampoResumoPedido(item.NM_ENDERECO) & "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Bairro:</td>"
            codigoHtml &= "<td class=""label"" align=""left"">" & validarCampoResumoPedido(item.NM_BAIRRO) & "</td>"
            codigoHtml &= "<td class=""labelNegrito"">Cidade:</td>"
            codigoHtml &= "<td class=""label"" colspan=""3"">" & validarCampoResumoPedido(item.NM_CIDADE) & "</td>"
            codigoHtml &= "</tr>"
            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Estado:</td>"
            codigoHtml &= "<td class=""label"">" & validarCampoResumoPedido(item.CD_UF) & "</td>"
            codigoHtml &= "<td class=""labelNegrito"">CEP:</td>"
            codigoHtml &= "<td class=""label"">" & validarCampoResumoPedido(item.NR_CEP) & "</td>"
            codigoHtml &= "<td class=""labelNegrito"">Tipo:</td>"
            codigoHtml &= "<td class=""label"">" & validarCampoResumoPedido(item.NM_TIPO_ENDERECO) & "</td>"
            codigoHtml &= "</tr>"
        Next
        codigoHtml &= "</table>"
        codigoHtml &= "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "<!--fecha a table 2 dados endereço propietario-->"

        codigoHtml &= "</table>"
#End Region

#Region "Tabela dados Cliente"

        codigoHtml &= "<table class=""headerTb"" id=""tbHeaderCliente"">"
        codigoHtml &= "<tbody>"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td>" + titulo3 + ""
        codigoHtml &= "<span id=""lblTitDadosCliente"">" & IIf(validacao = True, " - igual ao propietário", "") & "</span>"
        codigoHtml &= "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</tbody>"
        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a tbHeaderCliente-->"

        If validacao = False Then
            codigoHtml &= "<table class=""bodyTb"" width=""100%"">"
            codigoHtml &= "<tbody>"

            For Each item As Object In LISTA_DADOS(3)
                codigoHtml &= "<tr>"
                codigoHtml &= "<td align=""left"" class=""label"">"
                codigoHtml &= "<span class=""labelNegrito"" colspan=""2"">Nome:</span>"
                codigoHtml &= "<span id=""lblNomeProprietario"">" & CStr(item.NM_PESSOA) & "</span>"
                codigoHtml &= "</td>"
                codigoHtml &= "</tr>"

                codigoHtml &= "<tr>"
                codigoHtml &= "<td align=""left"" class=""label"">"
                codigoHtml &= "<span class=""labelNegrito"" colspan=""2"">CPF/CNPJ:</span>"
                codigoHtml &= "<span id=""lblNomeProprietario"">" & CStr(item.NR_CNPJ_CPF) & "</span>"
                codigoHtml &= "</td>"
                codigoHtml &= "<td align=""left"" class=""label"">"
                codigoHtml &= "<span class=""labelNegrito"" colspan=""2"">RG/Inscr. est:</span>"
                codigoHtml &= "<span id=""lblNomeProprietario"">" & CStr(item.NR_IE_RG) & "</span>"
                codigoHtml &= "</td>"
                codigoHtml &= "</tr>"

                codigoHtml &= "</tbody>"
                codigoHtml &= "</table>"
                codigoHtml &= "<!--Fecha a tbBodyDadosCliente-->"
            Next
        End If
#End Region

#Region "Tabela dados Contato"
        codigoHtml &= "<table class=""headerTb"" id=""tbHeaderContato"" width=""100%"">"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td>" & titulo4 & "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a tbHeaderContato-->"

        codigoHtml &= "<table class=""bodyTb"" width=""100%"">"
        codigoHtml &= "<tr>"
        codigoHtml &= "<th class=""labelNegrito"">Nome</th>"
        codigoHtml &= "<th class=""labelNegrito"">Relacionamento</th>"
        codigoHtml &= "<th class=""labelNegrito"">Comercial</th>"
        codigoHtml &= "<th class=""labelNegrito"">Residencial</th>"
        codigoHtml &= "<th class=""labelNegrito"">Celular</th>"
        codigoHtml &= "</tr>"

        For Each item As Object In LISTA_DADOS(4)
            codigoHtml &= "<tr>"
            codigoHtml &= "<td align=""left"" class=""label"">" & validarCampoResumoPedido(item.NM_CONTATO) & "</td>"
            codigoHtml &= "<td align=""left"" class=""label"">" & validarCampoResumoPedido(item.NM_GRAU_RELACIONAMENTO) & "</td>"
            codigoHtml &= "<td align=""left"" class=""label"">(" & validarCampoResumoPedido(item.NR_DDD_COMERCIAL) & ")" & validarCampoResumoPedido(item.NR_TELEFONE_COMERCIAL) & "</td>"
            codigoHtml &= "<td align=""left"" class=""label"">(" & validarCampoResumoPedido(item.NR_DDD_RESIDENCIAL) & ")" & validarCampoResumoPedido(item.NR_TELEFONE_RESIDENCIAL) & "</td>"
            codigoHtml &= "<td align=""left"" class=""label"">(" & validarCampoResumoPedido(item.NR_DDD_CELULAR) & ")" & validarCampoResumoPedido(item.NR_TELEFONE_CELULAR) & "</td>"
            codigoHtml &= "</tr>"
        Next

        codigoHtml &= "</table>"
        codigoHtml &= "<!--Fecha a tbBodyDadosContato-->"
#End Region

#Region "Tabela dados detalhes agendamento"
        codigoHtml &= "<!--Abre a tbHeaderDetalhesAgendamento-->"
        codigoHtml &= "<table class=""headerTb"" id=""tbHeaderdetalhesAgen"" width=""100%"">"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td>" & titulo5 & "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a a tbHeaderDetalhesAgendamento-->"

        codigoHtml &= "<!--Abre a tbBodyDetalhesAgendamento-->"
        codigoHtml &= "<table class=""bodyTb"" id=""tbBodyDetalhesAgendamento"" width=""100%"">"

        For Each item As Object In LISTA_DADOS(5)
            codigoHtml &= "<tr>"
            codigoHtml &= "<td align=""left"" class=""label"">"
            codigoHtml &= "<span class=""labelNegrito"" colspan=""2"">Data Agendamento:</span>"
            codigoHtml &= "<span id=""lblNomeProprietario"">" & CStr(item.DT_AGENDAMENTO) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "<td align=""left"" class=""label"">"
            codigoHtml &= "<span class=""labelNegrito"">Hora:</span>"
            codigoHtml &= "<span id=""lblTipoProprietario"">" & CStr(item.HORA) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "<td align=""left"" class=""label"">"
            codigoHtml &= "<span class=""labelNegrito"">Local:</span>"
            codigoHtml &= "<span id=""lblCpfProprietario"">" & CStr(item.NM_LOCAL_INSTALACAO) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "</tr>"

            codigoHtml &= "<tr>"
            codigoHtml &= "<td align=""left"" class=""label"">"
            codigoHtml &= "<span class=""labelNegrito"">Loja:</span>"
            codigoHtml &= "<span id=""lblSexo"">" & CStr(item.NM_PESSOA_LOJA) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "</tr>"

            codigoHtml &= "<tr>"
            codigoHtml &= "<td align=""left"" class=""label"">"
            codigoHtml &= "<span class=""labelNegrito"">Operador:</span>"
            codigoHtml &= "<span id=""lblSexo"">" & CStr(item.NM_PESSOA_OPERADOR) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "<td align=""left"" class=""label"">"
            codigoHtml &= "<span class=""labelNegrito"">Data Do Cadastro:</span>"
            codigoHtml &= "<span id=""lblSexo"">" & CStr(item.DT_ALTERACAO) & "</span>"
            codigoHtml &= "</td>"
            codigoHtml &= "</tr>"
        Next

        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a tbBodyDetalhesAgendamento-->"
#End Region

#Region "Tabela dados Local da Instalação"
        codigoHtml &= "<table class=""headerTb"" id=""tbHeaderLocalInst"" width=""100%"">"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td>" & titulo6 & "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</table>"
        codigoHtml &= "<!--Fecha a tbHeaderLocalInst-->"

        codigoHtml &= "<table border=""0"" class=""bodyTb"" cellpadding=""0"" cellspacing=""0"">"

        For Each item As Object In LISTA_DADOS(6)
            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Endereço:</td>"
            codigoHtml &= "<td colspan=""5"" class=""label"">" & CStr(item.NM_ENDERECO) & "</td>"
            codigoHtml &= "</tr>"

            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Bairro:</td>"
            codigoHtml &= "<td class=""label"" align=""left"">" & CStr(item.NM_BAIRRO) & "</td>"
            codigoHtml &= "<td class=""labelNegrito"">Cidade:</td>"
            codigoHtml &= "<td class=""label"" colspan=""3"">" & CStr(item.NM_CIDADE) & "</td>"
            codigoHtml &= "</tr>"

            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Estado:</td>"
            codigoHtml &= "<td class=""label"">" & CStr(item.CD_UF) & "</td>"
            codigoHtml &= "<td class=""labelNegrito"">CEP:</td>"
            codigoHtml &= "<td class=""label"">" & CStr(item.NR_CEP) & "</td>"
            codigoHtml &= "<td class=""labelNegrito"">Observações:</td>"
            codigoHtml &= "<td class=""label"">" & CStr(item.DS_OBSERVACAO) & "</td>"
            codigoHtml &= "</tr>"

            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito""> Contato:</td>"
            codigoHtml &= "<td class=""label"">" & CStr(item.NM_CONTATO) & "</td>"
            codigoHtml &= "<td class=""labelNegrito"">Fone Contato:</td>"
            codigoHtml &= "<td class=""label"">" & CStr(item.NR_TELEFONE_CONTATO) & "</td>"
            codigoHtml &= "</tr>"
        Next

        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a tbBodyLocalInst-->"
#End Region

#Region "Tabela dados Serviços"
        codigoHtml &= "<table class=""headerTb"" id=""tbHeaderServicos"" width=""100%"">"
        codigoHtml &= "<tr>"
        codigoHtml &= "<td>" & titulo7 & "</td>"
        codigoHtml &= "</tr>"
        codigoHtml &= "</table>"
        codigoHtml &= "<!--Fecha a tbBodyLocalInst-->"


        codigoHtml &= "<!--Abre a tbBodyServicos-->"
        codigoHtml &= "<table border=""0"" class=""bodyTb"" cellpadding=""0"" cellspacing=""0"">"

        For Each item As Object In LISTA_DADOS(7)
            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Serviço:</td>"
            codigoHtml &= "<td colspan=""5"" class=""label"">" & CStr(item.NM_PRODUTO) & "</td>"
            codigoHtml &= "</tr>"

            codigoHtml &= "<tr>"
            codigoHtml &= "<td class=""labelNegrito"">Serviços Adcionais:</td>"
            codigoHtml &= "<td class=""label"" align=""left"">" & CStr(item.ADCIONAL_PRODUTO) & "</td>"
            codigoHtml &= "</tr>"
        Next

        codigoHtml &= "</table>"
        codigoHtml &= "<!--fecha a tbBodyServicos-->"
#End Region

        codigoHtml &= "</div>"
        codigoHtml &= " <!-- Fecha container principal que abrange toda a pagina -->"
        codigoHtml &= "</body>"
        codigoHtml &= "</html>"

        Return codigoHtml
    End Function

    Function GerarResumoPedido(LISTA_DADOS As List(Of Object))
        Dim html As String
        Dim css As String
        Dim documento As GeraResumoPedido = New GeraResumoPedido()

        html = GerarHtml(LISTA_DADOS)
        css = GerarCss()

        documento.nmArquivo = "Resumo do Pedido"
        documento.binData = HtmlParaPdf(html, css)

        Return documento.binData
    End Function

    Function HtmlParaPdf(HTML As String, CSS As String)
        Dim bytes As Byte()
        Using ms As MemoryStream = New MemoryStream()
            Using doc As New Document()
                Using writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
                    doc.Open()
                    Using msCss As MemoryStream = New MemoryStream(System.Text.Encoding.UTF8.GetBytes(CSS))
                        Using mshtml As MemoryStream = New MemoryStream(System.Text.Encoding.UTF8.GetBytes(HTML))
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, mshtml, msCss)
                        End Using
                    End Using
                    doc.Close()
                End Using
            End Using
            bytes = ms.ToArray()
            Return bytes
        End Using
    End Function

    Function validarCampoResumoPedido(item As Object)
        If item Is Nothing Then
            item = ""
        End If

        Select Case item.ToString()

            Case ""
                Return "*"
            Case "0"
                Return "*"
            Case Nothing
                Return "*"
            Case Else
                Return item
        End Select
    End Function

End Class
