using DigiMenu.DAL;
using DigiMenu.DAO;
using DigiMenu.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity; // Include

namespace DigiMenu
{
    public partial class Default : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (rptProdutos != null)
            {
                rptProdutos.ItemDataBound += rptProdutos_ItemDataBound;
            }
        }

        private void RestoreSessionFromCookie()
        {
            if (Session["UsuarioId"] != null) return;
            var cookie = Request.Cookies["DigiMenuUser"];
            if (cookie == null) return;
            int id;
            int tipo;
            if (int.TryParse(cookie.Values["Id"], out id))
                Session["UsuarioId"] = id;
            var nome = cookie.Values["Nome"];
            if (!string.IsNullOrEmpty(nome))
                Session["UsuarioNome"] = nome;
            if (int.TryParse(cookie.Values["Tipo"], out tipo))
                Session["TipoUsuarioId"] = tipo;
        }

        private void rptProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var dataItem = e.Item.DataItem;
            if (dataItem == null) return;
            var prop = dataItem.GetType().GetProperty("IdProduto");
            if (prop == null) return;
            var id = prop.GetValue(dataItem, null);
            var btn = e.Item.FindControl("compra") as Button;
            if (btn != null && id != null)
            {
                btn.CommandArgument = id.ToString();
                btn.PostBackUrl = "carrinho.aspx?add=" + id;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RestoreSessionFromCookie();
            if (!IsPostBack)
            {
                AjustarLoginHeader();
                AplicarFiltroQueryString();

                // Carrega dados do carrossel
                var listaDados = BuscaDadosInterisos();

                // Indicadores (usam apenas o índice)
                rptIndicators.DataSource = listaDados;
                rptIndicators.DataBind();

                // Slides usam o DTO completo (UrlImagem, NomeProduto, Descricao, PrecoProduto)
                rptCarousel.DataSource = listaDados;
                rptCarousel.DataBind();
            }
        }

        private List<ImagemProdutoDTO> BuscaDadosInterisos()
        {
            using (var context = new DigiMenuEntities())
            {
                // Busca imagens vinculadas a carrosséis ativos e ordena pela ordem configurada
                var dadosQuery = context.ImagemProduto
                    .Include("Produto")
                    .Include("Carousel")
                    .Where(i => i.Carousel.Ativo)
                    .OrderBy(i => i.Carousel.Ordem)
                    .Select(i => new
                    {
                        i.IdImagemProduto,
                        i.CaminhoImagem,
                        ProdutoId = i.Produto.IdProduto,
                        NomeProduto = i.Produto.Nome,
                        Descricao = i.Produto.Descricao,
                        PrecoProduto = i.Produto.Preco, // decimal
                        OrdemCarousel = i.Carousel.Ordem
                    })
                    .ToList();

                // Converte para DTO, formata preço e garante caminho relativo à raiz
                var dados = dadosQuery.Select(i => new ImagemProdutoDTO
                {
                    IdImagemProduto = i.IdImagemProduto,
                    UrlImagem = ResolveUrl("~/" + (i.CaminhoImagem ?? string.Empty).TrimStart('~', '/')),
                    ProdutoId = i.ProdutoId,
                    NomeProduto = i.NomeProduto,
                    Descricao = i.Descricao,
                    PrecoProduto = i.PrecoProduto.ToString("F2"),
                    OrdemCarousel = i.OrdemCarousel
                }).ToList();

                return dados;
            }
        }

        protected void btnFiltrarPreco_Click(object sender, EventArgs e)
        {
            AplicarFiltroFaixaPreco();
        }

        //corta o nome para exibir no header
        private void AjustarLoginHeader()
        {
            var nomeCompleto = Session["UsuarioNome"] as string;
            if (!string.IsNullOrEmpty(nomeCompleto))
            {
                string primeiroNome = nomeCompleto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (divLogin != null) divLogin.Visible = false;
                if (divUser != null)
                {
                    divUser.Visible = true;
                    if (lblUserName != null) lblUserName.InnerText = "Olá, " + primeiroNome + "!";
                }
            }
            else
            {
                if (divLogin != null) divLogin.Visible = true;
                if (divUser != null) divUser.Visible = false;
            }
        }
        //aplica filtro pela query string
        private void AplicarFiltroQueryString()
        {
            var dao = new ProdutoDAO();
            string cat = Request.QueryString["cat"]; // nome
            if (!string.IsNullOrWhiteSpace(cat))
            {
                BindProdutos(dao.ListarAtivosPorCategoriaNome(cat));
                carrousel.Style.Add("display", "none");
                return;
            }
            BindProdutos(dao.ListarAtivos());
        }

        //vincula a lista de produtos ao repeater
        private void BindProdutos(System.Collections.IEnumerable lista)
        {
            rptProdutos.DataSource = lista;
            rptProdutos.DataBind();
        }

        //pesquisa produtos pelo termo
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            string termo = txtPesquisa.Text.Trim();
            var dao = new ProdutoDAO();
            var baseAtivos = dao.ListarAtivos();
            var filtrados = baseAtivos.Where(p => (p.Nome ?? "").IndexOf(termo, System.StringComparison.OrdinalIgnoreCase) >= 0 || (p.Descricao ?? "").IndexOf(termo, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            BindProdutos(filtrados);
        }


        //aplica filtro por faixa de preço 
        private void AplicarFiltroFaixaPreco()
        {
            var dao = new ProdutoDAO();

            var tbMin = FindControl("txtPrecoMin") as TextBox;
            var tbMax = FindControl("txtPrecoMax") as TextBox;
            string minTxt = tbMin != null ? tbMin.Text : null;
            string maxTxt = tbMax != null ? tbMax.Text : null;

            decimal? precoMin = TryParsePreco(minTxt);
            decimal? precoMax = TryParsePreco(maxTxt);

            if (!precoMin.HasValue && !precoMax.HasValue)
            {
                BindProdutos(dao.ListarAtivos());
                return;
            }

            var filtrados = dao.ListarAtivosPorFaixaPreco(precoMin, precoMax);
            BindProdutos(filtrados);
        }

        // tenta converter texto em decimal, retornando null se falhar
        private decimal? TryParsePreco(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return null;
            texto = texto.Replace("R$", "").Trim();
            // aceita tanto vírgula quanto ponto como separador decimal
            texto = texto.Replace('.', ',');
            decimal valor;
            if (decimal.TryParse(texto, out valor))
                return valor;
            return null;
        }




        //logout do usuário
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                var cookie = new HttpCookie(".ASPXAUTH") { Expires = DateTime.Now.AddDays(-1d) };
                Response.Cookies.Add(cookie);
            }
            Response.Redirect("FrmLogin.aspx");
        }




        //redireciona para a página de detalhes do produto
        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument.ToString(), out int idProduto))
            {
                Response.Redirect("Detalhes.aspx?produtoId=" + idProduto);
            }
        }
    }
}