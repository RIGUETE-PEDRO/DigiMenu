<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmPainelAdministrativo.aspx.cs" Inherits="DigiMenu.admin.FrmPainelAdministrativo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
     <link href="../styles/StyleAdmin.css" rel="stylesheet" />
</head>
<body>
    <!--nav bar -->
    <nav class="navbar navbar-expand-lg bg-body-tertiary">
  <div class="container-fluid">
     <img id="imgLogo" src="../img/logo.png" alt="Logo" />
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
      <div class="navbar-nav">
        <a class="nav-link active" aria-current="page" href="#">Home</a>
        <a class="nav-link" href="CadastroProduto.aspx">Produtos</a>
        <a class="nav-link" href="CadastroCarrousel.aspx">Carousel</a>
        <a class="nav-link" href="FrmPedidos.aspx">Pedidos</a>
        
      </div>
    </div>
  </div>
</nav>

    <h2>Painel Administrativo</h2>



     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI" crossorigin="anonymous"></script>

</body>
</html>
