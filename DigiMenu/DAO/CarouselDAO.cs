using System;
using System.Collections.Generic;
using System.Linq;
using static DigiMenu.admin.WebForm1;

namespace DigiMenu.DAL
{
    public class CarouselDAO
    {
        public Carousel BuscarPorId(int id)
        {
            using (var context = new DigiMenuEntities())
            {
                return context.Carousel.FirstOrDefault(c => c.IdCarousel == id);
            }
        }

        public List<Carousel> BuscarTodos()
        {
            using (var context = new DigiMenuEntities())
            {
                return context.Carousel.OrderBy(c => c.Ordem).ToList();
            }
        }

        // Atualiza/insere registros de carrossel usando a chave Nome = "P:{IdProduto}"
        public void AtualizarCarrousel(List<ProdutoCarrouselDTO> produtos)
        {
            if (produtos == null || produtos.Count == 0) return;

            using (var ctx = new DigiMenuEntities())
            {
                foreach (var dto in produtos)
                {
                    string chave = $"P:{dto.IdProduto}";
                    var registro = ctx.Carousel.FirstOrDefault(c => c.Nome == chave);
                    if (registro == null)
                    {
                        registro = new Carousel { Nome = chave };
                        ctx.Carousel.Add(registro);
                    }

                    registro.Ativo = dto.Ativo;
                    registro.Ordem = (dto.Ativo && dto.Ordem > 0) ? dto.Ordem : 0;
                }

                ctx.SaveChanges();
            }
        }

        internal void SalvarOuAtualizar(Carousel carousel)
        {
            using (var ctx = new DigiMenuEntities())
            {
                Carousel existente = null;

                if (carousel.IdCarousel > 0)
                {
                    existente = ctx.Carousel.Find(carousel.IdCarousel);
                }

                if (existente == null && !string.IsNullOrWhiteSpace(carousel.Nome))
                {
                    existente = ctx.Carousel.FirstOrDefault(c => c.Nome == carousel.Nome);
                }

                if (existente != null)
                {
                    existente.Nome = carousel.Nome;
                    existente.Ativo = carousel.Ativo;
                    existente.Ordem = carousel.Ordem;
                }
                else
                {
                    ctx.Carousel.Add(new Carousel
                    {
                        Nome = carousel.Nome,
                        Ativo = carousel.Ativo,
                        Ordem = carousel.Ordem
                    });
                }

                ctx.SaveChanges();
            }
        }
    }
}
