using System;
using System.Linq;
using System.Web.UI;

namespace DigiMenu.admin
{
    public partial class FrmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, System.EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string senha = txtSenha.Text.Trim();

            HashHelper hashHelper = new HashHelper();

            string senhaHash = hashHelper.GerarHashSHA256(senha);

            try
            {
                using (var db = new DigiMenuEntities())
                {
                  var user = db.Usuario.FirstOrDefault(u => (u.Email == usuario || u.Telefone == usuario) && u.HashSenha == senhaHash);

                    if (user != null) {
                        
                        // Autenticação bem-sucedida
                        Session["UsuarioId"] = user.Id;
                        Session["UsuarioNome"] = user.Nome;
                        Session["TipoUsuarioId"] = user.TipoUsuarioId;

                        var loginUsuario = new Log
                        {
                            TarefasId = 2, // Login
                            DataHora = DateTime.Now,
                            UsuarioId = user.Id
                        };
                        db.Log.Add(loginUsuario);
                        db.SaveChanges();

                        // Redireciona para a página inicial do admin
                        Response.Redirect("../Default.aspx");
                    }
                    else {
                        // Autenticação falhou
                        // Exibe mensagem de erro
                        lblMensagem.Text = "Usuário ou senha inválidos.";
                        lblMensagem.Visible = true;
                        return;

                    }

                }


            }
            catch (Exception ex) {
                return;
            }



        }
    }
}