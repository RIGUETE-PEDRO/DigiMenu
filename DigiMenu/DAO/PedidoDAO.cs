using System;

namespace DigiMenu.DAL
{
    public class PedidoDAO
    {
        protected readonly DigiMenuEntities Context;
        public PedidoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
