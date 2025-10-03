using System;

namespace DigiMenu.DAL
{
    public class StatusDAO
    {
        protected readonly DigiMenuEntities Context;
        public StatusDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
