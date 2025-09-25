<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmLogin.aspx.cs" Inherits="DigiMenu.FrmLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
</head>
<body>
<div class="container d-flex justify-content-center align-items-center" style="height: 100vh;">
    <div class="card" style="width: 100%; max-width: 400px;">
      <div class="card-body">
        <h5 class="card-title text-center mb-4">Login</h5>
        <!-- Formulário de login -->
        <form action="login_process.php" method="POST" aria-labelledby="loginForm">
          <div class="mb-3">
            <label for="username" class="form-label">Nome de Usuário</label>
            <input type="text" class="form-control" id="username" name="username" placeholder="Digite seu nome de usuário" required aria-describedby="usernameHelp">
           
          </div>
          <div class="mb-3">
            <label for="password" class="form-label">Senha</label>
            <input type="password" class="form-control" id="password" name="password" placeholder="Digite sua senha" required aria-describedby="passwordHelp">
            
          </div>
          <div class="mb-3 form-check">
            <input type="checkbox" class="form-check-input" id="rememberMe" name="rememberMe">
            <label class="form-check-label" for="rememberMe">Lembrar-me</label>
          </div>
          <button type="submit" class="btn btn-primary w-100">Entrar</button>
        </form>
        <div class="mt-3 text-center">
          <a href="#" class="text-decoration-none">Esqueceu a senha?</a>
        </div>
      </div>
    </div>
  </div>

  <!-- Bootstrap JS -->
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI" crossorigin="anonymous"></script>

</body>
</html>
