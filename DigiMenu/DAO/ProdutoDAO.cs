using System.Collections.Generic;
using System.Linq;

namespace DigiMenu.DAO
{
    public class ProdutoDAO
    {
        private DigiMenuEntities ctx = new DigiMenuEntities();

        public void Salvar(Produto produto)
        {
            ctx.Produto.Add(produto);
            ctx.SaveChanges();
        }

        public List<Produto> Listar()
        {
            return ctx.Produto.ToList();
        }
    }
}
