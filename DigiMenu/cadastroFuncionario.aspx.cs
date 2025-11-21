using DigiMenu.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu
{
    public partial class cadastroFuncionario : System.Web.UI.Page
    {
        Mensagens mensagem = new Mensagens();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Evento de clique do botão Cadastrar para cadastrar um novo funcionário
        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            

            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string confirmarSenha = txtConfirmaSenha.Text.Trim();
            string telefone = txtTelefone.Text.Trim();

            // Validações básicas
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("Preencha todos os campos obrigatórios.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("E-mail inválido.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }

            if (senha.Length < 6)
            {
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("A senha deve ter pelo menos 6 caracteres.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }

            if (senha != confirmarSenha)
            {
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("As senhas não coincidem.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }

            if (!string.IsNullOrWhiteSpace(telefone) && !telefone.All(char.IsDigit))
            {
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("O telefone deve conter apenas números.", "erro");
                PlaceHolderMensagens.Controls.Add(div);
                return;
            }

            try
            {
                var dao = new UsuarioDAO();

                if (dao.EmailExiste(email))
                {
                    PlaceHolderMensagens.Controls.Clear();
                    var msg = mensagem.MostrarMensagem("E-mail já cadastrado.", "erro");
                    PlaceHolderMensagens.Controls.Add(msg);
                  
                    return;
                }

                var helper = new HashHelper();
                string senhaHash = helper.GerarHashSHA256(senha);

                var novoUsuario = new Usuario
                {
                    Nome = nome,
                    Email = email,
                    HashSenha = senhaHash,
                    Telefone = telefone,
                    Criacao = DateTime.Now,
                    TipoUsuarioId = 2 // Tipo 2 para funcionários
                };

                // Salvar usuário usando DAO
                dao.Salvar(novoUsuario);





                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("Usuário cadastrado com sucesso!", "sucesso");
                PlaceHolderMensagens.Controls.Add(div);

               

                LimparCampos();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null) msg += " | Inner: " + ex.InnerException.Message;
                if (ex.InnerException?.InnerException != null) msg += " | Inner2: " + ex.InnerException.InnerException.Message;
                PlaceHolderMensagens.Controls.Clear();
                var div = mensagem.MostrarMensagem("Ocorreu um erro ao cadastrar o usuário: " + msg, "erro");
                PlaceHolderMensagens.Controls.Add(div);
                
            }
        }

        
      

        private void LimparCampos()
        {
            txtNome.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSenha.Text = string.Empty;
            txtConfirmaSenha.Text = string.Empty;
            txtTelefone.Text = string.Empty;
        }
    }
}