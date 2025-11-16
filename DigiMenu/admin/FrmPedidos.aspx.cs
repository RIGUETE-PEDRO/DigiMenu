using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DigiMenu.DAL;

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
            var dao = new PedidoDAO();
            dao.AdminCarregarPendentes(rptPedidos, pnlSemPedidos);
        }

        private void CarregarPedidosNaoPendentes()
        {
            var dao = new PedidoDAO();
            dao.AdminCarregarNaoPendentes(null, cblPedidosAceitos, pnlSemAceitos);
        }

        private void CarregarStatusDisponiveis()
        {
            var dao = new PedidoDAO();
            dao.CarregarStatus(ddlNovoStatus);
        }

        protected void rptPedidos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int pedidoId;
            if (!int.TryParse(e.CommandArgument.ToString(), out pedidoId)) return;

            var itemDao = new ItemPedidoDAO();
            var novoStatusId = itemDao.ObterStatusIdPorComando(e.CommandName);

            if (novoStatusId.HasValue)
            {
                var dao = new PedidoDAO();
                dao.AlterarStatus(pedidoId, novoStatusId.Value);
                MostrarMensagem("Status atualizado.", "sucesso");
                CarregarPedidos();
                CarregarPedidosNaoPendentes();
            }
        }

        protected void btnAplicarStatus_Click(object sender, EventArgs e)
        {
            int novoStatusId;
            if (!int.TryParse(ddlNovoStatus.SelectedValue, out novoStatusId))
            {
                MostrarMensagem("Selecione um status válido.", "erro");
                return;
            }

            var idsSelecionados = cblPedidosAceitos.Items.Cast<ListItem>()
                .Where(i => i.Selected)
                .Select(i => int.Parse(i.Value))
                .ToList();

            if (idsSelecionados.Count == 0)
            {
                MostrarMensagem("Selecione ao menos um pedido.", "alerta");
                return;
            }

            var dao = new PedidoDAO();
            dao.AlterarStatusEmMassa(idsSelecionados, novoStatusId);
            MostrarMensagem("Status aplicado aos pedidos selecionados.", "sucesso");
            CarregarPedidos();
            CarregarPedidosNaoPendentes();
        }

        private void MostrarMensagem(string texto, string tipo)
        {
            var ph = phMsg; // PlaceHolder no markup
            if (ph != null)
            {
                ph.Controls.Clear();
                var msg = new Mensagens().MostrarMensagem(texto, tipo);
                ph.Controls.Add(msg);
            }
        }
    }
}