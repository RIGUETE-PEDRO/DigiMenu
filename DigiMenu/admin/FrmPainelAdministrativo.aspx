<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPainelAdministrativo.aspx.cs" Inherits="DigiMenu.admin.FrmPainelAdministrativo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous"/>
     <link href="../styles/StyleAdmin.css" rel="stylesheet" />
</head>
<body>
    <!--nav bar -->
    <nav class="navbar navbar-expand-lg navbarDesigner">
  <div class="container-fluid">
     <img id="imgLogo" src="../img/logo.png" alt="Logo" />
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>

    <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
      <div class="navbar-nav">
        <a class="nav-link text-white active" aria-current="page" href="#">Início</a>
        <a class="nav-link text-white" href="Cadastro-de-produto">Produtos</a>
        <a class="nav-link text-white" href="Configurar-carrousel">Carousel</a>
        <a class="nav-link text-white" href="Lista-de-Pedidos">Pedidos</a>
        <a class="nav-link text-white" href="../Default.aspx">Visualizar Página</a>
      </div>
    </div>
  </div>
</nav>

    <div class="titulo">
        <h2>Painel Administrativo</h2>
    </div>

    <footer>
        <p>&copy; 2025 Pedro Riguete & Maria Massucato. Todos os direitos reservados.</p>
    </footer>

     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI" crossorigin="anonymous"></script>

</body>
</html>
