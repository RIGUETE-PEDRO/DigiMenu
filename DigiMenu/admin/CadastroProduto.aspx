<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroProduto.aspx.cs" Inherits="DigiMenu.CadastroProduto" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Cadastro de Produto</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
    <link href="../styles/styleProduto.css" rel="stylesheet"/>
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
                    <a class="nav-link " aria-current="page" href="FrmPainelAdministrativo.aspx">Home</a>
                    <a class="nav-link active" href="CadastroProduto.aspx">Produtos</a>
                    <a class="nav-link" href="CadastroCarrousel.aspx">Carousel</a>
                     <a class="nav-link" href="FrmPedidos.aspx">Pedidos</a>
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
                            <h2 class="form-title mb-4 h4">Produto</h2>
                            <div class="mb-3">
                                <label for="txtNome" class="form-label">Nome do Produto</label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" Placeholder="Digite o nome" />
                            </div>
                            <div class="mb-3">
                                <label for="txtDescricao" class="form-label">Descrição do Produto</label>
                                <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" Placeholder="Digite a descrição" />
                            </div>
                            <div class="row">
                                <div class="col-6 mb-3">
                                    <label for="txtPreco" class="form-label">Preço</label>
                                    <asp:TextBox ID="txtPreco" runat="server" CssClass="form-control" Placeholder="0,00" />
                                </div>
                                <div class="col-6 mb-3">
                                    <label for="txtEstoque" class="form-label">Estoque</label>
                                    <asp:TextBox ID="txtEstoque" runat="server" CssClass="form-control" Placeholder="0" />
                                </div>
                            </div>
                            <div class="form-check form-switch mb-3">
                                <input id="Checkbox1" runat="server" type="checkbox" class="form-check-input" />
                                <label class="form-check-label" for="Checkbox1">Oferta Produto</label>
                            </div>
                            <div class="mb-3">
                                
                                
                                <asp:FileUpload ID="File1" runat="server" />
                               <label class="form-label d-block">Imagem do Produto</label>
                                <asp:Image ID="imgPreview" runat="server" CssClass="img-thumbnail mb-2" Visible="false" Width="160" />
                            </div>

                            <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" CssClass="d-block mb-2 small"></asp:Label>

                            <asp:Button ID="btnCadastrar" ValidationGroup="cad" OnClick="btnCadastrar_Click" runat="server" Text="Cadastrar" CssClass="btn btn-primary w-100 mb-2" />
                            <asp:Button ID="btnAtualizar" OnClick="Atualizar_Click" runat="server" Text="Atualizar" CssClass="btn btn-success w-100 mb-2" Visible="false" />
                            <a id="btnVoltar" runat="server" href="CadastroProduto.aspx" class="btn btn-secondary w-100 mb-2" visible="false">Cancelar</a>
                           
                        </div>
                    </div>
                </div>

                <!-- Coluna direita: Tabela -->
                <div class="col-12 col-lg-7">
                    <div class="card shadow-sm h-100">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-3">
                                <h2 class="h5 mb-0">Produtos Cadastrados</h2>
                                <span class="text-muted small">Lista</span>
                            </div>
                            <div class="table-responsive">
                                <table class="table table-sm table-hover align-middle mb-0">
                                    <thead class="table-dark">
                                        <tr>
                                            <th scope="col">CÓD</th>
                                            <th scope="col">NOME</th>
                                            <th scope="col">PREÇO</th>
                                            <th scope="col">STATUS</th>
                                            <th scope="col">ESTOQUE</th>
                                            <th scope="col">AÇÕES</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptProdutos">
                                            <ItemTemplate>
                                                <tr>
                                                    <th scope="row"><%# Eval("IdProduto") %></th>
                                                    <td><%# Eval("Nome") %></td>
                                                    <td>R$ <%# Eval("Preco", "{0:F2}") %></td>
                                                    <td>
                                                        <%# (bool)Eval("Ativo") 
                                                            ? "<span class='badge bg-success'>Ativo</span>" 
                                                            : "<span class='badge bg-secondary'>Inativo</span>" %>
                                                    </td>
                                                    <td><%# Eval("Estoque") %></td>
                                                    <td class="text-nowrap">
                                                        <asp:ImageButton ID="btnVisualizar" ImageUrl="~/img/visualizar.svg" AlternateText="Visualizar" runat="server" OnClick="btnVisualizar_Click" CommandArgument='<%# Eval("IdProduto") %>' CausesValidation="false" />
                                                        <asp:ImageButton ID="btnEditar" ImageUrl="~/img/edit.svg" AlternateText="Editar" runat="server" OnClick="btnEditar_Click" CommandArgument='<%# Eval("IdProduto") %>' CausesValidation="false" />
                                                        <asp:ImageButton ID="btnExcluir" ImageUrl="~/img/deletar.svg" AlternateText="Excluir" runat="server" OnClick="btnExcluir_Click" CommandArgument='<%# Eval("IdProduto") %>' CausesValidation="false" OnClientClick="return confirm('Confirma excluir?');" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
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
