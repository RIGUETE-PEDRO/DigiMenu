using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace DigiMenu
{
    public class Mensagens
    {
        public static string Sucesso(string texto)
        {
            return "sucesso:{texto}";
        }

        public static string Erro(string texto)
        {
            return "erro:{texto}";
        }

        public static string Aviso(string texto)
        {
            return "aviso:{texto}";
        }


        public HtmlGenericControl MostrarMensagem(string texto, string tipo = "sucesso")
        {
            var divMensagem = new HtmlGenericControl("div");

            string classe = "alert alert-info"; // valor padrão

            if (tipo == "sucesso")
                classe = "alert alert-success";
            else if (tipo == "erro")
                classe = "alert alert-danger";
            else if (tipo == "alerta")
                classe = "alert alert-warning";

            divMensagem.Attributes["class"] = classe;
            divMensagem.InnerText = texto;
            divMensagem.Style["display"] = "block";

            return divMensagem;
        }

    }

}