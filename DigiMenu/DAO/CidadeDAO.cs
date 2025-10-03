using System;

namespace DigiMenu.DAL
{
    public class CidadeDAO
    {
        protected readonly DigiMenuEntities Context;
        public CidadeDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
