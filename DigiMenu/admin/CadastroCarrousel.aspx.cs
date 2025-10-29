using DigiMenu.DAL;
using DigiMenu.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DigiMenu.admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rptProdutos.ItemCreated += rptProdutos_ItemCreated;
            rptProdutos.ItemDataBound += rptProdutos_ItemDataBound;

            if (!IsPostBack)
            {
                CarregarProdutos();
            }
        }

        private static string BuildKey(int produtoId) => $"P:{produtoId}";

        private void CarregarProdutos()
        {
            var produtoDAO = new ProdutoDAO();
            var produtosAtivos = produtoDAO.BuscarAtivos();

            using (var ctx = new DigiMenuEntities())
            {
                var dados = produtosAtivos
                    .Select(p =>
                    {
                        string key = BuildKey(p.IdProduto);
                        var cfg = ctx.Carousel.FirstOrDefault(c => c.Nome == key)
                                  ?? ctx.Carousel.FirstOrDefault(c => c.Nome == p.Nome);
                        return new
                        {
                            IdProduto = p.IdProduto,
                            Nome = p.Nome,
                            Ativo = cfg != null && cfg.Ativo,
                            // Só mostra ordem quando ativo; se inativo (ou 0), deixa vazio na UI
                            Ordem = (cfg != null && cfg.Ativo && cfg.Ordem > 0) ? (int?)cfg.Ordem : null
                        };
                    })
                    .ToList();

                ViewState["ProdutoIds"] = dados.Select(d => d.IdProduto).ToList();
                rptProdutos.DataSource = dados;
                rptProdutos.DataBind();

                RegisterToggleInitScript();
                RegisterOrderUniqueScript();
            }
        }

        // Alterna habilitação e limpa o valor ao desmarcar
        private void rptProdutos_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var chk = e.Item.FindControl("chkAtivo") as CheckBox;
            if (chk != null)
            {
                chk.AutoPostBack = false;
                var currentClass = chk.InputAttributes["class"] ?? string.Empty;
                chk.InputAttributes["class"] = (string.IsNullOrWhiteSpace(currentClass) ? string.Empty : currentClass + " ") + "chk-ativo";
                string js = "var r=this.closest('tr');if(r){var i=r.querySelector('input.input_number');if(i){i.disabled=!this.checked;if(!this.checked){i.value='';}}}";
                chk.InputAttributes["onclick"] = js;
            }
        }

        // Estado inicial: habilita quando ativo; limpa texto quando inativo
        private void rptProdutos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
            var chk = e.Item.FindControl("chkAtivo") as CheckBox;
            var txt = e.Item.FindControl("txtOrdem") as TextBox;
            if (txt != null && chk != null)
            {
                txt.Enabled = chk.Checked;
                if (!chk.Checked)
                {
                    txt.Text = string.Empty;
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            var linhas = new List<(int ProdutoId, string Nome, bool Ativo, int? Ordem, TextBox CampoOrdem, CheckBox CampoAtivo)>();
            var produtoIds = ViewState["ProdutoIds"] as List<int> ?? new List<int>();

            for (int i = 0; i < rptProdutos.Items.Count; i++)
            {
                var item = rptProdutos.Items[i];
                if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) continue;

                var litNome = (Literal)item.FindControl("litNome");
                var chkAtivo = (CheckBox)item.FindControl("chkAtivo");
                var txtOrdem = (TextBox)item.FindControl("txtOrdem");

                string nome = litNome != null ? (litNome.Text ?? string.Empty).Trim() : string.Empty;
                bool ativo = chkAtivo != null && chkAtivo.Checked;

                int? ordem = null;
                if (ativo && txtOrdem != null && int.TryParse(txtOrdem.Text, out int ordemVal) && ordemVal > 0)
                {
                    ordem = ordemVal;
                }

                int produtoId = (i < produtoIds.Count) ? produtoIds[i] : 0;
                linhas.Add((produtoId, nome, ativo, ordem, txtOrdem, chkAtivo));
            }

            // Se ativo e sem ordem > 0 -> desativa e limpa textbox; no banco vai 0
            bool desativadosPorOrdem = false;
            for (int i = 0; i < linhas.Count; i++)
            {
                var l = linhas[i];
                if (l.Ativo && (!l.Ordem.HasValue || l.Ordem.Value <= 0))
                {
                    if (l.CampoAtivo != null) l.CampoAtivo.Checked = false;
                    if (l.CampoOrdem != null) l.CampoOrdem.Text = string.Empty;
                    linhas[i] = (l.ProdutoId, l.Nome, false, null, l.CampoOrdem, l.CampoAtivo);
                    desativadosPorOrdem = true;
                }
            }

            // Duplicidade de ordem entre ativos
            int? ordemDuplicada = linhas
                .Where(l => l.Ativo && l.Ordem.HasValue)
                .GroupBy(l => l.Ordem.Value)
                .Where(g => g.Count() > 1)
                .Select(g => (int?)g.Key)
                .FirstOrDefault();

            if (ordemDuplicada.HasValue)
            {
                foreach (var l in linhas.Where(l => l.Ativo && l.Ordem == ordemDuplicada))
                {
                    if (l.CampoOrdem != null) l.CampoOrdem.Text = string.Empty;
                }
                MostrarMensagem("Ordem inválida.");
                RegisterClearOrderValueScript(ordemDuplicada.Value);
                return;
            }

            // Persistência por Id: Nome = P:{IdProduto}, Ordem = 0 quando inativo
            try
            {
                using (var ctx = new DigiMenuEntities())
                {
                    foreach (var linha in linhas)
                    {
                        if (linha.ProdutoId <= 0) continue;
                        string key = BuildKey(linha.ProdutoId);

                        var existente = ctx.Carousel.FirstOrDefault(c => c.Nome == key)
                                       ?? ctx.Carousel.FirstOrDefault(c => c.Nome == linha.Nome);

                        if (existente == null)
                        {
                            existente = new Carousel { Nome = key };
                            ctx.Carousel.Add(existente);
                        }
                        else
                        {
                            existente.Nome = key;
                        }

                        existente.Ativo = linha.Ativo;
                        existente.Ordem = linha.Ativo ? linha.Ordem.GetValueOrDefault(0) : 0;
                    }

                    ctx.SaveChanges();
                    LogDAO log = new LogDAO();
                    int usuarioId = Convert.ToInt32(Session["UsuarioId"]);
                    log.Registrar(usuarioId, 4);
                }
            }
            catch (Exception ex)
            {
                MostrarMensagem("Erro ao salvar: " + ex.Message);
                return;
            }

            var msg = desativadosPorOrdem
                ? "Alguns itens foram mantidos inativos por não possuírem ordem > 0. Configurações salvas com sucesso."
                : "Configurações salvas com sucesso.";
            MostrarMensagem(msg);
            CarregarProdutos();
        }

        private void RegisterToggleInitScript()
        {
            var script = @"(function(){
                try{
                    var chks = document.querySelectorAll('input[type=checkbox].chk-ativo');
                    chks.forEach(function(c){
                        var r = c.closest('tr');
                        if(!r) return;
                        var i = r.querySelector('input.input_number');
                        if(!i) return;
                        i.disabled = !c.checked;
                        if(!c.checked){ i.value=''; }
                    });
                }catch(e){}
            })();";
            ClientScript.RegisterStartupScript(GetType(), "initToggleState", script, true);
        }

        private void RegisterOrderUniqueScript()
        {
            var script = @"(function(){
                try{
                    function isActiveRow(input){
                        var r = input.closest('tr');
                        if(!r) return false;
                        var chk = r.querySelector('input[type=checkbox].chk-ativo');
                        return !!(chk && chk.checked);
                    }
                    function validateUnique(ev){
                        var current = ev.target;
                        var val = (current.value||'').trim();
                        if(!val){return;}
                        var n = parseInt(val,10);
                        if(isNaN(n) || n<=0){ current.value=''; return; }
                        var dup = false;
                        var inputs = document.querySelectorAll('input.input_number');
                        inputs.forEach(function(inp){
                            if(inp===current) return;
                            if(!isActiveRow(inp)) return;
                            if((inp.value||'').trim() === val){ dup = true; }
                        });
                        if(dup){
                            alert('Ordem inválida');
                            current.value='';
                            try{ current.focus(); }catch(e){}
                        }
                    }
                    var inputs = document.querySelectorAll('input.input_number');
                    inputs.forEach(function(i){
                        i.addEventListener('change', validateUnique);
                        i.addEventListener('blur', validateUnique);
                    });
                }catch(e){}
            })();";
            ClientScript.RegisterStartupScript(GetType(), "orderUnique", script, true);
        }

        private void RegisterClearOrderValueScript(int ordem)
        {
            var script = $@"(function(){{
                var v = '{ordem}';
                try{{
                    document.querySelectorAll('input.input_number').forEach(function(i){{
                        if((i.value||'').trim()===v) i.value='';
                    }});
                }}catch(e){{}}
            }})();";
            ClientScript.RegisterStartupScript(GetType(), "clearDupOrder", script, true);
        }

        private void MostrarMensagem(string mensagem)
        {
            ClientScript.RegisterStartupScript(GetType(), "msg", $"alert('{mensagem.Replace("'", " ")}');", true);
        }
    }
}