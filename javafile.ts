var Controller =
{
    dataSource:
    {
        ddlLoja: new kendo.data.DataSource({
            transport: {
                read: { url: urlListarLoja, async: false }
            },
            schema: {
                model: {
                    id: "CD_PESSOA",
                    fields: {
                        CD_PESSOA: { type: "number" },
                        NM_FANTASIA: { type: "string" }
                    }
                }
            },
        }),

        pesquisaGrid: new kendo.data.DataSource
            ({
                transport: {
                    read: { url: urlListarPorLoja, async: false }
                },
                schema: {
                    model: {
                        id: "CD_PEDIDO",
                        fields:
                        {
                            NR_STATUS: { type: "string" },
                            camposLink: { type: "string" },
                            FL_DOMICILIAR: { type: "boolean" },
                            tipo_agendamento: { type: "string" },
                            CD_SOLICITACAO: { type: "string" },
                            NR_CEP: { type: "string" },
                            servicos: { type: "string" },
                            DT_INICIO: { type: "date" },
                            DT_FIM: { type: "date" },
                            NU_PERIODO: { type: "number" },
                            DT_HORARIO_INICIO: { type: "date" },
                            DT_HORARIO_FIM: { type: "date" },
                            CD_LOCAL_INSTALACAO: { type: "string" },
                            NM_PESSOA_BENEFICIARIO: { type: "string" },
                            NM_PESSOA: { type: "string" },
                            NM_ENDERECO: { type: "string" },
                            DS_PONTO_REFERENCIA: { type: "string" },
                            tipo_agendamento: { type: "string" },
                            NM_BAIRRO: { type: "string" },
                            NR_CNPJ_CPF: { type: "string" },
                            DS_OBSERVACAO: { type: "string" },
                            NR_CEP: { type: "string" },
                            CD_PESSOA_SEGURADORA: { type: "number" },
                            CD_PEDIDO: { type: "number" }
                        }
                    }
                }
            }),
    },

    init: function () {
        $("#Btns").hide();
        // Constante definida para representar o tipo domiciliar = "2"
        const localInstalacao = "2"

        $('#ddlLoja').kendoComboBox(
            {
                placeholder: "Selecione...",
                dataTextField: "NM_FANTASIA",
                dataValueField: "CD_PESSOA",
                dataSource: Controller.dataSource.ddlLoja,
                enable: false,
                index: 0
            });

        if (dtAgendamento != "" && dtAgendamentoFin != "") {
            var dt = dtAgendamento;
            var dtFin = dtAgendamentoFin;
            $("#txtDtInicio").val(dt);
            $("#txtDtInicio").kendoDatePicker({ format: "dd/MM/yyyy" });
            $("#txtDtFim").val(dtFin);
            $("#txtDtFim").kendoDatePicker({ format: "dd/MM/yyyy" });
        } else {
            $("#txtDtInicio").kendoDatePicker({ format: "dd/MM/yyyy", value: new Date() });
            $("#txtDtFim").kendoDatePicker({ format: "dd/MM/yyyy", value: new Date() });
        }

        function animarCarregamento() {
            $('#container').delay(350).css({ 'display': 'block' });
        }

        function animacaocarregamentoStop() {
            $('#container').delay(350).css({ 'display': 'none' });
        }

        function AlertMsg(message) { staticNotification.show('' + message + '', "error"); }
        var staticNotification = $("#staticNotification").kendoNotification({
            autoHideAfter: 5000,
            appendTo: "#divNotificacao"
        }).data("kendoNotification");


        $("#btnPesquisar").click(function () {
            dataItem = [];
            $("#grdContainer").empty();
            var dataInicial = $("#txtDtInicio").data("kendoDatePicker").value();
            var dataFinal = $("#txtDtFim").data("kendoDatePicker").value();
            var dtInicial = kendo.toString(dataInicial, "yyyy-MM-dd");
            var dtFinal = kendo.toString(dataFinal, "yyyy-MM-dd");

            Controller.dataSource.pesquisaGrid.read({ CD_PESSOA: $("#ddlLoja").val(), DT_INICIO: dtInicial, DT_FIM: dtFinal, CD_LOCAL_INSTALACAO: localInstalacao, FL_DT_FIM: 1 });

            if (Controller.dataSource.pesquisaGrid.total() <= 0) {
                staticNotification.show("Não houveram agendamentos no período selecionado", "error");
                var container = $(staticNotification.options.appendTo);
                container.scrollTop(container[0].scrollHeight);
                $("#grdContainer").hide();
                $("#Btns").hide();
                return;

            } else {
                $("#grdContainer").show();
                if (Controller.dataSource.pesquisaGrid.total() > 0)
                    $("#Btns").show();

                $("#grdContainer").kendoGrid({
                    dataSource: Controller.dataSource.pesquisaGrid,
                    persistSelection: true,
                    sortable: true,
                    scrollable: true,
                    pageable: {
                        numeric: false,
                        previousNext: false,
                        messages: {
                            refresh: "Refresh data",
                            display: "Total de: {2} Agendamentos"
                        }
                    },
                    columns: [
                        { width: "30px", field: "", title: "", template: kendo.template($('#commandFielsTemplate').html()), attributes: { style: "text-align:center" }, headerTemplate: "<span id='commandSpan' style='position:absolute'></span>" },
                        { width: "30px", headerTemplate: '<input type="checkbox" id="check-all" class="check-box-Header" title="Selecionar Todos" /><label for="check-all"></label>', id: "IMP_OPT", title: "", "template": '<input  type="checkbox" />', attributes: { "class": "checkbox" } },
                        { width: "60px", field: "NR_CEP", title: "CEP" },
                        { width: "150px", field: "", title: "", template: kendo.template($('#clientFieldTemplate').html()), headerTemplate: kendo.template($('#clientHeaderTemplate').html()) },
                        { width: "120px", field: "NR_CNPJ_CPF", title: "CPF/CNPJ" },
                        { width: "100px", field: "NM_BAIRRO", title: Integra.getStorage("Titulo", "Bairro") },
                        { width: "200px", field: "NM_ENDERECO", title: Integra.getStorage("Titulo", "Endereco") },
                        { width: "100px", field: "DS_PONTO_REFERENCIA", title: "Referência", attributes: { "class": "block-with-text", style: "max-width:150px" } },
                        { width: "70px", field: "tipo_agendamento", title: Integra.getStorage("Titulo", "Tipo") },
                        { width: "150px", field: "DS_OBSERVACAO", title: Integra.getStorage("Titulo", "Observacao"), attributes: { "class": "block-with-text", style: "max-width:150px" } },
                    ], sort: { field: "NR_CEP", dir: "desc" }
                });
            };
        });

        var dataItem = [];

        $("#grdContainer").kendoTooltip({
            filter: "td:nth-child(10), td:nth-child(8)",
            show: function (e) {
                if (this.content.text().length > 15) {
                    this.content.parent().css("visibility", "visible");
                }
            },
            hide: function (e) {
                this.content.parent().css("visibility", "hidden");
            },
            position: "left",
            animation:
            {
                close: {
                    effects: "fade:out"
                },
                open: {
                    effects: "fade:in",
                    duration: 800
                }
            },
            content: function (e) {
                return e.target.closest("tr").context.innerText;
            }
        }).data("kendoTooltip");

        $('#grdContainer').on('click', ':checkbox', function () {

            var grid = $("#grdContainer").data("kendoGrid");
            var tr = $(this).closest("tr");

            var checado = false;

            if (this.offsetParent.className == "checkbox") {
                var checado = $(this).prop("checked");

                if (checado != true) {
                    dataItem.pop(grid.dataItem(tr));
                } else {
                    dataItem.push(grid.dataItem(tr));
                }
            }

            if (this.className == "check-box-Header") {
                if ($(this).is(':checked')) {
                    $('input:checkbox').prop("checked", true);
                    var dataSource = Controller.dataSource.pesquisaGrid._data
                    dataItem = [];
                    dataItem = dataSource;
                } else {
                    $('input:checkbox').prop("checked", false);
                    dataItem = [];
                }
            }
        });

        $("#btnImprimir").click(function () {
            if (dataItem.length > 0) {
                for (i = 0; i < dataItem.length; i++) {
                    var cdPedido = dataItem[i].CD_PEDIDO;
                    chamadaImpressao(cdPedido, urlCarregarDados)
                }


            } else {
                staticNotification.show("No mínimo um agendamento deve estar selecionado para realizar a impressão.", "error");
            }
        });

        $("#btnResPedido").click(function () {
            if (dataItem.length > 0) {
                var dadosArray = [];
                for (i = 0; i < dataItem.length; i++) {
                    dadosArray.push(dataItem[i].CD_SOLICITACAO);
                }
                chamadaImpressao(dadosArray, urlCarregarResumoPedidoLote)
            } else {
                staticNotification.show("No mínimo um agendamento deve estar selecionado para realizar a impressão.", "error");
            }
        });

        function chamadaImpressao(dados, url) {
            $.ajax({
                type: "POST",
                url: url,
                async: true,
                data: { dados: dados },
                success: function (data) {
                    var arr = data.binData;
                    var byteArray = new Uint8Array(arr);
                    var a = window.document.createElement('a');
                    a.href = window.URL.createObjectURL(new Blob([byteArray], { type: 'application/octet-stream' }));
                    a.download = data.nmArquivo;
                    document.body.appendChild(a)
                    a.click();
                    document.body.removeChild(a)
                    animacaocarregamentoStop()
                },
                beforeSend: function () {
                    animarCarregamento()
                },
                error: function (request, status, erro) {
                    console.log("Problema ocorrido: " + status + "\nDescição: " + erro);
                    console.log("Informações da requisição: \n" + request.getAllResponseHeaders());
                    animacaocarregamentoStop()
                }
            });

        }

    }
};
$(document).ready(function () {
    Controller.init();
});





