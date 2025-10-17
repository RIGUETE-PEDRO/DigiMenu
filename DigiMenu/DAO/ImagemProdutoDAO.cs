using System;
using System.Linq;

namespace DigiMenu.DAL
{
    public class ImagemProdutoDAO
    {
        private const string IMAGEM_PADRAO = "imgProduto/sem-imagem.png";

        internal void SalvarImagem(ImagemProduto imagemProduto)
        {
            using (var ctx = new DigiMenuEntities())
            {
                if (string.IsNullOrWhiteSpace(imagemProduto.CaminhoImagem))
                {
                    imagemProduto.CaminhoImagem = IMAGEM_PADRAO;
                }
                ctx.ImagemProduto.Add(imagemProduto);
                ctx.SaveChanges();
            }
        }

        internal ImagemProduto BuscarImagemPorProdutoId(int idProduto)
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.ImagemProduto.FirstOrDefault(i => i.ProdutoId == idProduto);
            }
        }
    }
}
