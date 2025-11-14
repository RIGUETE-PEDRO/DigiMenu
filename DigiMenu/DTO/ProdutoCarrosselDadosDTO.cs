using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiMenu.DTO
{
    public class ProdutoCarrosselDadosDTO
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int? Ordem { get; set; }
    }
}