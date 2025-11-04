using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DigiMenu.DAL;

namespace DigiMenu
{
    public partial class StatusPedido : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int clienteId = 0;
                if (Session["ClienteId"] != null)
                {
                    int.TryParse(Session["ClienteId"].ToString(), out clienteId);
                }

                PedidoDAO pedidoDAO = new PedidoDAO();
                pedidoDAO.CarregarPedidos(clienteId, rptPedidos, pnlSemPedidos);
            }
        }

        protected void rptPedidos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int clienteId = 0;
            if (Session["ClienteId"] != null)
            {
                int.TryParse(Session["ClienteId"].ToString(), out clienteId);
            }

            int id;
            if (!int.TryParse(e.CommandArgument.ToString(), out id)) return;

            using (var ctx = new DigiMenuEntities())
            {
                var pedido = ctx.Pedido.FirstOrDefault(p => p.IdPedido == id && p.UsuarioId == clienteId);
                if (pedido == null) return;

                if (e.CommandName == "Aceitar")
                {
                    pedido.StatusId = 2; // Aceito
                }
                else if (e.CommandName == "Negar")
                {
                    pedido.StatusId = 3; // Negado
                }
                ctx.SaveChanges();
            }

            PedidoDAO pedidoDAO = new PedidoDAO();
            pedidoDAO.CarregarPedidos(clienteId, rptPedidos, pnlSemPedidos);
        }
    }
}