using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DigiMenu.DAO;

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
            }
        }

        protected void btnFiltrarPreco_Click(object sender, EventArgs e)
        {
            AplicarFiltroFaixaPreco();
        }

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

        private void AplicarFiltroQueryString()
        {
            var dao = new ProdutoDAO();
            string catIdStr = Request.QueryString["catId"];
            if (int.TryParse(catIdStr, out int catId) && catId > 0)
            {
                BindProdutos(dao.ListarAtivosPorCategoriaId(catId));
                return;
            }
            string cat = Request.QueryString["cat"]; // nome
            if (!string.IsNullOrWhiteSpace(cat))
            {
                BindProdutos(dao.ListarAtivosPorCategoriaNome(cat));
                return;
            }
            BindProdutos(dao.ListarAtivos());
        }

        private void BindProdutos(System.Collections.IEnumerable lista)
        {
            rptProdutos.DataSource = lista;
            rptProdutos.DataBind();
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            string termo = txtPesquisa.Text.Trim();
            var dao = new ProdutoDAO();
            var baseAtivos = dao.ListarAtivos();
            var filtrados = baseAtivos.Where(p => (p.Nome ?? "").IndexOf(termo, System.StringComparison.OrdinalIgnoreCase) >= 0 || (p.Descricao ?? "").IndexOf(termo, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            BindProdutos(filtrados);
        }

        protected void Filtro_CheckedChanged(object sender, EventArgs e)
        {
            // Mantido se futuramente adicionar outros filtros (categoria etc.)
        }

        

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

        protected void compra_Click(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }
            var btn = sender as Button;
            if (btn == null) return;
            if (!int.TryParse(btn.CommandArgument, out int produtoId)) return;
            int usuarioId = (int)Session["UsuarioId"];

            using (var ctx = new DigiMenuEntities())
            {
                var carrinho = ctx.Carrinho.FirstOrDefault(c => c.UsuarioId == usuarioId);
                if (carrinho == null)
                {
                    carrinho = new Carrinho { UsuarioId = usuarioId, DataCriacao = DateTime.Now };
                    ctx.Carrinho.Add(carrinho);
                    ctx.SaveChanges();
                }
                var item = ctx.ItemCarrinho.FirstOrDefault(i => i.CarrinhoId == carrinho.IdCarrinho && i.ProdutoId == produtoId);
                var produto = ctx.Produto.FirstOrDefault(p => p.IdProduto == produtoId);
                if (produto == null) return;
                if (item == null)
                {
                    item = new ItemCarrinho { CarrinhoId = carrinho.IdCarrinho, ProdutoId = produtoId, Quantidade = 1, PrecoTotal = produto.Preco };
                    ctx.ItemCarrinho.Add(item);
                }
                else
                {
                    int q = (item.Quantidade ?? 0) + 1;
                    item.Quantidade = q;
                    item.PrecoTotal = q * produto.Preco;
                }
                ctx.SaveChanges();
            }
            new LogDAO().Registrar(usuarioId, 6);
            Response.Redirect("carrinho.aspx");
        }

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
    }
}