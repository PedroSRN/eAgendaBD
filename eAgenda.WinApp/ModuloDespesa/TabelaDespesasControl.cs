using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WinApp.Compartilhado;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace eAgenda.WinApp.ModuloDespesa
{
    public partial class TabelaDespesasControl : UserControl
    {
        public TabelaDespesasControl()
        {
            InitializeComponent();
            grid.ConfigurarGridZebrado();
            grid.ConfigurarGridSomenteLeitura();
            grid.Columns.AddRange(ObterColunas());
        }

        private DataGridViewColumn[] ObterColunas()
        {
            var colunas = new DataGridViewColumn[]
           {
                new DataGridViewTextBoxColumn { DataPropertyName = "Numero", HeaderText = "Número"},

                new DataGridViewTextBoxColumn { DataPropertyName = "Descricao", HeaderText = "Descrição"},

                new DataGridViewTextBoxColumn { DataPropertyName = "Valor", HeaderText = "Valor"},

                new DataGridViewTextBoxColumn { DataPropertyName = "Data", HeaderText = "Data"}
           };

            return colunas;
        }

        internal void AtualizarRegistros(List<Despesa> despesas)
        {
            grid.Rows.Clear();

            foreach (Despesa despesa in despesas)
            {
                grid.Rows.Add(despesa.Numero, despesa.Descricao, despesa.Valor, despesa.Data);
            }
        }

        internal int ObtemNumeroDespesaSelecionada()
        {
            return grid.SelecionarNumero<int>();
        }
    }
}
