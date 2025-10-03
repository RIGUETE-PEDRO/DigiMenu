using System;

namespace DigiMenu.DAL
{
    public class EstadoDAO
    {
        protected readonly DigiMenuEntities Context;
        public EstadoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
