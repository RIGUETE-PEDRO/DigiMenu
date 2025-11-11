using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu.admin
{
    public partial class Pedidos : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GarantirSecaoAceitos();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarPedidos();
                CarregarPedidosNaoPendentes();
                CarregarStatusDisponiveis();
            }
            if (Session["UsuarioLogado"] == null)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            // Verifica se é administrador (tipo 2)
            if (Session["TipoUsuario"] == null || Convert.ToInt32(Session["TipoUsuario"]) != 2)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }
        }

        private void GarantirSecaoAceitos()
        {
            // Se os controles já existem no markup, não recria
            if (FindControl("ddlNovoStatus") != null && FindControl("cblPedidosAceitos") != null && FindControl("btnAplicarStatus") != null && FindControl("pnlSemAceitos") != null)
                return;

            var container = new PlaceHolder { ID = "phAceitosDyn" };

            // Separador e título
            container.Controls.Add(new LiteralControl("<hr class=\"my-5\" />"));
            container.Controls.Add(new LiteralControl("<h2>Pedidos (não pendentes)</h2>"));

            // Painel vazio
            var pnlSemAceitos = new Panel { ID = "pnlSemAceitos", CssClass = "alert alert-info mt-3", Visible = false };
            pnlSemAceitos.Controls.Add(new LiteralControl("Nenhum pedido para listar."));
            container.Controls.Add(pnlSemAceitos);

            // Linha com dropdown e botão
            container.Controls.Add(new LiteralControl("<div class=\"row g-3 align-items-end mt-2\">"));
            var col1 = new Panel { CssClass = "col-12 col-md-4" };
            col1.Controls.Add(new LiteralControl("<label class=\"form-label\">Novo status</label>"));
            var ddlNovoStatus = new DropDownList { ID = "ddlNovoStatus", CssClass = "form-select" };
            col1.Controls.Add(ddlNovoStatus);
            container.Controls.Add(col1);

            var col2 = new Panel { CssClass = "col-12 col-md-8 d-flex" };
            var btnAplicar = new Button { ID = "btnAplicarStatus", CssClass = "btn btn-primary ms-auto", Text = "Aplicar status aos selecionados" };
            btnAplicar.Click += btnAplicarStatus_Click;
            col2.Controls.Add(btnAplicar);
            container.Controls.Add(col2);
            container.Controls.Add(new LiteralControl("</div>"));

            // CheckBoxList com pedidos não pendentes
            container.Controls.Add(new LiteralControl("<div class=\"mt-3\">"));
            var cbl = new CheckBoxList { ID = "cblPedidosAceitos", CssClass = "list-group" };
            container.Controls.Add(cbl);
            container.Controls.Add(new LiteralControl("</div>"));

            if (form1 != null)
            {
                form1.Controls.Add(container);
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

                var rpt = FindControl("rptPedidosAceitos") as Repeater;
                var pnlSem = FindControl("pnlSemAceitos") as Panel;

                if (pnlSem != null)
                {
                    pnlSem.Visible = naoPendentes.Count == 0;
                }

                if (rpt != null)
                {
                    rpt.DataSource = naoPendentes;
                    rpt.DataBind();
                }

                var cbl = FindControl("cblPedidosAceitos") as CheckBoxList;
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
            var ddl = FindControl("ddlNovoStatus") as DropDownList;
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
            var cbl = FindControl("cblPedidosAceitos") as CheckBoxList;
            var ddl = FindControl("ddlNovoStatus") as DropDownList;
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