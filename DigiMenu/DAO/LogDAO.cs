using DigiMenu;
using System;

public class LogDAO
{
    public void Registrar(int usuarioId, int tarefaId)
    {
        using (var ctx = new DigiMenuEntities())
        {
            var log = new Log
            {
                UsuarioId = usuarioId,
                TarefasId = tarefaId,
                DataHora = DateTime.Now
            };
            ctx.Log.Add(log);
            ctx.SaveChanges();
        }
    }
}
