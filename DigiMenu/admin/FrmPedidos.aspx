<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPedidos.aspx.cs" Inherits="DigiMenu.admin.Pedidos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Pedidos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
    <link href="../styles/StyleAdmin.css" rel="stylesheet" />
</head>
<body>
        <nav class="navbar navbar-expand-lg bg-body-tertiary">
  <div class="container-fluid">
    <img id="imgLogo" src="../img/logo.png" alt="Logo" />
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
      <div class="navbar-nav">
        <a class="nav-link" aria-current="page" href="administracao">Home</a>
        <a class="nav-link" href="Cadastro-de-produto">Produtos</a>
        <a class="nav-link" href="Configurar-carrousel">Carousel</a>
        <a class="nav-link active" href="Lista-de-Pedidos">Pedidos</a>
      </div>
    </div>
  </div>
</nav>

    <form id="form1" runat="server">
        <div class="container mt-4">
            <h2>Pedidos pendentes</h2>
            <asp:PlaceHolder ID="phMsg" runat="server" />

            <asp:Repeater ID="rptPedidos" runat="server" OnItemCommand="rptPedidos_ItemCommand">
                <HeaderTemplate>
                    <div class="list-group">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="list-group-item">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <strong>Pedido #<%# Eval("IdPedido") %></strong>
                                <div class="text-muted">Data: <%# ((DateTime)Eval("Data")).ToString("dd/MM/yyyy HH:mm") %> | Cliente: <%# Eval("Cliente") %> | Status: <%# Eval("Status") %></div>
                            </div>
                            <div>
                                <span class="me-3">Total: R$ <%# string.Format("{0:N2}", Eval("Total")) %></span>
                                <asp:LinkButton ID="btnAceitar" runat="server" CssClass="btn btn-success btn-sm me-2" CommandName="Aceitar" CommandArgument='<%# Eval("IdPedido") %>'>Aceitar</asp:LinkButton>
                                <asp:LinkButton ID="btnNegar" runat="server" CssClass="btn btn-danger btn-sm" CommandName="Negar" CommandArgument='<%# Eval("IdPedido") %>'>Negar</asp:LinkButton>
                            </div>
                        </div>
                        <div class="mt-2">
                            <asp:Repeater ID="rptItens" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-sm mb-0">
                                        <thead>
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
                                    </tbody></table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlSemPedidos" runat="server" Visible="false" CssClass="alert alert-info mt-3">
                Nenhum pedido pendente.
            </asp:Panel>
        </div>
    </form>
</body>
</html>
