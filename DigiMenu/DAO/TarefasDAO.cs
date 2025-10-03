using System;

namespace DigiMenu.DAL
{
    public class TarefasDAO
    {
        protected readonly DigiMenuEntities Context;
        public TarefasDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
