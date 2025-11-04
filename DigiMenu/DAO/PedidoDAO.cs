using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace DigiMenu.DAL
{
    public class PedidoDAO
    {
        public void CarregarPedidos(int clienteId, Repeater rptPedidos, Panel pnlSemPedidos)
        {
            using (var ctx = new DigiMenuEntities())
            {
         
                var pedidos = ctx.Pedido
                    .Where(p => p.UsuarioId == clienteId) 
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
                        })
                    })
                    .AsEnumerable() 
                    .Select(p => new
                    {
                        p.IdPedido,
                        p.Data,
                        p.Total,
                        p.Cliente,
                        p.Status,
                        Itens = p.Itens.ToList()
                    })
                    .OrderByDescending(p => p.Data)
                    .ToList();

                
                pnlSemPedidos.Visible = !pedidos.Any();

                
                rptPedidos.DataSource = pedidos;
                rptPedidos.DataBind();

               
                int index = 0;
                foreach (RepeaterItem item in rptPedidos.Items)
                {
                    var pedido = pedidos[index++];
                    var rptItens = item.FindControl("rptItens") as Repeater;
                    if (rptItens != null)
                    {
                        rptItens.DataSource = pedido.Itens;
                        rptItens.DataBind();
                    }
                }
            }
        }

        internal void Salvar(Pedido pedido)
        {
            
            using (var ctx = new DigiMenuEntities())
            {
                ctx.Pedido.Add(pedido);
                ctx.SaveChanges();
            }
        }
    }
}
