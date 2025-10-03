
using System;

namespace DigiMenu.DAL
{
    public class TipoUsuarioDAO
    {
        protected readonly DigiMenuEntities Context;
        public TipoUsuarioDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
