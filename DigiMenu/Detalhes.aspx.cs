using DigiMenu.DAO;
using DigiMenu.DAL; // para ImagemProdutoDAO
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu
{
    public partial class Detalhes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int produtoId;
            if (!int.TryParse(Request.QueryString["produtoId"], out produtoId) || produtoId <= 0)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            var produtoDAO = new ProdutoDAO();
            var produto = produtoDAO.BuscarPorId(produtoId);
            if (produto == null)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            lblNome.InnerText = produto.Nome;
            lblPreco.InnerText = "R$ " + produto.Preco.ToString("F2");
            lblDescricao.InnerText = produto.Descricao;
            Title = produto.Nome + " - DigiMenu";

            // Busca imagem (Produto não tem propriedade Imagem)
            var imagemDAO = new ImagemProdutoDAO();
            var img = imagemDAO.BuscarImagemPorProdutoId(produtoId);
            var caminho = (img != null && !string.IsNullOrWhiteSpace(img.CaminhoImagem)) ? img.CaminhoImagem : "imgProduto/sem-imagem.png";
            imgProduto.Src = ResolveUrl("~/" + caminho.TrimStart('~', '/'));
        }

        protected void btnAdicionarAoCarrinho_Click(object sender, EventArgs e)
        {
            ProdutoDAO produtoDAO = new ProdutoDAO();
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }
            int produtoId;
            if (!int.TryParse(Request.QueryString["produtoId"], out produtoId)) return;
            int usuarioId = (int)Session["UsuarioId"];

           produtoDAO.AdicionarProdutoAoCarrinho(usuarioId, produtoId);
            Response.Redirect("carrinho.aspx");
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}