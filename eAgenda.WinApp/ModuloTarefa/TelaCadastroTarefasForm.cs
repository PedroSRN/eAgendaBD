using eAgenda.Dominio.ModuloTarefa;
using FluentValidation.Results;
using System;
using System.Windows.Forms;

namespace eAgenda.WinApp.ModuloTarefa
{
    public partial class TelaCadastroTarefasForm : Form // View
    {
        private Tarefa tarefa;

        public TelaCadastroTarefasForm()
        {
            InitializeComponent();

            CarregarPrioridades();
        }

        private void CarregarPrioridades()
        {
            var prioridades = Enum.GetValues(typeof(PrioridadeTarefaEnum));

            foreach (var item in prioridades)
            {
                cmbPrioridades.Items.Add(item);
            }

            cmbPrioridades.SelectedItem = PrioridadeTarefaEnum.Baixa;
        }

        public Func<Tarefa, ValidationResult> GravarRegistro { get; set; }

        public Tarefa Tarefa
        {
            get
            {
                return tarefa;
            }
            set
            {
                tarefa = value;
                txtNumero.Text = tarefa.Numero.ToString();
                txtTitulo.Text = tarefa.Titulo;
                cmbPrioridades.SelectedItem = tarefa.Prioridade;
            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            tarefa.Titulo = txtTitulo.Text;

            tarefa.Prioridade = (PrioridadeTarefaEnum)cmbPrioridades.SelectedItem;

            var resultadoValidacao = GravarRegistro(tarefa);

            if (resultadoValidacao.IsValid == false)
            {
                string erro = resultadoValidacao.Errors[0].ErrorMessage;

                TelaPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }
        }

        private void TelaCadastroTarefasForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void TelaCadastroTarefasForm_Load(object sender, EventArgs e)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape("");
        }
    }
}
