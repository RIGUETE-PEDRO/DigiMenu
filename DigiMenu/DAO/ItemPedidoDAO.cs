using System;

namespace DigiMenu.DAL
{
    public class ItemPedidoDAO
    {
        protected readonly DigiMenuEntities Context;
        public ItemPedidoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
