using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DigiMenu.DAO;

namespace DigiMenu.admin
{
    public partial class Clientes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioLogado"] == null)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }
            if (Session["TipoUsuario"] == null || Convert.ToInt32(Session["TipoUsuario"]) != 2)
            {
                Response.Redirect("~/FrmLogin.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CarregarClientes();
            }
        }

        private void CarregarClientes()
        {
            var dao = new ClienteAdminDAO();
            var lista = dao.ListarTodos();
            pnlSemClientes.Visible = lista.Count == 0;
            rptClientes.DataSource = lista;
            rptClientes.DataBind();
        }

        protected void rptClientes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id;
            if (!int.TryParse(e.CommandArgument.ToString(), out id)) return;
            var dao = new ClienteAdminDAO();
            if (e.CommandName == "Editar")
            {
                var cliente = dao.BuscarPorId(id);
                if (cliente == null) return;
                hfClienteId.Value = cliente.Id.ToString();
                txtNome.Text = cliente.Nome;
                txtEmail.Text = cliente.Email;
                txtTelefone.Text = cliente.Telefone;
                pnlEdicao.Visible = true;
            }
            else if (e.CommandName == "Toggle")
            {
                bool novoEstado = dao.AlterarBloqueio(id);
                MostrarMensagem(novoEstado ? "Cliente bloqueado." : "Cliente desbloqueado.", "sucesso");
                CarregarClientes();
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(hfClienteId.Value, out id)) return;
            var dao = new ClienteAdminDAO();
            dao.AtualizarDados(id, txtNome.Text.Trim(), txtEmail.Text.Trim(), txtTelefone.Text.Trim());
            pnlEdicao.Visible = false;
            MostrarMensagem("Dados atualizados.", "sucesso");
            CarregarClientes();
            Response.Redirect("~/Clientes");

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlEdicao.Visible = false;
        }

        private void MostrarMensagem(string texto, string tipo)
        {
            phMensagens.Controls.Clear();
            phMensagens.Controls.Add(new Mensagens().MostrarMensagem(texto, tipo));
        }
    }
}