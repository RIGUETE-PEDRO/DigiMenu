using System;
using System.Web.UI;

namespace DigiMenu.DAL
{
    public class EnderecoDAO
    {
        protected readonly DigiMenuEntities Context;
        private readonly Control _control;

        public EnderecoDAO(DigiMenuEntities context = null, Control control = null)
        {
            Context = context ?? new DigiMenuEntities();
            _control = control;
        }

        // Salva o endereço e retorna o IdEndereco gerado
        public int SalvarEndereco(int usuarioId, Endereco endereco)
        {
            if (usuarioId == 0)
            {
                _control?.Page?.Response.Redirect("FrmLogin.aspx");
                return 0;
            }

            using (var ctx = new DigiMenuEntities())
            {
                endereco.UsuarioId = usuarioId;
                ctx.Endereco.Add(endereco);
                ctx.SaveChanges();
                return endereco.IdEndereco;
            }
        }
    }
}
