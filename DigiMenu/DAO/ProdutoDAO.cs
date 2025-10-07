using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; // para StringBuilder
using System.Data.Entity.Validation; // para capturar erros de validação


namespace DigiMenu.DAO
{
    public class ProdutoDAO
    {
        private DigiMenuEntities ctx = new DigiMenuEntities();
        private const string IMAGEM_PADRAO = "imgProduto/sem-imagem.png"; // certifique-se de criar este arquivo

        public void Salvar(Produto produto)
        {
            if (string.IsNullOrWhiteSpace(produto.imagem))
            {
                produto.imagem = IMAGEM_PADRAO; // garante consistência na criação
            }
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

            // Regras imagem:
            // null => não altera (mantém a existente)
            // string.Empty => remover -> usar imagem padrão
            // valor não vazio => substituir
            if (produto.imagem != null)
            {
                if (produto.imagem == string.Empty)
                {
                    existente.imagem = IMAGEM_PADRAO; // substitui por default
                }
                else if (!string.IsNullOrWhiteSpace(produto.imagem))
                {
                    existente.imagem = produto.imagem;
                }
            }

            // Se por qualquer motivo ficar vazio, reforça padrão
            if (string.IsNullOrWhiteSpace(existente.imagem))
            {
                existente.imagem = IMAGEM_PADRAO;
            }

            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    sb.AppendLine($"Entidade: {eve.Entry.Entity.GetType().Name} Estado: {eve.Entry.State}");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine($" - Propriedade: {ve.PropertyName} Erro: {ve.ErrorMessage}");
                    }
                }
                throw new Exception("Erro de validação ao atualizar produto: " + sb.ToString(), ex);
            }
        }
    }
}
