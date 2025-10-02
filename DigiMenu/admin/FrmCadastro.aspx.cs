using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu
{
    public partial class FrmCadastro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Removido carregamento de tipos, pois TipoUsuarioId será fixo
        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // Resetar mensagem e recebe os dados do formulário
                lblMensagem.Visible = false;
                string nome = txtNome.Text.Trim();
                string email = txtEmail.Text.Trim();
                string senha = txtSenha.Text.Trim();
                string confirmarSenha = txtConfirmaSenha.Text.Trim();
                string telefone = txtTelefone.Text.Trim();

                // Validações
                if (senha == confirmarSenha)
                {
                    // Validações adicionais de campos vazios e formato
                    if (string.IsNullOrWhiteSpace(nome) ||
                        string.IsNullOrWhiteSpace(email) ||
                        string.IsNullOrWhiteSpace(senha))
                    {
                        ExibirErro("Preencha todos os campos obrigatórios.");
                        return;
                    }
                    // Validação simples de e-mail
                    if (!email.Contains("@") || !email.Contains("."))
                    {
                        ExibirErro("E-mail inválido.");
                        return;
                    }

                    // Validação de senha (mínimo 6 caracteres)
                    if (senha.Length < 6)
                    {
                        ExibirErro("A senha deve ter pelo menos 6 caracteres.");
                        return;
                    }
                    // Verifica se as senhas coincidem
                    if (senha != confirmarSenha)
                    {
                        ExibirErro("As senhas não coincidem.");
                        return;
                    }

                    // Validação de telefone (opcional, mas se preenchido deve ser numérico)
                    if (!string.IsNullOrWhiteSpace(telefone) && !telefone.All(char.IsDigit))
                    {
                        ExibirErro("O telefone deve conter apenas números.");
                        return;
                    }

                    try
                    {
                        // Verifica se o e-mail já está cadastrado
                        using (var ctx = new DigiMenuEntities())
                        {
                            bool emailExiste = ctx.Usuario.Any(u => u.Email == email);
                            if (emailExiste)
                            {
                                ExibirErro("E-mail já cadastrado.");
                                return;
                            }

                            // Instancia o helper de hash
                            HashHelper helper = new HashHelper();

                            // Cria o hash da senha
                            string senhaHash = helper.GerarHashSHA256(senha);

                            // Cria o novo usuário
                            var novoUsuario = new Usuario
                            {
                                Nome = nome,
                                Email = email,
                                HashSenha = senhaHash,
                                Telefone = telefone,
                                Criacao = DateTime.Now,
                                TipoUsuarioId = 1 // padrão fixo
                            };

                            // Salva no banco
                            ctx.Usuario.Add(novoUsuario);
                            ctx.SaveChanges();

                            // Mensagem de sucesso e limpa campos
                            lblMensagem.Text = "Usuário cadastrado com sucesso!";
                            lblMensagem.CssClass += " text-success";
                            lblMensagem.Visible = true;

                            // Limpa os campos do formulário
                            txtNome.Text = string.Empty;
                            txtSenha.Text = string.Empty;
                            txtConfirmaSenha.Text = string.Empty;
                            txtEmail.Text = string.Empty;
                            txtTelefone.Text = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        if (ex.InnerException != null)
                            msg += " | Inner: " + ex.InnerException.Message;
                        if (ex.InnerException != null && ex.InnerException.InnerException != null)
                            msg += " | Inner2: " + ex.InnerException.InnerException.Message;
                        ExibirErro("Ocorreu um erro ao cadastrar o usuário: " + msg);
                        return;
                    }
                }
                else
                {
                    lblMensagem.Text = "As senhas não coincidem. Tente novamente.";
                    lblMensagem.Visible = true;
                }
            }
        }

       

        private void ExibirErro(string mensagem)
        {
            lblMensagem.Text = mensagem;
            lblMensagem.CssClass += " text-danger";
            lblMensagem.Visible = true;
        }
    }
}