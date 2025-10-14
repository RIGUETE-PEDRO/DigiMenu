using DigiMenu.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu.admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Carrega produtos na tabela
                CarregarProdutos();
            }           
        }

        private void CarregarProdutos()
        {
            ProdutoDAO produtoDAO = new ProdutoDAO();

            List<Produto> produtosAtivos = produtoDAO.BuscarAtivos();

            rptProdutos.DataSource = produtosAtivos;
            rptProdutos.DataBind();


        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

        }
    }
}