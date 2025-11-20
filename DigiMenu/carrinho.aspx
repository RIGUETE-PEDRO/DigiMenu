<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="carrinho.aspx.cs" Inherits="DigiMenu.carrinho" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Carrinho</title>
    <link href="styles/Carrinho.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB" crossorigin="anonymous" />
    <link href="styles/Carrinho.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
    <nav class="navbar navbar-expand-lg navbarDesigner">
        <div class="container-fluid">
           <a class="navbar-brand" href="#">
               <img id="imgLogo" src="img/logo.png" alt="Logo" />
           </a>
            <div class="collapse navbar-collapse">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a class="nav-link text-white" aria-current="page" href="Default.aspx">Principal</a>
                    </li>
                </ul>
            </div>
        </div>
    </nav>



        <div class="container cart-container">
            <div class="embacar" runat="server">
                <div class="cart-header">
                    <h2 class="mb-0">Meu carrinho</h2>
                    <div class="endereco" id="endereco" runat="server">

                        
                        <label>Cidade</label>
                        <input type="text" id="cidadeEntrega" name="cidadeEntrega" class="form-control" placeholder="Digite sua cidade" runat="server"/>
                        <label>Logadouro</label>
                        <input type="text" id="logadouroEntrega" name="logadouroEntrega" class="form-control" placeholder="Digite seu logadouro" runat="server"/>
                        <label>Número</label>
                        <input type="text" id="numeroEntrega" name="numeroEntrega" class="form-control" placeholder="Digite o número" runat="server"/>
                        <label>Complemento</label>
                        <input type="text" id="complementoEntrega" name="complementoEntrega" class="form-control" placeholder="Digite o complemento" runat="server"/>


                        <asp:Button ID="btnFinalizar" runat="server" CssClass="btn btn-success btn-lg" Text="Finalizar pedido" OnClick="btnFinalizar_Click" runat="server"/>

                    </div>
                    <asp:Button ID="btnContinuarComprando" runat="server" Text="Continuar comprando" CssClass="btn btn-outline-secondary" OnClick="btnContinuarComprando_Click" />
                </div>

                <asp:PlaceHolder ID="phMensagens" runat="server" />

                <asp:Panel ID="pnlCarrinhoVazio" runat="server" Visible="false" CssClass="cart-empty">
                    <p>Seu carrinho está vazio.</p>
                    <a href="Default.aspx" class="btn btn-primary">Ver produtos</a>
                </asp:Panel>

                <asp:Panel ID="pnlCarrinho" runat="server" Visible="false">
                    <div class="table-responsive">
                        <table class="table align-middle">
                            <thead>
                                <tr>
                                    <th>Produto</th>
                                    <th></th>
                                    <th>Preço</th>
                                    <th>Quantidade</th>
                                    <th>Total</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptCarrinho" runat="server" OnItemCommand="rptCarrinho_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <img src="<%# Eval("Imagem") %>" class="produto-img" alt="<%# Eval("Nome") %>" />
                                            </td>
                                            <td>
                                                <strong><%# Eval("Nome") %></strong>
                                            </td>
                                            <td>R$ <%# string.Format("{0:N2}", Eval("PrecoUnitario")) %></td>
                                            <td>
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtQuantidade" runat="server" CssClass="form-control" Text='<%# Eval("Quantidade") %>' />
                                                    <asp:LinkButton ID="btnAtualizar" runat="server" CssClass="btn btn-outline-secondary" CommandName="Atualizar" CommandArgument='<%# Eval("IdItemCarrinho") %>'>Atualizar</asp:LinkButton>
                                                </div>
                                            </td>
                                            <td>R$ <%# string.Format("{0:N2}", Eval("PrecoTotal")) %></td>
                                            <td>
                                                <asp:LinkButton ID="btnRemover" runat="server" CssClass="btn btn-outline-danger btn-sm" CommandName="Remover" CommandArgument='<%# Eval("IdItemCarrinho") %>'>Remover</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <div class="d-flex justify-content-end align-items-center gap-3">
                        <h4 class="me-3">Total:
                            <asp:Label ID="lblTotal" runat="server" Text="R$ 0,00" /></h4>
                        <asp:Button ID="PedirEndereco" runat="server" CssClass="btn btn-success btn-lg" Text="Finalizar pedido" OnClick="PedirEndereco_Click" />
                    </div>
                </asp:Panel>
            </div>
        </div>

    </form>
    <footer>
        <p>&copy; 2025 Pedro Riguete & Maria Massucato. Todos os direitos reservados.</p>
    </footer>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI" crossorigin="anonymous"></script>
</body>
</html>
