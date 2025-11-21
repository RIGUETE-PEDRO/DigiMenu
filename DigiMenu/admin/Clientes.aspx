<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="DigiMenu.admin.Clientes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Clientes</title>
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
                    <a class="nav-link text-white" href="administracao">Inicio</a>
                    <a class="nav-link text-white" href="Cadastro-de-produto">Produtos</a>
                    <a class="nav-link text-white" href="Configurar-carrousel">Carousel</a>
                    <a class="nav-link text-white" href="Lista-de-Pedidos">Pedidos</a>
                    <a class="nav-link text-white" href="../Default.aspx">Visualizar Página</a>
                    <a class="nav-link text-white active" href="#">Clientes</a>
                </div>
            </div>
        </div>
    </nav>
    <form id="form1" runat="server">
        <div class="container-fluid mt-3">
            <h2 class="h4 mb-3">Clientes</h2>
            <asp:PlaceHolder ID="phMensagens" runat="server" />
            <div class="row">
                <div class="col-12 col-lg-7">
                    <div class="card shadow-sm mb-4">
                        <div class="card-body">
                            <div class="d-flex justify-content-between align-items-center mb-2">
                                <h5 class="mb-0">Lista de Clientes</h5>
                                <span class="text-muted small">Gerenciar</span>
                            </div>
                            <div class="table-responsive">
                                <table class="table table-sm table-hover align-middle mb-0">
                                    <thead class="table-dark">
                                        <tr>
                                            <th>ID</th>
                                            <th>Nome</th>
                                            <th>Email</th>
                                            <th>Telefone</th>
                                            <th>Status</th>
                                            <th>Ações</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptClientes" runat="server" OnItemCommand="rptClientes_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%# Eval("Id") %></td>
                                                    <td><%# Eval("Nome") %></td>
                                                    <td><%# Eval("Email") %></td>
                                                    <td><%# Eval("Telefone") %></td>
                                                    <td>
                                                        <%# (string)Eval("Status") == "Bloqueado" ? "<span class='badge bg-danger'>Bloqueado</span>" : "<span class='badge bg-success'>Ativo</span>" %>
                                                    </td>
                                                    <td class="text-nowrap">
                                                        <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-primary">Editar</asp:LinkButton>
                                                        <asp:LinkButton ID="btnToggle" runat="server" CommandName="Toggle" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-outline-warning"><%# (string)Eval("Status") == "Bloqueado" ? "Desbloquear" : "Bloquear" %></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="pnlSemClientes" runat="server" Visible="false" CssClass="alert alert-info mt-3">Nenhum cliente cadastrado.</asp:Panel>
                        </div>
                    </div>
                </div>
                <div class="col-12 col-lg-5">
                    <asp:Panel ID="pnlEdicao" runat="server" Visible="false" CssClass="card shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title">Editar Cliente</h5>
                            <asp:HiddenField ID="hfClienteId" runat="server" />
                            <div class="mb-3">
                                <label class="form-label" for="txtNome">Nome</label>
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label" for="txtEmail">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label" for="txtTelefone">Telefone</label>
                                <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control" />
                            </div>
                            <div class="d-flex gap-2">
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-primary" OnClick="btnSalvar_Click" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-secondary" OnClick="btnCancelar_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>
    <footer>
        <p>&copy; 2025 Pedro Riguete & Maria Massucato. Todos os direitos reservados.</p>
    </footer>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
</body>
</html>
