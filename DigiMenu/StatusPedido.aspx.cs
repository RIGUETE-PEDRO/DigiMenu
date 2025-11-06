using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DigiMenu.DAL;

namespace DigiMenu
{
    public partial class StatusPedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Exige login do usuário
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                PedidoDAO pedidoDAO = new PedidoDAO();
                pedidoDAO.CarregarPedidos(usuarioId, rptPedidos, pnlSemPedidos);
            }
        }

        protected void rptPedidos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Sem comandos para o cliente nesta tela.
        }
    }
}