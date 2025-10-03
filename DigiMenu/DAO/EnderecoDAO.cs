using System;

namespace DigiMenu.DAL
{
    public class EnderecoDAO
    {
        protected readonly DigiMenuEntities Context;
        public EnderecoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
