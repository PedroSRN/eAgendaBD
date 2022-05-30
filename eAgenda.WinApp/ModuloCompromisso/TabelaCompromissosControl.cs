using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.WinApp.Compartilhado;
using System.Collections.Generic;
using System.Windows.Forms;

namespace eAgenda.WinApp.ModuloCompromisso
{
    public partial class TabelaCompromissosControl : UserControl
    {
        public TabelaCompromissosControl()
        {
            InitializeComponent();
            grid.ConfigurarGridZebrado();
            grid.ConfigurarGridSomenteLeitura();
            grid.Columns.AddRange(ObterColunas());
        }

        public DataGridViewColumn[] ObterColunas()
        {
            var colunas = new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "Numero", HeaderText = "Numero"},

                new DataGridViewTextBoxColumn { DataPropertyName = "Assunto", HeaderText = "Assunto"},

                new DataGridViewTextBoxColumn { DataPropertyName = "Data", HeaderText = "Data"},

                new DataGridViewTextBoxColumn { DataPropertyName = "HoraInicio", HeaderText = "Horário"},

                new DataGridViewTextBoxColumn {DataPropertyName = "Contato", HeaderText = "Contato"}
            };

            return colunas;
        }

        public int ObtemNumeroCompromissoSelecionado()
        {
            return grid.SelecionarNumero<int>();
        }

        public void AtualizarRegistros(List<Compromisso> compromissos)
        {
            grid.Rows.Clear();

            foreach (var compromisso in compromissos)
            {
                grid.Rows.Add(compromisso.Numero, compromisso.Assunto,
                    compromisso.Data.ToShortDateString(), compromisso.HoraInicio,
                    compromisso.Contato?.Nome);
            }
        }

    }
}
