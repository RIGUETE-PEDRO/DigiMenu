using System;

namespace DigiMenu.DAL
{
    public class CarrinhoDAO
    {
        protected readonly DigiMenuEntities Context;
        public CarrinhoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
