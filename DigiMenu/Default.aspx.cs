using DigiMenu.DAL;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        private void rptProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var dataItem = e.Item.DataItem;
            if (dataItem == null) return;

            var prop = dataItem.GetType().GetProperty("IdProduto");
            if (prop == null) return;

            var id = prop.GetValue(dataItem, null);
            var btn = e.Item.FindControl("compra") as Button;
            if (btn != null && id != null)
            {
                var idStr = id.ToString();
                btn.CommandArgument = idStr;
                btn.PostBackUrl = "carrinho.aspx?add=" + idStr;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                CarregarProdutosAtivos();
                iniciarFiltro();
            }
        }

        private void iniciarFiltro()
        {
            ddlCategoria.Visible = rbCategoria.Checked;
            txtPreco.Visible = false;
            ddlOferta.Visible = false;
        }

        private void CarregarProdutosAtivos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                var produtosAtivos = ctx.Produto
                                        .Where(p => p.Ativo)
                                        .Select(p => new
                                        {
                                            p.IdProduto,
                                            p.Nome,
                                            p.Descricao,
                                            p.Preco,
                                            p.Estoque,
                                            Imagem = p.ImagemProduto.Select(img => img.CaminhoImagem).FirstOrDefault()
                                        })
                                        .ToList();

                rptProdutos.DataSource = produtosAtivos;
                rptProdutos.DataBind();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            string termoPesquisa = txtPesquisa.Text.Trim();

            using (var ctx = new DigiMenuEntities())
            {
                var resultados = ctx.Produto
                                    .Where(p => p.Ativo && (p.Nome.Contains(termoPesquisa) || p.Descricao.Contains(termoPesquisa)))
                                    .Select(p => new
                                    {
                                        p.IdProduto,
                                        p.Nome,
                                        p.Descricao,
                                        p.Preco,
                                        p.Estoque,
                                        Imagem = p.ImagemProduto.Select(img => img.CaminhoImagem).FirstOrDefault()
                                    })
                                    .ToList();
                rptProdutos.DataSource = resultados;
                rptProdutos.DataBind();
            }
        }

        protected void Filtro_CheckedChanged(object sender, EventArgs e)
        {
            ddlCategoria.Visible = rbCategoria.Checked;
            txtPreco.Visible = rbPreco.Checked;
     
        }

        protected void compra_Click(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            var btn = sender as System.Web.UI.WebControls.Button;
            if (btn == null) return;
            int produtoId;
            if (!int.TryParse(btn.CommandArgument, out produtoId)) return;

            int usuarioId = (int)Session["UsuarioId"];  

            using (var ctx = new DigiMenuEntities())
            {
                var carrinho = ctx.Carrinho.FirstOrDefault(c => c.UsuarioId == usuarioId);
                if (carrinho == null)
                {
                    carrinho = new Carrinho
                    {
                        UsuarioId = usuarioId,
                        DataCriacao = DateTime.Now
                    };
                    ctx.Carrinho.Add(carrinho);
                    ctx.SaveChanges();
                }

                var item = ctx.ItemCarrinho.FirstOrDefault(i => i.CarrinhoId == carrinho.IdCarrinho && i.ProdutoId == produtoId);
                var produto = ctx.Produto.FirstOrDefault(p => p.IdProduto == produtoId);
                if (produto == null) return;

                if (item == null)
                {
                    item = new ItemCarrinho
                    {
                        CarrinhoId = carrinho.IdCarrinho,
                        ProdutoId = produtoId,
                        Quantidade = 1,
                        PrecoTotal = produto.Preco
                    };
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

            LogDAO log = new LogDAO();
            log.Registrar(usuarioId, 6);

            Response.Redirect("carrinho.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            if (Request.Cookies[".ASPXAUTH"] != null)
            {
                var cookie = new HttpCookie(".ASPXAUTH");
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }

            Response.Redirect("FrmLogin.aspx");
        }

    }
}