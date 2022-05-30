using eAgenda.Dominio.Compartilhado;
using System;

namespace eAgenda.Dominio.ModuloDespesa
{
    public class Despesa : EntidadeBase<Despesa>
    {
        public Despesa()
        {
            Data = DateTime.Now;
        }

        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public FormaPgtoDespesaEnum FormaPagamento { get; set; }

        public CategoriaDespesaEnum Categoria { get; set; }

        public override void Atualizar(Despesa registro)
        {

        }

        public override string ToString()
        {
            return $"Número: {Numero}, Descrição: {Descricao}, Data: {Data.ToShortDateString()}, Categoria: {Categoria}";
        }
    }
}
