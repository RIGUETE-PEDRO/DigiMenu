using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu.admin
{
    public partial class FrmPainelAdministrativo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
    }
}