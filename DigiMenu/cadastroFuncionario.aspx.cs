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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            lblMensagem.Visible = false;

            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text.Trim();
            string confirmarSenha = txtConfirmaSenha.Text.Trim();
            string telefone = txtTelefone.Text.Trim();

            // Validações básicas
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            {
                ExibirErro("Preencha todos os campos obrigatórios.");
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                ExibirErro("E-mail inválido.");
                return;
            }

            if (senha.Length < 6)
            {
                ExibirErro("A senha deve ter pelo menos 6 caracteres.");
                return;
            }

            if (senha != confirmarSenha)
            {
                ExibirErro("As senhas não coincidem.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(telefone) && !telefone.All(char.IsDigit))
            {
                ExibirErro("O telefone deve conter apenas números.");
                return;
            }

            try
            {
                var dao = new UsuarioDAO();

                if (dao.EmailExiste(email))
                {
                    ExibirErro("E-mail já cadastrado.");
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

           
              



                lblMensagem.Text = "Usuário cadastrado com sucesso!";
                lblMensagem.CssClass += " text-success";
                lblMensagem.Visible = true;

                LimparCampos();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null) msg += " | Inner: " + ex.InnerException.Message;
                if (ex.InnerException?.InnerException != null) msg += " | Inner2: " + ex.InnerException.InnerException.Message;
                ExibirErro("Ocorreu um erro ao cadastrar o usuário: " + msg);
            }
        }

        private void ExibirErro(string mensagem)
        {
            lblMensagem.Text = mensagem;
            lblMensagem.CssClass += " text-danger";
            lblMensagem.Visible = true;
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