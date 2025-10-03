using System;
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

        // Retorna o caminho (relativo) da imagem para permitir exclusão física fora da camada de dados
        public string Delete(int idProduto)
        {
            var produto = ctx.Produto.Find(idProduto);
            if (produto == null)
                return null;

            string imagem = produto.imagem; // ex: "imgProduto/abc123.jpg"
            ctx.Produto.Remove(produto);
            ctx.SaveChanges();
            return imagem;
        }

        public Produto BuscarPorId(int idProduto)
        {
            return ctx.Produto.Find(idProduto);
        }

        public void Atualizar(Produto produto)
        {
            var existente = ctx.Produto.Find(produto.IdProduto);
            if (existente == null) return;

            existente.Nome = produto.Nome;
            existente.Descricao = produto.Descricao;
            existente.Preco = produto.Preco;
            existente.Estoque = produto.Estoque;
            existente.Ativo = produto.Ativo;
            // Só altera imagem se vier uma nova (não nula / não vazia)
            if (!string.IsNullOrWhiteSpace(produto.imagem))
            {
                existente.imagem = produto.imagem;
            }
            ctx.SaveChanges();
        }
    }
}
