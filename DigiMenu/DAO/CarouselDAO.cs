using System;
using System.Collections.Generic;
using System.Linq;

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


        internal void SalvarOuAtualizar(Carousel carousel)
        {
            using (var ctx = new DigiMenuEntities())
            {
                Carousel existingCarousel = null;

                if (carousel.IdCarousel > 0)
                {
                    existingCarousel = ctx.Carousel.Find(carousel.IdCarousel);
                }

                if (existingCarousel == null && !string.IsNullOrWhiteSpace(carousel.Nome))
                {
                    // Chave preferencial: P:{IdProduto}
                    existingCarousel = ctx.Carousel.FirstOrDefault(c => c.Nome == carousel.Nome);

                    if (existingCarousel == null && carousel.Nome.StartsWith("P:"))
                    {
                        // Backcompat: se antes gravou só o nome descritivo, tenta localizar por nome antigo
                        // Nome antigo não é conhecido aqui, então não conseguimos migrar diretamente.
                        // A migração ocorrerá quando CarregarProdutos encontrar o antigo e passar a usar a nova chave.
                    }
                }

                if (existingCarousel != null)
                {
                    existingCarousel.Nome = carousel.Nome;
                    existingCarousel.Ativo = carousel.Ativo;
                    existingCarousel.Ordem = carousel.Ordem;
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
