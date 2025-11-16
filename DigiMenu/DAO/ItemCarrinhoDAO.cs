using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DigiMenu.DAL
{
    public class ItemCarrinhoDAO
    {
        protected readonly DigiMenuEntities Context;
        private readonly Control _control;

        public ItemCarrinhoDAO(DigiMenuEntities context = null, Control control = null)
        {
            Context = context ?? new DigiMenuEntities();
            _control = control;
        }

        // Finaliza o carrinho e retorna o Id do Pedido criado (0 em erro)
        // Agora aceita opcionalmente o idEndereco para vincular cada ItemPedido ao Endereco criado
        public int FinalizarCarrinho(int usuarioId, int idEndereco = 0)
        {
            int pedidoId = 0;

            using (var ctx = new DigiMenuEntities())
            {
                var carrinho = ctx.Carrinho.FirstOrDefault(c => c.UsuarioId == usuarioId);
                if (carrinho == null || !carrinho.ItemCarrinho.Any())
                {
                    return 0;
                }

                // valida estoque antes de finalizar
                foreach (var it in carrinho.ItemCarrinho)
                {
                    int estoque = it.Produto.Estoque;
                    if ((it.Quantidade ?? 0) > estoque)
                    {
                        var ph = _control?.FindControl("phMensagens") as PlaceHolder;
                        if (ph != null)
                        {
                            var msg = new Mensagens().MostrarMensagem($"Quantidade de '{it.Produto.Nome}' excede o estoque disponível.", "erro");
                            ph.Controls.Add(msg);
                        }
                        CarregarCarrinho();
                        return 0;
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
                        PrecoUnitario = item.Produto.Preco,
                        IdEndereco = idEndereco // associa o endereco existente (0 se não fornecido)
                    };

                    pedido.ItemPedido.Add(itemPedido);
                    pedido.Total += (item.Quantidade ?? 1) * item.Produto.Preco;

                    // baixa estoque
                    item.Produto.Estoque -= (item.Quantidade ?? 1);
                }

                ctx.Pedido.Add(pedido);
                ctx.ItemCarrinho.RemoveRange(carrinho.ItemCarrinho);
                ctx.SaveChanges(); // gera Ids de Pedido e ItemPedido

                pedidoId = pedido.IdPedido;

                return pedidoId; // usado para associar endereco posteriormente
            }
        }

        public void AdicionarProduto(int produtoId)
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0)
            {
                _control?.Page?.Response.Redirect("FrmLogin.aspx");
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
                    var ph = _control?.FindControl("phMensagens") as PlaceHolder;
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

        // Método corrigido para processar ações em um item do carrinho
        public bool ProcessarItemCarrinho(int idItemCarrinho, int usuarioId, string commandName, int? novaQuantidade = null)
        {
            if (usuarioId <= 0) return false;
            using (var ctx = new DigiMenuEntities())
            {
                var item = ctx.ItemCarrinho.FirstOrDefault(x => x.IdItemCarrinho == idItemCarrinho && x.Carrinho.UsuarioId == usuarioId);
                if (item == null) return false;

                if (commandName == "Remover")
                {
                    ctx.ItemCarrinho.Remove(item);
                    ctx.SaveChanges();
                    return true;
                }
                if (commandName == "Atualizar")
                {
                    if (!novaQuantidade.HasValue || novaQuantidade.Value <= 0) return false;
                    int estoque = item.Produto.Estoque;
                    if (novaQuantidade.Value > estoque)
                    {
                        var ph = _control?.FindControl("phMensagens") as PlaceHolder;
                        if (ph != null)
                        {
                            var msg = new Mensagens().MostrarMensagem($"Quantidade de '{item.Produto.Nome}' excede o estoque disponível (máximo {estoque}).", "erro");
                            ph.Controls.Add(msg);
                        }
                        return false;
                    }
                    item.Quantidade = novaQuantidade.Value;
                    item.PrecoTotal = novaQuantidade.Value * item.Produto.Preco;
                    ctx.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public void CarregarCarrinho()
        {
            int usuarioId = ObterUsuarioId();
            if (usuarioId == 0)
            {
                _control?.Page?.Response.Redirect("FrmLogin.aspx");
                return;
            }

            var pnlCarrinho = _control?.FindControl("pnlCarrinho") as Panel;
            var pnlCarrinhoVazio = _control?.FindControl("pnlCarrinhoVazio") as Panel;
            var rptCarrinho = _control?.FindControl("rptCarrinho") as Repeater;
            var lblTotal = _control?.FindControl("lblTotal") as Label;

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

        public int ObterUsuarioId(object sessionValor)
        {
            return (int)(sessionValor ?? 0);
        }

        // Overload para ler automaticamente da sessão quando possível
        public int ObterUsuarioId()
        {
            var sessionValor = _control?.Page?.Session != null ? _control.Page.Session["UsuarioId"] : null;
            return (int)(sessionValor ?? 0);
        }

        public void AdicionarOuIncrementar(int carrinhoId, int produtoId, int quantidade)
        {
            var item = Context.ItemCarrinho.FirstOrDefault(i => i.CarrinhoId == carrinhoId && i.ProdutoId == produtoId);
            var produto = Context.Produto.FirstOrDefault(p => p.IdProduto == produtoId);
            if (produto == null) return;

            if (item == null)
            {
                item = new ItemCarrinho
                {
                    CarrinhoId = carrinhoId,
                    ProdutoId = produtoId,
                    Quantidade = quantidade,
                    PrecoTotal = quantidade * produto.Preco
                };
                Context.ItemCarrinho.Add(item);
            }
            else
            {
                int q = (item.Quantidade ?? 0) + quantidade;
                item.Quantidade = q;
                item.PrecoTotal = q * produto.Preco;
            }
            Context.SaveChanges();
        }

        
    }
}
