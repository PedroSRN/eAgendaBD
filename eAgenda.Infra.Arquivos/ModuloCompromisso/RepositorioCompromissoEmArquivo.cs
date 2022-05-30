using eAgenda.Dominio.ModuloCompromisso;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Infra.Arquivos.ModuloCompromisso
{
    public class RepositorioCompromissoEmArquivo : RepositorioEmArquivoBase<Compromisso>, IRepositorioCompromisso
    {
        public RepositorioCompromissoEmArquivo(DataContext dataContext) : base(dataContext)
        {
            if (dataContext.Compromissos.Count > 0)
                contador = dataContext.Compromissos.Max(x => x.Numero);
        }

        public override List<Compromisso> ObterRegistros()
        {
            return dataContext.Compromissos;
        }

        public override AbstractValidator<Compromisso> ObterValidador()
        {
            return new ValidadorCompromisso();
        }       

        public List<Compromisso> SelecionarCompromissosFuturos(DateTime dataInicial, DateTime dataFinal)
        {
            return ObterRegistros()
                .Where(x => x.Data >= dataInicial)
                .Where(x => x.Data <= dataFinal)
                .ToList();
        }

        public List<Compromisso> SelecionarCompromissosPassados(DateTime hoje)
        {
            return ObterRegistros()
                .Where(x => x.Data < hoje)
                .ToList();
        }
    }
}
