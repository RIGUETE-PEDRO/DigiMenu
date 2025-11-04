using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiMenu.DAO
{
    public class CategoriaDAO
    {
        public List<Categoria> ListarOrdenado()
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Categoria
                          .OrderBy(c => c.nome)
                          .ToList();
            }
        }
    }
}