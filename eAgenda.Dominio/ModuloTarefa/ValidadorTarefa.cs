using FluentValidation;
using System;

namespace eAgenda.Dominio.ModuloTarefa
{
    public class ValidadorTarefa : AbstractValidator<Tarefa>
    {
        public ValidadorTarefa()
        {
            RuleFor(x => x.Titulo)
                .NotNull().NotEmpty();

            RuleFor(x => x.DataCriacao)
                .NotEqual(DateTime.MinValue)
                .WithMessage("O campo Data de Criação é obrigatório");
        }
    }
}