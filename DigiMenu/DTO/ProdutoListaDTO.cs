using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiMenu.DTO
{
    public class ProdutoListaDTO
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public string Imagem { get; set; }
    }
}