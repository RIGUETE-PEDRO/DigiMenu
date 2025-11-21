using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; // para Entry / State

namespace DigiMenu.DAO
{
    public class ClienteAdminDAO
    {
        public List<ClienteAdminDTO> ListarTodos()
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Usuario
                    .Where(u => u.TipoUsuarioId != 2) // exclui administradores
                    .Select(u => new ClienteAdminDTO
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Email = u.Email,
                        Telefone = u.Telefone,
                        Criacao = u.Criacao,
                        Bloqueado = u.bloqueado,
                        Status = u.bloqueado ? "Bloqueado" : "Ativo"
                    }).ToList();
            }
        }

        public ClienteAdminDTO BuscarPorId(int id)
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Usuario
                    .Where(u => u.Id == id)
                    .Select(u => new ClienteAdminDTO
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Email = u.Email,
                        Telefone = u.Telefone,
                        Criacao = u.Criacao,
                        Bloqueado = u.bloqueado,
                        Status = u.bloqueado ? "Bloqueado" : "Ativo"
                    }).FirstOrDefault();
            }
        }

        public void AtualizarDados(int id, string nome, string email, string telefone)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var u = ctx.Usuario.FirstOrDefault(x => x.Id == id);
                if (u == null) return;
                u.Nome = nome;
                u.Email = email;
                u.Telefone = telefone;
                ctx.SaveChanges();
            }
        }

        // Alterna o valor da coluna bloqueado (persistente). Retorna novo estado.
        public bool AlterarBloqueio(int id)
        {
            using (var ctx = new DigiMenuEntities())
            {
                var u = ctx.Usuario.SingleOrDefault(x => x.Id == id);
                if (u == null) return false;
                u.bloqueado = !u.bloqueado;
                // Garante que EF reconheça modificação mesmo se Change Tracking estiver desativado
                ctx.Entry(u).Property(x => x.bloqueado).IsModified = true;
                ctx.SaveChanges();
                return u.bloqueado;
            }
        }

        public bool EstaBloqueado(int id)
        {
            using (var ctx = new DigiMenuEntities())
            {
                return ctx.Usuario.Where(u => u.Id == id).Select(u => u.bloqueado).FirstOrDefault();
            }
        }
    }

    public class ClienteAdminDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public System.DateTime Criacao { get; set; }
        public bool Bloqueado { get; set; }
        public string Status { get; set; }
    }
}