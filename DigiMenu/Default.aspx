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
        <nav class="navbar navbar-expand-lg navbarDesigner">
            <div class="container-fluid">
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarTogglerDemo03" aria-controls="navbarTogglerDemo03" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <a class="navbar-brand" href="#">
                    <img id="imgLogo" src="img/logo.png" alt="Logo" />
                </a>

                 <!-- Navegação Principal -->
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

                    <!-- Barra de Pesquisa com Controles de Servidor -->
                    <div class="d-flex" role="search">
                        <asp:TextBox ID="txtPesquisa" runat="server" CssClass="form-control me-2" Placeholder="Digite o produto" />
                        <asp:Button ID="btnPesquisar" runat="server" CssClass="btn btn-primary" Text="Pesquisar" OnClick="btnPesquisar_Click" />
                    </div>

                    <!-- Carrinho -->
                    <div id="cart">
                        <a href="carrinho.aspx" class="cart">
                            <img src="img/shopping_cart_24dp_FFFFFF_FILL0_wght400_GRAD0_opsz24.svg" alt="imagem de carinho" />

                        </a>
                    </div>

                    <!-- Login/Usuário com Controles de Servidor -->
                    <div class="buttonLogin" id="divLogin" runat="server">
                        <a href="FrmLogin.aspx" class="login">
                            <p class="pLogin">Login</p>
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


        <!-- Carrossel (Mantido como estava) -->
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



               <!-- FILTROS: Estrutura mais limpa com Flexbox -->
        <div class="container mt-4">
            <section class="filters">
                <h2>Filtros</h2>
                <div class="filter-options">
                    <div class="filter-group">
                        <!-- Radio buttons -->
                        <asp:RadioButton ID="rbCategoria" GroupName="filtro" Text="Categoria" runat="server" AutoPostBack="true" OnCheckedChanged="Filtro_CheckedChanged" Checked="true" />
                        <asp:RadioButton ID="rbPreco" GroupName="filtro" Text="Preço" runat="server" AutoPostBack="true" OnCheckedChanged="Filtro_CheckedChanged" />
                    </div>
                    
                    <!-- DropDownList Categoria -->
                    <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select" Placeholder="Selecione a categoria">
                        <asp:ListItem Text="Selecione a categoria" Value="" />
                        <asp:ListItem Text="Bebidas" Value="Bebidas" />
                        <asp:ListItem Text="Comidas" Value="Comidas" />
                    </asp:DropDownList>

                    <!-- TextBox Preço -->
                    <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control" Placeholder="Digite o preço"></asp:TextBox>

                    <!-- DropDownList Oferta -->
                    <asp:DropDownList ID="ddlOferta" runat="server" CssClass="form-select">
                        <asp:ListItem Text="Selecione a oferta" Value="" />
                        <asp:ListItem Text="10%" Value="10" />
                        <asp:ListItem Text="20%" Value="20" />
                    </asp:DropDownList>
                </div>
            </section>
        </div>

        <!-- PRODUTOS: Novo layout de Card com Repeater -->
        <div class="container mt-4">
            <div class="row product-grid" id="produtosRow">
                <asp:Repeater ID="rptProdutos" runat="server">
                    <ItemTemplate>
                        <!-- Usando col-lg-3 para 4 colunas em desktop, col-md-4 para 3 em tablet, col-sm-6 para 2 em mobile -->
                        <div class="col-12 col-sm-6 col-md-2 col-lg-3 mb-4 d-flex">
                            <div class="product-card">
                                <img src='<%# Eval("Imagem") %>' alt='<%# Eval("Nome") %>' />
                                <div class="product-info">
                                    <h5 class="card-title"><%# Eval("Nome") %></h5>
                                    <p class="card-text"><%# Eval("Descricao") %></p>
                                </div>
                                <ul class="product-details">
                                    <li class="price">Preço: R$ <%# Eval("Preco") %></li>
                                    <li>Estoque: <%# Eval("Estoque") %></li>
                                </ul>
                                <div class="product-actions">
                                    <!-- Botões com classes customizadas para o novo estilo -->
                                    <asp:Button Text="Comprar" OnClick="compra_Click" runat="server" ID="compra" CssClass="btn-custom btn-primary-custom" />
                                    <asp:Button Text="Detalhes" runat="server" CssClass="btn-custom btn-secondary-custom" />
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
