<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPedidos.aspx.cs" Inherits="DigiMenu.admin.Pedidos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pedidos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <link href="../styles/StyleAdmin.css" rel="stylesheet" />
</head>
<body>
    <nav class="navbar navbar-expand-lg navbarDesigner">
        <div class="container-fluid">
            <img id="imgLogo" src="../img/logo.png" alt="Logo" />
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
                <div class="navbar-nav">
                    <a class="nav-link text-white" aria-current="page" href="administracao">Início</a>
                    <a class="nav-link text-white" href="Cadastro-de-produto">Produtos</a>
                    <a class="nav-link text-white" href="Configurar-carrousel">Carousel</a>
                    <a class="nav-link text-white active" href="Lista-de-Pedidos">Pedidos</a>
                    <a class="nav-link text-white" href="../Default.aspx">Visualizar Página</a>
                </div>
            </div>
        </div>
    </nav>

    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2 class="mb-4">Pedidos pendentes</h2>

            <asp:PlaceHolder ID="phMsg" runat="server" />

            <asp:Repeater ID="rptPedidos" runat="server" OnItemCommand="rptPedidos_ItemCommand">
                <ItemTemplate>
                    <div class="card mb-4 order-card">
                        <div class="card-body p-3 order-header">
                            <div>
                                <h5 class="mb-1">Pedido #<%# Eval("IdPedido") %></h5>
                                <div class="text-muted small">
                                    Data: <%# ((DateTime)Eval("Data")).ToString("dd/MM/yyyy HH:mm") %> |
                                    Cliente: <%# Eval("Cliente") %> |
                                    Status:
                                    <span class='<%# (DataBinder.Eval(Container.DataItem,"Status") as string) == "Pendente" ? "badge badge-status badge-success" : "badge badge-status badge-secondary" %>'>
                                        <%# Eval("Status") %>
                                    </span>
                                    <div>
                                        <span class="me-3">Endereço</span>
                                        <div class="text-muted">
                                            <strong>Cidade: </strong><%# Eval("Cidade") %>,<strong>Numero: </strong><%# Eval("Numero") %> ,<strong>Complemento</strong> <%# Eval("Complemento") %>,<strong>Logradouro: </strong><%# Eval("Logradouro") %>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="text-end align-self-start">
                                <div class="mb-2 total-label">Total: R$ <%# string.Format("{0:N2}", Eval("Total")) %></div>
                                <asp:LinkButton ID="btnAceitar" runat="server" CssClass="btn btn-success btn-sm me-2" CommandName="Aceitar" CommandArgument='<%# Eval("IdPedido") %>'>Aceitar</asp:LinkButton>
                                <asp:LinkButton ID="btnNegar" runat="server" CssClass="btn btn-danger btn-sm" CommandName="Negar" CommandArgument='<%# Eval("IdPedido") %>'>Negar</asp:LinkButton>
                            </div>
                        </div>

                        <div class="card-body pt-0 pb-3 px-3">
                            <asp:Repeater ID="rptItens" runat="server">
                                <HeaderTemplate>
                                    <div class="table-responsive">
                                        <table class="table table-sm mb-0">
                                            <thead class="table-light">
                                                <tr>
                                                    <th>Produto</th>
                                                    <th class="text-end">Qtd</th>
                                                    <th class="text-end">Unit.</th>
                                                    <th class="text-end">Subtotal</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Produto") %></td>
                                        <td class="text-end"><%# Eval("Quantidade") %></td>
                                        <td class="text-end">R$ <%# string.Format("{0:N2}", Eval("PrecoUnitario")) %></td>
                                        <td class="text-end">R$ <%# string.Format("{0:N2}", (decimal)Eval("PrecoUnitario") * (int)Eval("Quantidade")) %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
                                        </table>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlSemPedidos" runat="server" Visible="false" CssClass="alert alert-info mt-3">
                Nenhum pedido pendente.
            </asp:Panel>

            <!-- Seção de pedidos não pendentes (mantive IDs que o code-behind usa) -->
            <hr class="my-5" />
            <h2>Pedidos (não pendentes)</h2>

            <div class="row g-3 align-items-end mt-2">
                <div class="col-12 col-md-4">
                    <label class="form-label">Novo status</label>
                    <asp:DropDownList ID="ddlNovoStatus" runat="server" CssClass="form-select" />
                </div>
                <div class="col-12 col-md-8 d-flex">
                    <asp:Button ID="btnAplicarStatus" runat="server" CssClass="btn btn-primary ms-auto" Text="Aplicar status aos selecionados" OnClick="btnAplicarStatus_Click" />
                </div>
            </div>

            <div class="mt-3">
                <asp:CheckBoxList ID="cblPedidosAceitos" runat="server" CssClass="list-group" RepeatLayout="Flow" />
            </div>

            <asp:Panel ID="pnlSemAceitos" runat="server" Visible="false" CssClass="alert alert-info mt-3">
                Nenhum pedido para listar.
            </asp:Panel>

        </div>
    </form>
</body>
</html>
