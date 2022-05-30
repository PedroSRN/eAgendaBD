using eAgenda.Dominio.ModuloContato;
using eAgenda.WinApp.Compartilhado;
using System.Collections.Generic;
using System.Windows.Forms;

namespace eAgenda.WinApp.ModuloContato
{
  

    internal class ControladorContato : ControladorBase
    {
        private readonly IRepositorioContato repositorioContato;
        private TabelaContatosControl tabelaContatos;
        public ControladorContato(IRepositorioContato repositorioContato)
        {
            this.repositorioContato = repositorioContato;
        }

        public override void Inserir()
        {
            TelaCadastroContatosForm tela = new TelaCadastroContatosForm();
            tela.Contato = new Contato();

            tela.GravarRegistro = repositorioContato.Inserir;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarContatos();
            }
        }

        public override void Editar()
        {
            Contato contatoSelecionado = ObtemContatoSelecionado();

            if (contatoSelecionado == null)
            {
                MessageBox.Show("Selecione um contato primeiro",
                "Edição de Contatos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TelaCadastroContatosForm tela = new TelaCadastroContatosForm();

            tela.Contato = contatoSelecionado;

            tela.GravarRegistro = repositorioContato.Editar;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                CarregarContatos();
            }
        }

        public override void Excluir()
        {
            Contato contatoSelecionado = ObtemContatoSelecionado();

            if (contatoSelecionado == null)
            {
                MessageBox.Show("Selecione um contato primeiro",
                "Exclusão de Contatos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult resultado = MessageBox.Show("Deseja realmente excluir a contato?",
                "Exclusão de Contatos", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                repositorioContato.Excluir(contatoSelecionado);
                CarregarContatos();
            }
        }

        public override void Agrupar()
        {
            TelaAgrupamentoContatoForm telaAgrupamento = new TelaAgrupamentoContatoForm();

            if (telaAgrupamento.ShowDialog() == DialogResult.OK)
            {
                tabelaContatos.AgruparContatos(telaAgrupamento.TipoAgrupamento);
            }
        }

        public override UserControl ObtemListagem()
        {
            //if (tabelaContatos == null)
            tabelaContatos = new TabelaContatosControl();

            CarregarContatos();

            return tabelaContatos;
        }

        public override ConfiguracaoToolboxBase ObtemConfiguracaoToolbox()
        {
            return new ConfiguracaoToolboxContato();
        }

        private Contato ObtemContatoSelecionado()
        {
            var numero = tabelaContatos.ObtemNumeroContatoSelecionado();

            return repositorioContato.SelecionarPorNumero(numero);
        }

        private void CarregarContatos()
        {
            List<Contato> contatos = repositorioContato.SelecionarTodos();

            tabelaContatos.AtualizarRegistros(contatos);

            TelaPrincipalForm.Instancia.AtualizarRodape($"Visualizando {contatos.Count} contato(s)");

        }
    }
}
