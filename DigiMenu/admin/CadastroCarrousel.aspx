<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CadastroCarrousel.aspx.cs" Inherits="DigiMenu.admin.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Cadastro de Carrousel</title>
     <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous">
    <link href="../styles/CadastroCarrousel.css" rel="stylesheet" />
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
                <a class="nav-link active" aria-current="page" href="FrmPainelAdministrativo.aspx">Home</a>
                <a class="nav-link" href="CadastroProduto.aspx">Produtos</a>
                <a class="nav-link" href="#">Carousel</a>
            </div>
        </div>
    </div>
</nav>

    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
