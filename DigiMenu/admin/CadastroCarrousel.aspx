<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroCarrousel.aspx.cs" Inherits="DigiMenu.admin.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Cadastro de Carrousel</title>
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
                    <a class="nav-link text-white active" href="#">Carousel</a>
                    <a class="nav-link text-white" href="Lista-de-Pedidos">Pedidos</a>
                    <a class="nav-link text-white" href="../Default.aspx">Visualizar Página</a>
                </div>
            </div>
        </div>
    </nav>

    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row justify-content-center">
                <div class="col-12 col-md-10 col-lg-6">
                    <div class="card carousel-card shadow-sm my-5">
                        <div class="card-body">
                            <h3 class="carousel-title mb-4">Carrossel Ativos</h3>

                            <asp:PlaceHolder ID="PlaceHolderMensagens" runat="server"></asp:PlaceHolder>

                            <div class="table-responsive">
                                <table class="table carousel-table mb-0">
                                    <thead>
                                        <tr>
                                            <th class="align-middle">Nome</th>
                                            <th class="text-center" style="width:90px;">Ativo</th>
                                            <th class="text-center" style="width:120px;">Ordem</th>
                                        </tr>
                                    </thead>

                                    <asp:Repeater ID="rptProdutos" runat="server">
                                        <ItemTemplate>
                                            <tr class="carousel-row">
                                                <td class="py-3 align-middle">
                                                    <asp:Literal ID="litNome" runat="server" Text='<%# Eval("Nome") %>' />
                                                </td>
                                                <td class="text-center align-middle">
                                                    <asp:CheckBox ID="chkAtivo" runat="server" Checked='<%# Eval("Ativo") %>' CssClass="form-check-input" />
                                                </td>
                                                <td class="text-center align-middle">
                                                    <asp:TextBox ID="txtOrdem" runat="server" CssClass="form-control input_number d-inline-block" Text='<%# Eval("Ordem") %>' />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>

                            <div class="mt-4">
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar Configurações" CssClass="btn btn-primary" OnClick="btnSalvar_Click" />
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

</body>
</html>