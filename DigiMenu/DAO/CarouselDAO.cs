using System;

namespace DigiMenu.DAL
{
    public class CarouselDAO
    {
        protected readonly DigiMenuEntities Context;
        public CarouselDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
