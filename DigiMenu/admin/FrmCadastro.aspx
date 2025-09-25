<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCadastro.aspx.cs" Inherits="DigiMenu.FrmCadastro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<title>Cadastro - DigiMenu</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
</head>
<body>

    <form id="form1" runat="server">
        <div class="container d-flex justify-content-center align-items-center" style="height: 100vh;">
            <div class="card" style="width: 100%; max-width: 400px;">
                <div class="card-body">
                    <h5 class="card-title text-center mb-4">Cadastro</h5>

                    <!-- Formulário de cadastro -->
                    <div class="mb-3">
                        <label for="txtNome" class="form-label">Nome</label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" Placeholder="Digite seu nome" required="required"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">E-mail</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Digite seu e-mail" required="required"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtSenha" class="form-label">Senha</label>
                        <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Digite sua senha" required="required"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label for="txtConfirmaSenha" class="form-label">Confirmar Senha</label>
                        <asp:TextBox ID="txtConfirmaSenha" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Confirme sua senha" required="required"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnCadastrar" runat="server" Text="Cadastrar" CssClass="btn btn-primary w-100" />
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
</body>
</html>
