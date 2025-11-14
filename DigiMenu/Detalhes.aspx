<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detalhes.aspx.cs" Inherits="DigiMenu.Detalhes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB" crossorigin="anonymous" />
    <link href="styles/StyleDetalhes.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
       <div class="container">
        <div class="Info">
            <h2 class="text titulo" id="lblNome" runat="server"></h2>

            <img id="imgProduto" runat="server"/>

            <h2  class="text preco" id="lblPreco" runat="server"></h2>

            <p class="text descricao" id="lblDescricao" runat="server"></p>

            
        </div>
        <asp:Button class="buttonAdicionar" ID="btnAdicionarAoCarrinho" runat="server" Text="Adicionar ao Carrinho" OnClick="btnAdicionarAoCarrinho_Click" />
        <asp:Button class="buttonVoltar" ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
        </div>
           </form>
</body>
</html>
