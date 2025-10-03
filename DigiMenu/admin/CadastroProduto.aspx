<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroProduto.aspx.cs" Inherits="DigiMenu.CadastroProduto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Cadastro de Produto</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
    <link href="../styles/styleProduto.css" rel="stylesheet" />
</head>
<body>
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <a href="FrmProdutos.aspx">FrmProdutos.aspx</a>
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
                <!-- Coluna esquerda: Formulário -->
                <div class="col-12 col-lg-5">
                    <div class="card shadow-sm h-100">
                        <div class="card-body">
                            <h2 class="form-title mb-4 h4">Novo Produto</h2>
                            <div class="mb-3">
                                <label for="txtNome" class="form-label">Nome do Produto</label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" Placeholder="Digite o nome" required="required"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtDescricao" class="form-label">Descrição do Produto</label>
                                <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" Placeholder="Digite a descrição" required="required"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtPreco" class="form-label">Preço do Produto</label>
                                <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Digite o preço" required="required"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEstoque" class="form-label">Quantidade em Estoque</label>
                                <asp:TextBox ID="txtEstoque" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Digite a quantidade em estoque" required="required"></asp:TextBox>
                            </div>
                            <div class="form-check form-switch mb-3">
                                <input id="Checkbox1" runat="server" type="checkbox" class="form-check-input" />
                                <label class="form-check-label" for="Checkbox1">Oferta Produto</label>
                            </div>
                            <div class="mb-4">
                                <label class="form-label d-block">Imagem do Produto</label>
                                <input id="File1" runat="server" type="file" class="form-control form-control-sm" />
                            </div>
                            <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" CssClass="d-block mb-2"></asp:Label>
                            <asp:Button ID="btnCadastrar" OnClick="btnCadastrar_Click" runat="server" Text="Cadastrar" CssClass="btn btn-primary w-100 product-submit mb-2" />
                            <a href="FrmPainelAdministrativo.aspx" class="btn w-100 btn-outline-secondary">Voltar ao Painel</a>
                        </div>
                    </div>
                </div>

                <!-- Coluna direita: Tabela -->
                <div class="col-12 col-lg-7">
                    <div class="card shadow-sm h-100">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-3">
                                <h2 class="h5 mb-0">Produtos Cadastrados</h2>
                                <span class="text-muted small">Lista </span>
                            </div>
                            <!--tabela responsiva-->
                            <div class="table-responsive">
                                <table class="table table-sm table-hover align-middle mb-0">
                                    <thead class="table-dark">
                                        <tr>
                                            <th scope="col">CÓD</th>
                                            <th scope="col">NOME</th>
                                            <th scope="col">PREÇO</th>
                                             <th scope="col">STATUS</th>
                                            <th scope="col">ESTOQUE</th>
                                            <th scope="col">EDIT</th>
                                        </tr>
                                    </thead>
                                    
                                    <!--resposavel por onde os dados vao aparecer-->
                                    <tbody runat="server" class="table-group-divider" id="tblProdutos">
                                      
                                    </tbody>
                                </table>
                            </div>

                        </div>
                    </div>
                </div>
                <!-- Fim col tabela -->
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
</body>
</html>
