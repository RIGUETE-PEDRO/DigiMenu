using DigiMenu.DAO;
using System;
using System.Web.Services.Description;

namespace DigiMenu.admin
{
    public partial class FrmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            
                string usuario = txtUsuario.Text.Trim();
                string senha = txtSenha.Text.Trim();
                var mensagem = new Mensagens();

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
                    PlaceHolderMensagens.Controls.Clear();
                    var div = mensagem.MostrarMensagem("usuario ou senha errado.", "erro");
                    PlaceHolderMensagens.Controls.Add(div);
                    return;
                }
                }
                catch                 {

                //mensagem de modularizada
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("erro ao processar login.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }
            }
        

        
    }
}
