<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StatusPedido.aspx.cs" Inherits="DigiMenu.StatusPedido" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB" crossorigin="anonymous" />
    <link href="styles/statusPedido.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark navbarDesigner">
            <div class="container-fluid">
                <img id="imgLogo" src="img/logo.png" alt="logo" />
                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                        <li class="nav-item">
                            <a class="nav-link" aria-current="page" href="Default.aspx">Principal</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        

            <div class="container mt-4">
                <h2>SEUS PEDIDOS</h2>
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
