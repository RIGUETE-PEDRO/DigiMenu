<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLogin.aspx.cs" Inherits="DigiMenu.admin.FrmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
      <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
      <link href="styles/StyleLogin.css" rel="stylesheet" />
</head>
<body>
        <form id="form1" runat="server">
    <div class="container d-flex justify-content-center align-items-center" style="height: 100vh;">
        <div class="card" style="width: 100%; max-width: 400px;">
            <div class="card-body">
                <h5 class="card-title text-center mb-4  text">Login</h5>

                <!-- Formulário de cadastro -->
                <div class="mb-3">
                    <label for="txtUsuario" class="form-label text">Usuario</label>
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" Placeholder="Digite o número de telefone ou email" required="required"></asp:TextBox>
                </div>
                   

                <div class="mb-3">
                    <label for="txtSenha" class="form-label text">Senha</label><link href="../styles/StyleLogin.css" rel="stylesheet" />
                    <asp:TextBox ID="txtSenha" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Digite sua senha" required="required"></asp:TextBox>
                </div>
                 <asp:PlaceHolder ID="PlaceHolderMensagens" runat="server"></asp:PlaceHolder>
         
                <asp:Button ID="btnCadastrar" OnClick="btnLogin_Click" runat="server" Text="Login" CssClass="btn btn-primary w-100 Login" />
                <a href="FrmCadastro.aspx" class="btn btn-primary w-100 cadastro ">Cadastrar</a>



               
            </div>
        </div>
    </div>
</form>
</body>
</html>
