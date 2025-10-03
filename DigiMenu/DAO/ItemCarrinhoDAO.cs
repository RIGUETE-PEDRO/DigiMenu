using System;

namespace DigiMenu.DAL
{
    public class ItemCarrinhoDAO
    {
        protected readonly DigiMenuEntities Context;
        public ItemCarrinhoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
