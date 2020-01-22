Imports System.Data.SqlTypes
Imports System.IO
Imports System.Threading.Tasks
Imports System.Web.Mvc
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports PortalIturan.Business
Imports PortalIturan.Client

Namespace PortalIturan.Web.Areas.Operacao
    Public Class TermosController
        Inherits Controller

        Function CarregaDadosResumoPedido(CD_SOLICITACAO_AGENDAMENTO As Integer)
            Dim resumoDs As DataSet
            Dim listDados As List(Of Object) = New List(Of Object)

            resumoDs = ClientContext.GetInstance().GetAGENDAMENTOCOB().ObterResumoPedido(New SqlInt32(CD_SOLICITACAO_AGENDAMENTO))
            Dim plataforma =
                        From r In resumoDs.Tables(1).AsEnumerable
                        Select New With {
                            .DS_CHASSI = r.Field(Of String)("DS_CHASSI"),
                            .DS_COMBUSTIVEL = r.Field(Of String)("DS_COMBUSTIVEL"),
                            .DS_COR = r.Field(Of String)("DS_COR"),
                            .DS_PLACA = r.Field(Of String)("DS_PLACA"),
                            .NM_MARCA = r.Field(Of String)("NM_MARCA"),
                            .NM_MODELO = r.Field(Of String)("NM_MODELO"),
                            .NR_PLATAFORMA = If(r.Field(Of Integer?)("NR_PLATAFORMA") Is Nothing, 0, r.Field(Of Integer?)("NR_PLATAFORMA")),
                            .NR_RENAVAM = If(r.Field(Of Long?)("NR_RENAVAM") Is Nothing, 0, r.Field(Of Long?)("NR_RENAVAM")),
                            .NU_ANO_FABRICACAO = If(r.Field(Of Int16?)("NU_ANO_FABRICACAO") Is Nothing, 0, r.Field(Of Int16?)("NU_ANO_FABRICACAO")),
                            .NU_ANO_MODELO = If(r.Field(Of Int16?)("NU_ANO_MODELO") Is Nothing, 0, r.Field(Of Int16?)("NU_ANO_MODELO"))
                            }

            Dim dadosPropietario =
                        (From r In resumoDs.Tables(2).AsEnumerable
                         Select New With {
                            .NM_PESSOA = r.Field(Of String)("NM_PESSOA"),
                            .NR_CNPJ_CPF = r.Field(Of String)("NR_CNPJ_CPF"),
                            .NR_IE_RG = r.Field(Of String)("NR_IE_RG"),
                            .NM_TIPO_PESSOA = r.Field(Of String)("NM_TIPO_PESSOA"),
                            .SEXO = r.Field(Of String)("SEXO"),
                            .DT_NASCIMENTO = r.Field(Of DateTime?)("DT_NASCIMENTO")
                            }).Distinct()

            Dim dadosCliente =
                    (From r In resumoDs.Tables(6).AsEnumerable
                     Select New With {
                            .NM_PESSOA = r.Field(Of String)("NM_PESSOA"),
                            .NR_CNPJ_CPF = r.Field(Of String)("NR_CNPJ_CPF"),
                            .NR_IE_RG = r.Field(Of String)("NR_IE_RG")
                         })


            Dim enderecoPropietario =
                         From r In resumoDs.Tables(3).AsEnumerable
                         Select New With {
                            .NR_CEP = r.Field(Of String)("NR_CEP"),
                            .CD_TIPO_ENDERECO = If(r.Field(Of Integer?)("CD_TIPO_ENDERECO") Is Nothing, 0, r.Field(Of Integer?)("CD_TIPO_ENDERECO")),
                            .NM_TIPO_ENDERECO = r.Field(Of String)("NM_TIPO_ENDERECO"),
                            .NM_ENDERECO = r.Field(Of String)("NM_ENDERECO"),
                            .NR_NUMERO = If(r.Field(Of Integer?)("NR_NUMERO") Is Nothing, 0, r.Field(Of Integer?)("NR_NUMERO")),
                            .DS_COMPLEMENTO = r.Field(Of String)("DS_COMPLEMENTO"),
                            .NM_BAIRRO = r.Field(Of String)("NM_BAIRRO"),
                            .CD_UF = r.Field(Of String)("CD_UF"),
                            .NM_CIDADE = r.Field(Of String)("NM_CIDADE")
                             }
            Dim contatos =
                         From r In resumoDs.Tables(4).AsEnumerable
                         Select New With {
                            .NM_CONTATO = r.Field(Of String)("NM_CONTATO"),
                            .NM_GRAU_RELACIONAMENTO = r.Field(Of String)("NM_GRAU_RELACIONAMENTO"),
                            .NR_DDD_COMERCIAL = If(r.Field(Of Int16?)("NR_DDD_COMERCIAL") Is Nothing, 0, r.Field(Of Int16?)("NR_DDD_COMERCIAL")),
                            .NR_DDD_RESIDENCIAL = If(r.Field(Of Int16?)("NR_DDD_RESIDENCIAL") Is Nothing, 0, r.Field(Of Int16?)("NR_DDD_RESIDENCIAL")),
                            .NR_DDD_CELULAR = If(r.Field(Of Int16?)("NR_DDD_CELULAR") Is Nothing, 0, r.Field(Of Int16?)("NR_DDD_CELULAR")),
                            .NR_TELEFONE_COMERCIAL = If(r.Field(Of Integer?)("NR_TELEFONE_COMERCIAL") Is Nothing, 0, r.Field(Of Integer?)("NR_TELEFONE_COMERCIAL")),
                            .NR_TELEFONE_RESIDENCIAL = If(r.Field(Of Integer?)("NR_TELEFONE_RESIDENCIAL") Is Nothing, 0, r.Field(Of Integer?)("NR_TELEFONE_RESIDENCIAL")),
                            .NR_TELEFONE_CELULAR = If(r.Field(Of Integer?)("NR_TELEFONE_CELULAR") Is Nothing, 0, r.Field(Of Integer?)("NR_TELEFONE_CELULAR")),
                            .NR_RAMAL_COMERCIAL = If(r.Field(Of Int16?)("NR_RAMAL_COMERCIAL") Is Nothing, 0, r.Field(Of Int16?)("NR_RAMAL_COMERCIAL"))
                            }

            Dim agendamentos =
                        From r In resumoDs.Tables(5).AsEnumerable()
                        Select New With {
                           .DT_CANCELAMENTO = r.Field(Of DateTime?)("DT_CANCELAMENTO"),
                           .DT_AGENDAMENTO = r.Field(Of DateTime?)("DT_AGENDAMENTO"),
                           .NM_LOCAL_INSTALACAO = r.Field(Of String)("NM_LOCAL_INSTALACAO"),
                           .NM_PESSOA_LOJA = r.Field(Of String)("NM_PESSOA_LOJA"),
                           .HORA = r.Field(Of String)("HORA"),
                           .NM_PESSOA_OPERADOR = r.Field(Of String)("NM_PESSOA_OPERADOR"),
                           .DT_ALTERACAO = r.Field(Of DateTime?)("DT_ALTERACAO")
                            }
            Dim localInstalacao =
                    From r In resumoDs.Tables(9).AsEnumerable()
                    Select New With {
                       .NM_ENDERECO = r.Field(Of String)("NM_ENDERECO"),
                       .NR_NUMERO = r.Field(Of String)("NR_NUMERO"),
                       .DS_COMPLEMENTO = r.Field(Of String)("DS_COMPLEMENTO"),
                       .NM_BAIRRO = r.Field(Of String)("NM_BAIRRO"),
                       .CD_UF = r.Field(Of String)("CD_UF"),
                       .NM_CIDADE = r.Field(Of String)("NM_CIDADE"),
                       .NR_CEP = r.Field(Of String)("NR_CEP"),
                       .DS_OBSERVACAO = r.Field(Of String)("DS_OBSERVACAO"),
                       .NM_CONTATO = r.Field(Of String)("NM_CONTATO"),
                       .NR_TELEFONE_CONTATO = r.Field(Of String)("NR_TELEFONE_CONTATO")
                        }

            Dim servicos =
                       From r In resumoDs.Tables(10).AsEnumerable()
                       Select New With {
                           .NM_PRODUTO = r.Field(Of String)("NM_PRODUTO"),
                           .ADCIONAL_PRODUTO = r.Field(Of String)("X")
                        }

            Dim validaPessBeneficiario =
                    (From r In resumoDs.Tables(8).AsEnumerable()
                     Select New With {
                       .CD_CLIENTE = r.Field(Of Int32)("CLIENTE"),
                       .CD_BENEFICIARIO = r.Field(Of Int32)("BENEFICIARIO")
                }).First()

            listDados.Add(plataforma)
            listDados.Add(dadosPropietario)
            listDados.Add(enderecoPropietario)
            listDados.Add(dadosCliente)
            listDados.Add(contatos)
            listDados.Add(agendamentos)
            listDados.Add(localInstalacao)
            listDados.Add(servicos)
            listDados.Add(validaPessBeneficiario)
            Return listDados
        End Function

        Function CarregarResumoPedidoLote(DADOS As List(Of Object))
            Dim dataList As New List(Of Byte())
            Dim documento As New GeraResumoPedido()
            Dim j As JsonResult = New JsonResult()
            Dim dadosResumo As List(Of Object)
            Dim SaidaPdf As Object = New With {.cdRetorno = 0, .nmArquivo = "", .binData = Nothing, .dsRetorno = ""}

            Parallel.ForEach(DADOS,
                                 Sub(x As Object)
                                     dadosResumo = CarregaDadosResumoPedido(x)
                                     dataList.Add(documento.GerarResumoPedido(dadosResumo))
                                 End Sub)
            SaidaPdf.binData = ConcatenaPdfs(dataList)
            SaidaPdf.nmArquivo = "Resumo do pedido em Lote.pdf"
            j.Data = SaidaPdf
            j.MaxJsonLength = Int32.MaxValue
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet
            Return j
        End Function

        Function CarregarTermosLote(DADOS As String)
            Dim CD_PEDIDO = Integer.Parse(DADOS)
            Dim listaDados As New List(Of Object)
            Dim documento As New GeraResumoPedido()
            Dim r As Object = New Object()
            Dim SaidaPdf As Object = New With {.cdRetorno = 0, .nmArquivo = "", .binData = Nothing, .dsRetorno = ""}
            Dim j As JsonResult = New JsonResult()
            listaDados.Clear()

            r = PEDIDOBOB.GetInstance.VisualizarPDFContratoPorPedido(CD_PEDIDO, Nothing, Nothing, Nothing, Nothing)
            Select Case r.cdRetorno
                Case 0
                    SaidaPdf = r
                Case 1
                    SaidaPdf = documento.gerarErroimpresao("Aviso: " + r.dsRetorno + "")
                Case -1
                    SaidaPdf = documento.gerarErroimpresao("Erro ao gerar Termo do CD_PEDIDO" + +"")
            End Select

            j.Data = SaidaPdf
            j.MaxJsonLength = Int32.MaxValue
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet
            Return j
        End Function

        Function ConcatenaPdfs(PDFS As List(Of Byte()))
            Try
                Dim mergedPdf As Byte()
                Using ms As MemoryStream = New MemoryStream()
                    Using document As Document = New Document()
                        Using copy As PdfSmartCopy = New PdfSmartCopy(document, ms)
                            document.Open()
                            For i As Integer = 0 To PDFS.Count - 1
                                Using Reader = New PdfReader(PDFS(i))
                                    copy.AddDocument(Reader)
                                End Using
                            Next
                            document.Close()
                        End Using
                    End Using
                    Return ms.ToArray()
                End Using
            Catch e As NullReferenceException
                Return e.Message & " A função ConcatenaPdfs espera um List Byte()como parâmetro."
            Catch e As Exception
                Return e.Message
            End Try
        End Function

        Function ListarLoja() As JsonResult
            Dim listaLoja = PESSOABOB.GetInstance().ListarLojas(SharedFunctions.ObterUsuario())
            Return Json(listaLoja, JsonRequestBehavior.AllowGet)
        End Function

        Function ListarPorLoja(CD_PESSOA As Int32, DT_INICIO As DateTime, DT_FIM As DateTime, CD_LOCAL_INSTALACAO As Integer, FL_DT_FIM As Integer) As JsonResult
            Session("AgendaLoja_Data") = DT_INICIO.ToString("dd/MM/yyyy")
            Session("AgendaLoja_Data_Fin") = DT_FIM.ToString("dd/MM/yyyy")
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1))
            Response.Cache.SetValidUntilExpires(False)
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches)
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.Cache.SetNoStore()
            Dim r = AGENDAMENTOBOB.GetInstance().ListarTermosPorLoja(CD_PESSOA, DT_INICIO, DT_FIM, CD_LOCAL_INSTALACAO, Nothing, FL_DT_FIM)
            Return Json(r, JsonRequestBehavior.AllowGet)
        End Function

        Function TermosEmLote() As ActionResult
            Dim usuario As Integer = SharedFunctions.ObterUsuario()
            ViewBag.usuario = usuario
            ViewBag.dtAgendamento = Session("AgendaLoja_Data")
            ViewBag.dtAgendamentoFin = Session("AgendaLoja_Data_Fin")
            Return View()
        End Function

        Function VisualizarPdfContrato(CD_PEDIDO As Integer)
            Dim saidaPdf As Byte()
            Dim nomeArquivo As String
            Dim resposta = System.Web.HttpContext.Current.Response
            Dim r As Object = PEDIDOBOB.GetInstance.VisualizarPDFContratoPorPedido(CD_PEDIDO, Nothing, Nothing, Nothing, Nothing)

            If r.binData Is Nothing Then
                Dim documento As New GeraResumoPedido()
                Dim pdfErro As Object
                nomeArquivo = "Erro ao gerar Termo.pdf"
                pdfErro = documento.gerarErroimpresao("Erro ao gerar Termo")

                nomeArquivo = pdfErro.nmArquivo
                saidaPdf = pdfErro.binData
            Else
                nomeArquivo = r.nmArquivo
                saidaPdf = r.binData
            End If

            resposta.AddHeader("Content-Disposition", "attachment; filename=" & nomeArquivo)
            resposta.ContentType = "application/pdf"
            resposta.BinaryWrite(saidaPdf)
            resposta.[End]()
        End Function

        Function VisualizarPdfResumo(CD_SOLICITACAO As Integer)
            Dim saidaPdf As Byte()
            Dim nomeArquivo As String
            Dim listaDados As List(Of Object)
            Dim documento As New GeraResumoPedido()
            Dim resposta = System.Web.HttpContext.Current.Response

            listaDados = CarregaDadosResumoPedido(CD_SOLICITACAO)

            If listaDados Is Nothing Then
                nomeArquivo = "Erro Resumo do Pedido.pdf"
                saidaPdf = documento.gerarErroimpresao("Erro ao carregar dados do resumo do pedido")
            Else
                nomeArquivo = "Resumo do Pedido Solicitação - " & CD_SOLICITACAO & ".pdf"
                saidaPdf = documento.GerarResumoPedido(listaDados)
            End If
            documento = Nothing
            listaDados = Nothing
            resposta.AddHeader("Content-Disposition", "attachment; filename=" & nomeArquivo)
            resposta.ContentType = "application/pdf"
            resposta.BinaryWrite(saidaPdf)
            resposta.[End]()
        End Function

    End Class
End Namespace