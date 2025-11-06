<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DigiMenu.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB" crossorigin="anonymous" />
    <link href="styles/StylePrincipal.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <!-- nav bar do bootstrap -->
        <nav class="navbar navbar-expand-lg navbar-dark bg-dark navbarDesigner">
            <div class="container-fluid">
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarTogglerDemo03" aria-controls="navbarTogglerDemo03" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <a class="navbar-brand" href="#">
                    <img id="imgLogo" src="img/logo.png" alt="Logo" />
                </a>

                <div class="collapse navbar-collapse" id="navbarTogglerDemo03">
                    <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                        <li class="nav-item">
                            <a class="nav-link active" aria-current="page" href="#">Principal</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#" aria-disabled="true">Lanches</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link " href="#" aria-disabled="true">Pizzas</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="#" aria-disabled="true">Bebidas</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="StatusPedido.aspx" aria-disabled="true">Pedidos</a>
                        </li>
                    </ul>
                    <div class="d-flex" role="search">
                        <asp:TextBox ID="txtPesquisa" runat="server" CssClass="form-control me-2" Placeholder="Digite o produto" />
                        <asp:Button ID="btnPesquisar" runat="server" CssClass="btn btn-outline-success" Text="Pesquisar" OnClick="btnPesquisar_Click" />
                    </div>

                    <div id="cart">
                        <a href="carrinho.aspx" class="cart">
                            <img src="img/shopping_cart_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24.svg" alt="imagem de carinho" />

                        </a>
                    </div>

                    <div class="buttonLogin" id="divLogin" runat="server">
                        <a href="FrmLogin.aspx" class="login">
                            <p>Login</p>
                        </a>
                    </div>
                    <div class="userDisplay d-flex align-items-center" id="divUser" runat="server" style="display: none;">
                        <span class="me-3" id="lblUserName" runat="server"></span>

                        <div class="userDisplay d-flex align-items-center" id="div1" runat="server" style="display: none;">



                            <asp:LinkButton ID="btnLogout" runat="server" CssClass="logout" OnClick="btnLogout_Click">Sair</asp:LinkButton>
                        </div>

                    </div>

                </div>
            </div>
        </nav>

        <div class="carrousel">

            <div id="carouselExampleIndicators" class="carousel slide" data-ride="carousel">
                <ol class="carousel-indicators">
                    <li data-target="#carouselExampleIndicators" data-slide-to="0" class="active"></li>
                    <li data-target="#carouselExampleIndicators" data-slide-to="1"></li>
                    <li data-target="#carouselExampleIndicators" data-slide-to="2"></li>
                </ol>
                <div class="carousel-inner">
                    <div class="carousel-item active">
                        <img class="d-block w-100" src="..." alt="First slide">
                    </div>
                    <div class="carousel-item">
                        <img class="d-block w-100" src="..." alt="Second slide">
                    </div>
                    <div class="carousel-item">
                        <img class="d-block w-100" src="..." alt="Third slide">
                    </div>
                </div>
                <a class="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-slide="prev">
                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                    <span class="sr-only">Previous</span>
                </a>
                <a class="carousel-control-next" href="#carouselExampleIndicators" role="button" data-slide="next">
                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                    <span class="sr-only">Next</span>
                </a>
            </div>
        </div>

        <!--filtros-->

        <div class="container mt-4">
            <h2>Filtros</h2>

            <!-- Radio buttons para escolher o tipo de filtro -->
            <asp:RadioButton ID="rbCategoria" GroupName="filtro" Text="Categoria" runat="server" AutoPostBack="true" OnCheckedChanged="Filtro_CheckedChanged" Checked="true" />
            <asp:RadioButton ID="rbPreco" GroupName="filtro" Text="Preço" runat="server" AutoPostBack="true" OnCheckedChanged="Filtro_CheckedChanged" />
           
            <div class="mt-2">
                <!-- Inputs correspondentes -->
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select mb-2">
                    <asp:ListItem Text="Selecione a categoria" Value="" />
                    <asp:ListItem Text="Bebidas" Value="Bebidas" />
                    <asp:ListItem Text="Comidas" Value="Comidas" />
                </asp:DropDownList>

                <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control mb-2" Placeholder="Digite o preço"></asp:TextBox>

                <asp:DropDownList ID="ddlOferta" runat="server" CssClass="form-select mb-2">
                    <asp:ListItem Text="Selecione a oferta" Value="" />
                    <asp:ListItem Text="10%" Value="10" />
                    <asp:ListItem Text="20%" Value="20" />
                </asp:DropDownList>
            </div>



        </div>


        <div class="container mt-4">
            <div class="row" id="produtosRow">
                <asp:Repeater ID="rptProdutos" runat="server">
                    <ItemTemplate>
                        <div class="col-12 col-sm-6 col-md-4 col-lg-3 mb-4 d-flex">
                            <div class="card flex-fill">
                                <img src='<%# Eval("Imagem") %>' class="card-img-top produto-imagem" alt='<%# Eval("Nome") %>' />
                                <div class="card-body d-flex flex-column">
                                    <h5 class="card-title"><%# Eval("Nome") %></h5>
                                    <p class="card-text flex-grow-1"><%# Eval("Descricao") %></p>
                                </div>
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item">Preço: R$ <%# Eval("Preco") %></li>
                                    <li class="list-group-item">Estoque: <%# Eval("Estoque") %></li>
                                </ul>
                                <div class="card-body d-flex justify-content-between">
                                    <asp:Button Text="Comprar" OnClick="compra_Click" runat="server" ID="compra" class="btn btn-primary btn-sm" />
                                    <asp:Button Text="Detalhes" runat="server" class="btn btn-secondary btn-sm" />



                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>



    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI" crossorigin="anonymous"></script>
</body>
</html>
