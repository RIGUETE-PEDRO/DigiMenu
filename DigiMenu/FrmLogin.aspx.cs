using DigiMenu.DAO;
using System;
using System.Web.Services.Description;
using System.Web;
using System.Web.Security; // adicionada

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
                    // Autenticação bem-sucedida (session)
                    Session["UsuarioId"] = user.Id;
                    Session["UsuarioNome"] = user.Nome;
                    Session["TipoUsuarioId"] = user.TipoUsuarioId;
                    // Ajuste: chaves esperadas em outras páginas
                    Session["UsuarioLogado"] = user.Nome; // usado para validar se está logado
                    Session["TipoUsuario"] = user.TipoUsuarioId; // usado para validar perfil

                    // Cookie persistente próprio (opcional)
                    var cookie = new HttpCookie("DigiMenuUser");
                    cookie.Values["Id"] = user.Id.ToString();
                    cookie.Values["Nome"] = user.Nome ?? string.Empty;
                    cookie.Values["Tipo"] = user.TipoUsuarioId.ToString();
                    cookie.HttpOnly = true;
                    cookie.Secure = Request.IsSecureConnection;
                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);

                    // Gerar ticket Forms Authentication com o papel no UserData (Admin se TipoUsuarioId==2)
                    bool isAdmin = user.TipoUsuarioId == 2;
                    string roles = isAdmin ? "Admin" : "User";
                    var ticket = new FormsAuthenticationTicket(
                        1,
                        user.Nome ?? user.Id.ToString(),
                        DateTime.Now,
                        DateTime.Now.AddMinutes(60),
                        false,
                        roles,
                        FormsAuthentication.FormsCookiePath
                    );
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection,
                        Path = FormsAuthentication.FormsCookiePath
                    };
                    Response.Cookies.Add(authCookie);

                    LogDAO log = new LogDAO();
                    log.Registrar(user.Id, 2);

                    // Redireciona conforme o tipo (tipo 2 -> área administrativa via urlMapping ~/administracao)
                    if (isAdmin)
                    {
                        Response.Redirect("~/administracao", false);
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx", false);
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
            catch
            {
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("erro ao processar login.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }
        }
    }
}
