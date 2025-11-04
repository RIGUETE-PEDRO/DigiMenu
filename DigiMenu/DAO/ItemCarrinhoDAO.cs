using System;
using System.Linq;

namespace DigiMenu.DAL
{
    public class ItemCarrinhoDAO
    {
        protected readonly DigiMenuEntities Context;
        public ItemCarrinhoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
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
