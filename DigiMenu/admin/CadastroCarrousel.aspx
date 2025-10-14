<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroCarrousel.aspx.cs" Inherits="DigiMenu.admin.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Cadastro de Carrousel</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
    <link href="../styles/CadastroCarrousel.css" rel="stylesheet" />
</head>
<body>

    <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">DigiMenu</a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
                <div class="navbar-nav">
                    <a class="nav-link active" aria-current="page" href="FrmPainelAdministrativo.aspx">Home</a>
                    <a class="nav-link" href="CadastroProduto.aspx">Produtos</a>
                    <a class="nav-link" href="#">Carousel</a>
                </div>
            </div>
        </div>
    </nav>

    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row g-4 align-items-start">
                <div class="col-12 col-lg-5">
                    <div class="card shadow-sm h-100">
                        <div class="card-body">
                            <h2 class="form-title mb-4 h4">Produtos Ativos</h2>

                            <asp:Repeater ID="rptProdutos" runat="server">
                                <HeaderTemplate>
                                    <table class="table">
                                        <tr>
                                            <th>Nome</th>
                                            <th>Ativo</th>
                                            <th>Ordem</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Eval("Nome") %></td>
                                        <td>
                                            <asp:CheckBox ID="chkAtivo" runat="server" Checked='<%# Eval("Ativo") %>' />
                                            <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id") %>' />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrdem" runat="server" Text='<%# Eval("Ordem") %>' Width="50px"
                                                Enabled='<%# (bool)Eval("AtivoNoCarrossel") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar Configurações" CssClass="btn btn-primary mt-3" OnClick="btnSalvar_Click" />
                                </FooterTemplate>
                            </asp:Repeater>


                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
