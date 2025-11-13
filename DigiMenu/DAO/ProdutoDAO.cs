using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;
using DigiMenu.DAL;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DigiMenu.DAO
{
    public class ProdutoDAO
    {
        // DTO para exibição em listas (WebForms/MVC)
        public class ProdutoListaDTO
        {
            public int IdProduto { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public decimal Preco { get; set; }
            public int Estoque { get; set; }
            public string Imagem { get; set; }
        }

        // DTO para dados do carrossel por produto (usado pela camada de apresentação)
        public class ProdutoCarrosselDados
        {
            public int IdProduto { get; set; }
            public string Nome { get; set; }
            public bool Ativo { get; set; }
            public int? Ordem { get; set; }
        }

        // Normalização de nome de categoria (remove acentos, plural simples)
        private static string NormalizarCategoria(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            s = s.Trim().ToLowerInvariant();
            s = RemoverAcentos(s);
            if (s.EndsWith("s")) s = s.Substring(0, s.Length - 1); // plural simples
            return s;
        }

        private static string RemoverAcentos(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var ch in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        public List<Produto> Listar()
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Produto.ToList();
            }
        }

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
                existente.Categoria = produto.Categoria;

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
                    else if (imagemProduto.CaminhoImagem != null)
                    {
                        imagemExistente.CaminhoImagem = string.IsNullOrWhiteSpace(imagemProduto.CaminhoImagem)
                            ? IMAGEM_PADRAO
                            : imagemProduto.CaminhoImagem;
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
                    throw new Exception("Erro de validação ao atualizar produto: " + sb + string.Empty, ex);
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

                // Se produto marcado como ativo (oferta), criamos/garantimos um registro de Carousel para ele
                Carousel carouselRegistro = null;
                if (produto.Ativo)
                {
                    string chave = $"P:{produto.IdProduto}";
                    carouselRegistro = ctx.Carousel.FirstOrDefault(c => c.Nome == chave);
                    if (carouselRegistro == null)
                    {
                        carouselRegistro = new Carousel
                        {
                            Nome = chave,
                            Ativo = true,
                            Ordem = 0
                        };
                        ctx.Carousel.Add(carouselRegistro);
                        ctx.SaveChanges();
                    }
                }

                if (imagemProduto == null)
                {
                    imagemProduto = new ImagemProduto
                    {
                        ProdutoId = produto.IdProduto,
                        CaminhoImagem = IMAGEM_PADRAO,
                        Carousel = carouselRegistro // associa se existir
                    };
                }
                else
                {
                    imagemProduto.ProdutoId = produto.IdProduto;
                    if (string.IsNullOrWhiteSpace(imagemProduto.CaminhoImagem))
                    {
                        imagemProduto.CaminhoImagem = IMAGEM_PADRAO;
                    }
                    // associa ao carrossel se criado
                    if (carouselRegistro != null)
                    {
                        imagemProduto.Carousel = carouselRegistro;
                    }
                }

                ctx.ImagemProduto.Add(imagemProduto);
                ctx.SaveChanges();
            }
        }

        internal List<Produto> BuscarAtivos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Produto.Where(p => p.Ativo).ToList();
            }
        }

        // NOVO: lista produtos ativos (DTO)
        public List<ProdutoListaDTO> ListarAtivos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Produto
                    .Where(p => p.Ativo)
                    .Select(p => new ProdutoListaDTO
                    {
                        IdProduto = p.IdProduto,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Preco = p.Preco,
                        Estoque = p.Estoque,
                        Imagem = p.ImagemProduto.Select(img => img.CaminhoImagem).FirstOrDefault()
                    }).ToList();
            }
        }

        // NOVO: lista produtos ativos por id da categoria
        public List<ProdutoListaDTO> ListarAtivosPorCategoriaId(int categoriaId)
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Produto
                    .Where(p => p.Ativo && p.Categoria == categoriaId)
                    .Select(p => new ProdutoListaDTO
                    {
                        IdProduto = p.IdProduto,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Preco = p.Preco,
                        Estoque = p.Estoque,
                        Imagem = p.ImagemProduto.Select(img => img.CaminhoImagem).FirstOrDefault()
                    }).ToList();
            }
        }

        // NOVO: lista produtos ativos por nome de categoria (normalizado)
        public List<ProdutoListaDTO> ListarAtivosPorCategoriaNome(string categoriaNome)
        {
            string alvo = NormalizarCategoria(categoriaNome);
            using (var ctx = new DigiMenuEntities())
            {
                var categorias = ctx.Categoria
                    .Select(c => new { c.id, c.nome })
                    .ToList()
                    .Select(c => new { c.id, nomeNorm = NormalizarCategoria(c.nome) })
                    .ToList();

                var catMatch = categorias.FirstOrDefault(c => c.nomeNorm == alvo || c.nomeNorm.StartsWith(alvo) || alvo.StartsWith(c.nomeNorm));
                IQueryable<Produto> query;
                if (catMatch != null)
                {
                    int cid = catMatch.id;
                    query = ctx.Produto.Where(p => p.Ativo && p.Categoria == cid);
                }
                else
                {
                    // fallback por navegação
                    query = ctx.Produto.Where(p => p.Ativo && p.Categoria1 != null);
                }

                var listaBase = query
                    .Select(p => new { p, CatNome = p.Categoria1.nome })
                    .ToList();

                return listaBase
                    .Where(x =>
                    {
                        var nomeCatNorm = NormalizarCategoria(x.CatNome);
                        return nomeCatNorm == alvo || nomeCatNorm.StartsWith(alvo) || alvo.StartsWith(nomeCatNorm);
                    })
                    .Select(x => new ProdutoListaDTO
                    {
                        IdProduto = x.p.IdProduto,
                        Nome = x.p.Nome,
                        Descricao = x.p.Descricao,
                        Preco = x.p.Preco,
                        Estoque = x.p.Estoque,
                        Imagem = x.p.ImagemProduto.Select(img => img.CaminhoImagem).FirstOrDefault()
                    })
                    .ToList();
            }
        }

        // NOVO: filtro por preço máximo
        public List<ProdutoListaDTO> ListarAtivosPorPrecoMax(decimal precoMax)
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Produto
                    .Where(p => p.Ativo && p.Preco <= precoMax)
                    .Select(p => new ProdutoListaDTO
                    {
                        IdProduto = p.IdProduto,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Preco = p.Preco,
                        Estoque = p.Estoque,
                        Imagem = p.ImagemProduto.Select(img => img.CaminhoImagem).FirstOrDefault()
                    })
                    .ToList();
            }
        }

        // NOVO: filtro por faixa de preço
        public List<ProdutoListaDTO> ListarAtivosPorFaixaPreco(decimal? precoMin, decimal? precoMax)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var query = ctx.Produto.Where(p => p.Ativo);
                if (precoMin.HasValue)
                    query = query.Where(p => p.Preco >= precoMin.Value);
                if (precoMax.HasValue)
                    query = query.Where(p => p.Preco <= precoMax.Value);

                return query
                    .Select(p => new ProdutoListaDTO
                    {
                        IdProduto = p.IdProduto,
                        Nome = p.Nome,
                        Descricao = p.Descricao,
                        Preco = p.Preco,
                        Estoque = p.Estoque,
                        Imagem = p.ImagemProduto.Select(img => img.CaminhoImagem).FirstOrDefault()
                    })
                    .ToList();
            }
        }

        // Corrigido: DAO não toca em UI. Só retorna dados para a camada de apresentação.
        public List<ProdutoCarrosselDados> BuscarDadosCarrossel(List<Produto> produtosAtivos)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var dados = produtosAtivos
                    .Select(p =>
                    {
                        string chave = $"P:{p.IdProduto}"; // chave preferencial por Id de produto
                        var cfg = ctx.Carousel.FirstOrDefault(c => c.Nome == chave)
                                  ?? ctx.Carousel.FirstOrDefault(c => c.Nome == p.Nome);
                        return new ProdutoCarrosselDados
                        {
                            IdProduto = p.IdProduto,
                            Nome = p.Nome,
                            Ativo = cfg != null && cfg.Ativo,
                            Ordem = (cfg != null && cfg.Ativo && cfg.Ordem > 0) ? (int?)cfg.Ordem : null
                        };
                    })
                    .ToList();

                return dados;
            }
        }
    }
}
