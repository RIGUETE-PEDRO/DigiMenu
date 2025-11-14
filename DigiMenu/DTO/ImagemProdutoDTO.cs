using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigiMenu.DTO
{
    public class ImagemProdutoDTO
    {
            public int IdImagemProduto { get; set; }
            public string UrlImagem { get; set; }
            public string Descricao { get; set; }

            public string PrecoProduto { get; set; }
            public string NomeProduto { get; set; }
            public int ProdutoId { get; set; }
            public bool Ativo { get; set; }

            public int OrdemCarousel { get; set; }
        
    }
}