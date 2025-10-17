using System;

namespace DigiMenu.DAL
{
    public class PedidoDAO
    {
        

        internal void Salvar(Pedido pedido)
        {
            //salva o pedido no banco de dados
            using (var ctx = new DigiMenuEntities())
            {
                // Adiciona o pedido
               ctx.Pedido.Add(pedido);
                ctx.SaveChanges();
            }
        }
    }
}
