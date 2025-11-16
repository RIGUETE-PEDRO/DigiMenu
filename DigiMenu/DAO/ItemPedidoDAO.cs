using System;
using System.Linq;
using System.Web.UI; // para Control
using System.Web.UI.WebControls;

namespace DigiMenu.DAL
{
    public class ItemPedidoDAO
    {
        private readonly Control _control;

        public ItemPedidoDAO(Control control = null)
        {
            _control = control;
        }

        // Retorna o StatusId correspondente ao comando (Aceitar/Negar). Null se não encontrado.
        public int? ObterStatusIdPorComando(string comando)
        {
            if (string.IsNullOrWhiteSpace(comando)) return null;
            using (var ctx = new DigiMenuEntities())
            {
                if (string.Equals(comando, "Aceitar", StringComparison.OrdinalIgnoreCase))
                {
                    var st = ctx.Status.FirstOrDefault(s => s.Nome.ToLower().Contains("aceit") || s.Nome.ToLower().Contains("aprov"));
                    if (st == null) st = ctx.Status.FirstOrDefault(s => s.IdStatus != 1);
                    return st?.IdStatus;
                }
                if (string.Equals(comando, "Negar", StringComparison.OrdinalIgnoreCase))
                {
                    var st = ctx.Status.FirstOrDefault(s => s.Nome.ToLower().Contains("neg") || s.Nome.ToLower().Contains("recus"));
                    if (st == null) st = ctx.Status.FirstOrDefault(s => s.IdStatus != 1);
                    return st?.IdStatus;
                }
            }
            return null;
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

                var rpt = _control?.FindControl("rptPedidos") as Repeater;
                var pnlSem = _control?.FindControl("pnlSemPedidos") as Panel;

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

        // Lista todos os pedidos NÃO pendentes (inclui 2, 3, etc.)
        private void CarregarPedidosNaoPendentes()
        {
            using (var ctx = new DigiMenuEntities())
            {
                var naoPendentes = ctx.Pedido
                    .Where(p => p.StatusId != 1)
                    .Select(p => new
                    {
                        p.IdPedido,
                        p.Data,
                        p.Total,
                        Cliente = p.Usuario.Nome,
                        Status = p.Status.Nome
                    })
                    .OrderByDescending(p => p.Data)
                    .ToList();

                var rpt = _control?.FindControl("rptPedidosAceitos") as Repeater;
                var pnlSem = _control?.FindControl("pnlSemAceitos") as Panel;

                if (pnlSem != null)
                {
                    pnlSem.Visible = naoPendentes.Count == 0;
                }

                if (rpt != null)
                {
                    rpt.DataSource = naoPendentes;
                    rpt.DataBind();
                }

                var cbl = _control?.FindControl("cblPedidosAceitos") as CheckBoxList;
                if (cbl != null)
                {
                    cbl.Items.Clear();
                    foreach (var p in naoPendentes)
                    {
                        string texto = $"#{p.IdPedido} - {p.Cliente} - {p.Data:dd/MM HH:mm} - Total R$ {p.Total:N2} - {p.Status}";
                        cbl.Items.Add(new ListItem(texto, p.IdPedido.ToString()));
                    }
                }
            }
        }

        private void CarregarStatusDisponiveis()
        {
            var ddl = _control?.FindControl("ddlNovoStatus") as DropDownList;
            if (ddl == null) return;
            using (var ctx = new DigiMenuEntities())
            {
                var lista = ctx.Status.OrderBy(s => s.IdStatus).Select(s => new { s.IdStatus, s.Nome }).ToList();
                ddl.DataSource = lista;
                ddl.DataTextField = "Nome";
                ddl.DataValueField = "IdStatus";
                ddl.DataBind();
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
            CarregarPedidosNaoPendentes();
            CarregarStatusDisponiveis();
        }

        protected void rptPedidosAceitos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var ddl = e.Item.FindControl("ddlStatus") as DropDownList;
            if (ddl == null) return;

            using (var ctx = new DigiMenuEntities())
            {
                var statuses = ctx.Status.OrderBy(s => s.IdStatus).Select(s => new { s.IdStatus, s.Nome }).ToList();
                ddl.DataSource = statuses;
                ddl.DataTextField = "Nome";
                ddl.DataValueField = "IdStatus";
                ddl.DataBind();
            }

            var dataItem = e.Item.DataItem;
            var propNome = dataItem.GetType().GetProperty("Status");
            if (propNome != null)
            {
                string statusNome = propNome.GetValue(dataItem, null) as string;
                var li = ddl.Items.FindByText(statusNome);
                if (li != null) li.Selected = true;
            }
        }

        protected void rptPedidosAceitos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "AlterarStatus") return;

            int id;
            if (!int.TryParse(e.CommandArgument.ToString(), out id)) return;

            var ddl = e.Item.FindControl("ddlStatus") as DropDownList;
            if (ddl == null) return;

            int novoStatusId;
            if (!int.TryParse(ddl.SelectedValue, out novoStatusId)) return;

            using (var ctx = new DigiMenuEntities())
            {
                var pedido = ctx.Pedido.FirstOrDefault(p => p.IdPedido == id);
                if (pedido == null) return;
                pedido.StatusId = novoStatusId;
                ctx.SaveChanges();
            }

            CarregarPedidos();
            CarregarPedidosNaoPendentes();
            CarregarStatusDisponiveis();
        }

        protected void btnAplicarStatus_Click(object sender, EventArgs e)
        {
            var cbl = _control?.FindControl("cblPedidosAceitos") as CheckBoxList;
            var ddl = _control?.FindControl("ddlNovoStatus") as DropDownList;
            if (cbl == null || ddl == null) return;

            int novoStatusId;
            if (!int.TryParse(ddl.SelectedValue, out novoStatusId)) return;

            var selecionados = cbl.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();
            if (selecionados.Count == 0) return;

            using (var ctx = new DigiMenuEntities())
            {
                var ids = selecionados.Select(int.Parse).ToList();
                var pedidos = ctx.Pedido.Where(p => ids.Contains(p.IdPedido)).ToList();
                foreach (var pedido in pedidos)
                {
                    pedido.StatusId = novoStatusId;
                }
                ctx.SaveChanges();
            }

            CarregarPedidos();
            CarregarPedidosNaoPendentes();
            CarregarStatusDisponiveis();
        }
    }
}
