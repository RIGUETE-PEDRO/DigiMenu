using System;
using DigiMenu.DAO;

namespace DigiMenu.admin
{
    public partial class FrmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            
                string usuario = txtUsuario.Text.Trim();
                string senha = txtSenha.Text.Trim();

                string senhaHash = new HashHelper().GerarHashSHA256(senha);

                try
                {
                    var usuarioDao = new UsuarioDAO();
                    var user = usuarioDao.Autenticar(usuario, senhaHash);

                    if (user != null)
                    {
                        // Autenticação bem-sucedida
                        Session["UsuarioId"] = user.Id;
                        Session["UsuarioNome"] = user.Nome;
                        Session["TipoUsuarioId"] = user.TipoUsuarioId;
                        
                        LogDAO log = new LogDAO();
                        log.Registrar(user.Id, 2);

                        if (user.TipoUsuarioId == 2)
                        {
                            // Administrador
                            Response.Redirect("admin/FrmPainelAdministrativo.aspx");
                        }
                        else
                        {
                            // Usuário comum
                            Response.Redirect("Default.aspx");
                    }
                    
                    }
                    else
                    {
                        ExibirMensagem("Usuário ou senha inválidos.", false);
                    }
                }
                catch (Exception ex)
                {
                    ExibirMensagem("Erro ao processar login: " + ex.Message, false);
                }
            }
        

        private void ExibirMensagem(string msg, bool sucesso)
        {
            lblMensagem.Text = msg;
            lblMensagem.CssClass = sucesso ? "text-success" : "text-danger";
            lblMensagem.Visible = true;
        }
    }
}
