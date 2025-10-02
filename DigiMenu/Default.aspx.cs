using System;
using System.Web.UI;

namespace DigiMenu
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var nomeCompleto = Session["UsuarioNome"] as string;
                if (!string.IsNullOrEmpty(nomeCompleto))
                {
                    string primeiroNome = nomeCompleto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    if (divLogin != null) divLogin.Visible = false;
                    if (divUser != null)
                    {
                        divUser.Visible = true;
                        if (lblUserName != null) lblUserName.InnerText = "Olá, " + primeiroNome + "!";
                    }
                }
                else
                {
                    if (divLogin != null) divLogin.Visible = true;
                    if (divUser != null) divUser.Visible = false;
                }
            }
        }
    }
}