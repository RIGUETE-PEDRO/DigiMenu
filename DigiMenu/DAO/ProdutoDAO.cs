using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;
using DigiMenu.DAL;
using System.Threading;

namespace DigiMenu.DAO
{
    public class ProdutoDAO
    {
        public List<Produto> Listar()
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Produto.ToList();
            }
        }

        // Ajustado: localizar a imagem do produto no banco e retornar o caminho relativo
        public string Delete(int idProduto)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var produto = ctx.Produto.Find(idProduto);
                if (produto == null)
                    return null;

                // Busca imagem associada
                var imagem = ctx.ImagemProduto.FirstOrDefault(i => i.ProdutoId == idProduto);
                string caminhoImagem = imagem?.CaminhoImagem;

                // Remove imagem (se houver)
                if (imagem != null)
                {
                    ctx.ImagemProduto.Remove(imagem);
                }

                // Remove produto
                ctx.Produto.Remove(produto);
                ctx.SaveChanges();
                return caminhoImagem;
            }
        }

        public Produto BuscarPorId(int idProduto)
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Produto.Find(idProduto);
            }
        }

        public void Atualizar(Produto produto, ImagemProduto imagemProduto)
        {
            using (var ctx = new DigiMenuEntities())
            {
                const string IMAGEM_PADRAO = "imgProduto/sem-imagem.png";
                var existente = ctx.Produto.Find(produto.IdProduto);
                if (existente == null) return;

                // Atualiza campos básicos
                existente.Nome = produto.Nome;
                existente.Descricao = produto.Descricao;
                existente.Preco = produto.Preco;
                existente.Estoque = produto.Estoque;
                existente.Ativo = produto.Ativo;

                // Atualiza imagem, se fornecida
                var imagemExistente = ctx.ImagemProduto.FirstOrDefault(i => i.ProdutoId == existente.IdProduto);

                if (imagemProduto != null)
                {
                    // Se não há registro de imagem ainda, cria um
                    if (imagemExistente == null)
                    {
                        imagemExistente = new ImagemProduto
                        {
                            ProdutoId = existente.IdProduto,
                            CaminhoImagem = string.IsNullOrWhiteSpace(imagemProduto.CaminhoImagem)
                                ? IMAGEM_PADRAO
                                : imagemProduto.CaminhoImagem
                        };
                        ctx.ImagemProduto.Add(imagemExistente);
                    }
                    else
                    {
                        // Atualiza caminho se vier preenchido; se vier vazio, aplica padrão
                        if (imagemProduto.CaminhoImagem != null)
                        {
                            imagemExistente.CaminhoImagem = string.IsNullOrWhiteSpace(imagemProduto.CaminhoImagem)
                                ? IMAGEM_PADRAO
                                : imagemProduto.CaminhoImagem;
                        }
                    }
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

        internal void Salvar(Produto produto, ImagemProduto imagemProduto)
        {
            using (var ctx = new DigiMenuEntities())
            {
                const string IMAGEM_PADRAO = "imgProduto/sem-imagem.png";

                // Adiciona produto primeiro para obter o Id
                ctx.Produto.Add(produto);
                ctx.SaveChanges();

                if (imagemProduto == null)
                {
                    imagemProduto = new ImagemProduto
                    {
                        ProdutoId = produto.IdProduto,
                        CaminhoImagem = IMAGEM_PADRAO
                    };
                }
                else
                {
                    imagemProduto.ProdutoId = produto.IdProduto;
                    if (string.IsNullOrWhiteSpace(imagemProduto.CaminhoImagem))
                    {
                        imagemProduto.CaminhoImagem = IMAGEM_PADRAO;
                    }
                }

                ctx.ImagemProduto.Add(imagemProduto);
                ctx.SaveChanges();
            }
        }
    }
}
