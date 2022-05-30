using eAgenda.Dominio.ModuloDespesa;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Infra.Arquivos.ModuloDespesa
{
    public class RepositorioDespesaEmArquivo : RepositorioEmArquivoBase<Despesa>, IRepositorioDespesa
    {
        public RepositorioDespesaEmArquivo(DataContext dataContext) : base(dataContext)
        {
            if (dataContext.Despesas.Count > 0)
                contador = dataContext.Despesas.Max(x => x.Numero);
        }

        public override List<Despesa> ObterRegistros()
        {
            return dataContext.Despesas;
        }

        public override AbstractValidator<Despesa> ObterValidador()
        {
            return new ValidadorDespesa();
        }
    }
}
