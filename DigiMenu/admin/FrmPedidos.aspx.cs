using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace DigiMenu.admin
{
    public partial class Pedidos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarPedidos();
            }
        }

        private void CarregarPedidos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                var pendentes = ctx.Pedido
                    .Where(p => p.StatusId == 1 || p.Status.Nome == "Pendente")
                    .Select(p => new
                    {
                        p.IdPedido,
                        p.Data,
                        p.Total,
                        Cliente = p.Usuario.Nome,
                        Status = p.Status.Nome,
                        Itens = p.ItemPedido.Select(i => new
                        {
                            Produto = i.Produto.Nome,
                            i.Quantidade,
                            i.PrecoUnitario
                        }).ToList()
                    })
                    .OrderByDescending(p => p.Data)
                    .ToList();

                var rpt = FindControl("rptPedidos") as Repeater;
                var pnlSem = FindControl("pnlSemPedidos") as Panel;

                if (pnlSem != null)
                {
                    pnlSem.Visible = pendentes.Count == 0;
                }

                if (rpt != null)
                {
                    rpt.DataSource = pendentes;
                    rpt.DataBind();

                    int index = 0;
                    foreach (RepeaterItem item in rpt.Items)
                    {
                        var pedido = pendentes[index++];
                        var rptItens = item.FindControl("rptItens") as Repeater;
                        if (rptItens != null)
                        {
                            rptItens.DataSource = pedido.Itens;
                            rptItens.DataBind();
                        }
                    }
                }
            }
        }

        protected void rptPedidos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id;
            if (!int.TryParse(e.CommandArgument.ToString(), out id)) return;

            using (var ctx = new DigiMenuEntities())
            {
                var pedido = ctx.Pedido.FirstOrDefault(p => p.IdPedido == id);
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

            CarregarPedidos();
        }
    }
}