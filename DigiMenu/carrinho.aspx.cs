using DigiMenu.DAL;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu
{
    public partial class carrinho : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Suporte a adicionar via querystring (?add=ID)
                int addId;
                if (int.TryParse(Request.QueryString["add"], out addId))
                {
                    AdicionarProduto(addId);
                }

                var rpt = FindControl("rptCarrinho") as Repeater;
                if (rpt != null)
                {
                    rpt.ItemDataBound += rptCarrinho_ItemDataBound;
                }

                CarregarCarrinho();
            }
        }

        private int ObterUsuarioId()
        {
            return (int)(Session["UsuarioId"] ?? 0);
        }

        private void AdicionarProduto(int produtoId)
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

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

                if (produto.Estoque <= 0)
                {
                    // mensagem de sem estoque
                    var ph = FindControl("phMensagens") as PlaceHolder;
                    if (ph != null)
                    {
                        var msg = new Mensagens().MostrarMensagem("Produto sem estoque.", "alerta");
                        ph.Controls.Add(msg);
                    }
                    return;
                }

                if (item == null)
                {
                    int qtd = Math.Min(1, produto.Estoque);
                    item = new ItemCarrinho
                    {
                        CarrinhoId = carrinho.IdCarrinho,
                        ProdutoId = produtoId,
                        Quantidade = qtd,
                        PrecoTotal = qtd * produto.Preco
                    };
                    ctx.ItemCarrinho.Add(item);
                }
                else
                {
                    int novaQtd = Math.Min((item.Quantidade ?? 0) + 1, produto.Estoque);
                    item.Quantidade = novaQtd;
                    item.PrecoTotal = novaQtd * produto.Preco;
                }

                ctx.SaveChanges();
            }
        }

        private void CarregarCarrinho()
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            var pnlCarrinho = FindControl("pnlCarrinho") as Panel;
            var pnlCarrinhoVazio = FindControl("pnlCarrinhoVazio") as Panel;
            var rptCarrinho = FindControl("rptCarrinho") as Repeater;
            var lblTotal = FindControl("lblTotal") as Label;

            using (var ctx = new DigiMenuEntities())
            {
                var carrinho = ctx.Carrinho.FirstOrDefault(c => c.UsuarioId == usuarioId);
                if (carrinho == null || !carrinho.ItemCarrinho.Any())
                {
                    if (pnlCarrinho != null) pnlCarrinho.Visible = false;
                    if (pnlCarrinhoVazio != null) pnlCarrinhoVazio.Visible = true;
                    return;
                }

                var itens = carrinho.ItemCarrinho.Select(i => new
                {
                    i.IdItemCarrinho,
                    i.Quantidade,
                    PrecoUnitario = i.Produto.Preco,
                    i.PrecoTotal,
                    Nome = i.Produto.Nome,
                    Imagem = i.Produto.ImagemProduto.FirstOrDefault().CaminhoImagem,
                    Estoque = i.Produto.Estoque
                }).ToList();

                if (rptCarrinho != null)
                {
                    rptCarrinho.DataSource = itens;
                    rptCarrinho.DataBind();
                }

                decimal total = itens.Sum(x => (decimal)(x.PrecoTotal ?? x.PrecoUnitario * (x.Quantidade ?? 0)));
                if (lblTotal != null) lblTotal.Text = $"R$ {total:N2}";

                if (pnlCarrinho != null) pnlCarrinho.Visible = true;
                if (pnlCarrinhoVazio != null) pnlCarrinhoVazio.Visible = false;
            }
        }

        private void rptCarrinho_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var txtQtd = e.Item.FindControl("txtQuantidade") as TextBox;
            if (txtQtd != null)
            {
                // aplica min/max baseado no estoque
                var dataItem = e.Item.DataItem;
                var propEstoque = dataItem.GetType().GetProperty("Estoque");
                int estoque = propEstoque != null ? (int)propEstoque.GetValue(dataItem, null) : int.MaxValue;
                txtQtd.Attributes["type"] = "number";
                txtQtd.Attributes["min"] = "1";
                txtQtd.Attributes["max"] = estoque.ToString();
            }
        }

        protected void rptCarrinho_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            int idItem;
            if (!int.TryParse(e.CommandArgument.ToString(), out idItem)) return;

            using (var ctx = new DigiMenuEntities())
            {
                var item = ctx.ItemCarrinho.FirstOrDefault(x => x.IdItemCarrinho == idItem && x.Carrinho.UsuarioId == usuarioId);
                if (item == null) return;

                if (e.CommandName == "Remover")
                {
                    ctx.ItemCarrinho.Remove(item);
                    ctx.SaveChanges();
                }
                else if (e.CommandName == "Atualizar")
                {
                    var txtQtd = e.Item.FindControl("txtQuantidade") as TextBox;
                    int qtd;
                    if (txtQtd != null && int.TryParse(txtQtd.Text, out qtd) && qtd > 0)
                    {
                        // respeitar estoque
                        int estoque = item.Produto.Estoque;
                        if (qtd > estoque)
                        {
                            var ph = FindControl("phMensagens") as PlaceHolder;
                            if (ph != null)
                            {
                                var msg = new Mensagens().MostrarMensagem($"Quantidade de '{item.Produto.Nome}' excede o estoque disponível (máximo {estoque}).", "erro");
                                ph.Controls.Add(msg);
                            }
                            // não salva alteração
                        }
                        else
                        {
                            item.Quantidade = qtd;
                            item.PrecoTotal = qtd * item.Produto.Preco;
                            ctx.SaveChanges();
                        }
                    }
                }
            }

            CarregarCarrinho();
        }

        protected void btnContinuarComprando_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            using (var ctx = new DigiMenuEntities())
            {
                var carrinho = ctx.Carrinho.FirstOrDefault(c => c.UsuarioId == usuarioId);
                if (carrinho == null || !carrinho.ItemCarrinho.Any())
                {
                    return;
                }

                // valida estoque antes de finalizar
                foreach (var it in carrinho.ItemCarrinho)
                {
                    int estoque = it.Produto.Estoque;
                    if ((it.Quantidade ?? 0) > estoque)
                    {
                        var ph = FindControl("phMensagens") as PlaceHolder;
                        if (ph != null)
                        {
                            var msg = new Mensagens().MostrarMensagem($"Quantidade de '{it.Produto.Nome}' excede o estoque disponível.", "erro");
                            ph.Controls.Add(msg);
                        }
                        CarregarCarrinho();
                        return;
                    }
                }

                var pedido = new Pedido
                {
                    Data = DateTime.Now,
                    UsuarioId = usuarioId,
                    StatusId = 1,
                    Total = 0
                };

                foreach (var item in carrinho.ItemCarrinho.ToList())
                {
                    var itemPedido = new ItemPedido
                    {
                        ProdutoId = item.ProdutoId,
                        Quantidade = item.Quantidade ?? 1,
                        PrecoUnitario = item.Produto.Preco
                    };
                    pedido.ItemPedido.Add(itemPedido);
                    pedido.Total += (item.Quantidade ?? 1) * item.Produto.Preco;

                    // baixa estoque
                    item.Produto.Estoque -= (item.Quantidade ?? 1);
                }

                ctx.Pedido.Add(pedido);
                ctx.ItemCarrinho.RemoveRange(carrinho.ItemCarrinho);
                ctx.SaveChanges();
            }

            Response.Redirect("StatusPedido.aspx");
        }
    }
}