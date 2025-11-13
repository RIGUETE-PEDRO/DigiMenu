using System.Collections.Generic;
using System.Linq;
using DigiMenu.DTO;

namespace DigiMenu.DAO
{
    public class ImagemProdutoQuery
    {
        // Retorna lista de DTOs juntando imagem + produto
        public List<ImagemProdutoDTO> ListarTodos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                var query = from img in ctx.ImagemProduto
                            join prod in ctx.Produto on img.ProdutoId equals prod.IdProduto
                            select new ImagemProdutoDTO
                            {
                                IdImagemProduto = img.IdImagemProduto,
                                UrlImagem = img.CaminhoImagem,
                                ProdutoId = prod.IdProduto,
                                NomeProduto = prod.Nome,
                                Descricao = prod.Descricao,
                                PrecoProduto = prod.Preco.ToString(),
                                Ativo = prod.Ativo
                            };
                return query.ToList();
            }
        }

        // Busca por Id da Imagem
        public ImagemProdutoDTO BuscarPorImagemId(int idImagem)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var dto = (from img in ctx.ImagemProduto
                           join prod in ctx.Produto on img.ProdutoId equals prod.IdProduto
                           where img.IdImagemProduto == idImagem
                           select new ImagemProdutoDTO
                           {
                               IdImagemProduto = img.IdImagemProduto,
                               UrlImagem = img.CaminhoImagem,
                               ProdutoId = prod.IdProduto,
                               NomeProduto = prod.Nome,
                               Descricao = prod.Descricao,
                               PrecoProduto = prod.Preco.ToString(),
                               Ativo = prod.Ativo
                           }).FirstOrDefault();
                return dto;
            }
        }

        // Busca pela chave do produto
        public ImagemProdutoDTO BuscarPorProdutoId(int produtoId)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var dto = (from img in ctx.ImagemProduto
                           join prod in ctx.Produto on img.ProdutoId equals prod.IdProduto
                           where prod.IdProduto == produtoId
                           select new ImagemProdutoDTO
                           {
                               IdImagemProduto = img.IdImagemProduto,
                               UrlImagem = img.CaminhoImagem,
                               ProdutoId = prod.IdProduto,
                               NomeProduto = prod.Nome,
                               Descricao = prod.Descricao,
                               PrecoProduto = prod.Preco.ToString(),
                               Ativo = prod.Ativo
                           }).FirstOrDefault();
                return dto;
			}
        }

        // Lista somente produtos ativos (ex.: para vitrine)
        public List<ImagemProdutoDTO> ListarAtivos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                var query = from img in ctx.ImagemProduto
                            join prod in ctx.Produto on img.ProdutoId equals prod.IdProduto
                            where prod.Ativo
                            select new ImagemProdutoDTO
                            {
                                IdImagemProduto = img.IdImagemProduto,
                                UrlImagem = img.CaminhoImagem,
                                ProdutoId = prod.IdProduto,
                                NomeProduto = prod.Nome,
                                Descricao = prod.Descricao,
                                PrecoProduto = prod.Preco.ToString(),
                                Ativo = prod.Ativo
                            };
                return query.ToList();
            }
        }
    }
}
