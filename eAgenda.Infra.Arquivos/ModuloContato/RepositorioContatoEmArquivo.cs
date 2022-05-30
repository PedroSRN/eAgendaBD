using eAgenda.Dominio.ModuloContato;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.Infra.Arquivos.ModuloContato
{
    public class RepositorioContatoEmArquivo : RepositorioEmArquivoBase<Contato>, IRepositorioContato
    {
        public RepositorioContatoEmArquivo(DataContext context) : base(context)
        {
            if (dataContext.Contatos.Count > 0)
                contador = dataContext.Contatos.Max(x => x.Numero);
        }

        public override List<Contato> ObterRegistros()
        {
            return dataContext.Contatos;
        }

        public override AbstractValidator<Contato> ObterValidador()
        {
            return new ValidadorContato();
        }
    }
}
