using System;

namespace DigiMenu.DAL
{
    public class ImagemProdutoDAO
    {
        protected readonly DigiMenuEntities Context;
        public ImagemProdutoDAO(DigiMenuEntities context = null)
        {
            Context = context ?? new DigiMenuEntities();
        }
    }
}
