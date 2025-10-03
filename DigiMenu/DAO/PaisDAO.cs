using System;

namespace DigiMenu.DAL
{
    public class PaisDAO
    {
        protected readonly DigiMenuEntities Context;
        public PaisDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
