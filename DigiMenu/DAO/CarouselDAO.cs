using System;

namespace DigiMenu.DAL
{
    public class CarouselDAO
    {
      

        internal void SalvarOuAtualizar(Carousel carousel)
        {
            using (var ctx = new DigiMenuEntities())
            {
                if (carousel.IdCarousel > 0)
                {
                    // Atualizar
                    var existingCarousel = ctx.Carousel.Find(carousel.IdCarousel);
                    if (existingCarousel != null)
                    {
                        existingCarousel.Nome = carousel.Nome;
                        existingCarousel.Ativo = carousel.Ativo;
                        existingCarousel.Ordem = carousel.Ordem;
                        // Atualize outros campos conforme necessário
                    }
                }
                else
                {
                    // Salvar novo
                    ctx.Carousel.Add(carousel);
                }
                ctx.SaveChanges();
            }
        }
    }
}
