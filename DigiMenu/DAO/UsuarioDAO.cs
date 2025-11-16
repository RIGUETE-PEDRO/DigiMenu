using System;
using System.Linq;

namespace DigiMenu.DAO
{
    public class UsuarioDAO
    {
        public bool EmailExiste(string email)
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Usuario.Any(u => u.Email == email);
            }
        }

        public Usuario Salvar(Usuario usuario)
        {
            using (var ctx = new DigiMenuEntities())
            {
                ctx.Usuario.Add(usuario);
                ctx.SaveChanges();
                return usuario;
            }
        }

       

       

        public Usuario Autenticar(string login, string senhaHash)
        {
            using (var db = new DigiMenuEntities())
            {
                return db.Usuario
                         .FirstOrDefault(u => (u.Email == login || u.Telefone == login)
                                           && u.HashSenha == senhaHash);
            }
        }
    }
}
