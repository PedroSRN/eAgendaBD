using eAgenda.Dominio.Compartilhado;
using eAgenda.Dominio.ModuloContato;
using System;

namespace eAgenda.Dominio.ModuloCompromisso
{
    public class Compromisso : EntidadeBase<Compromisso>
    {
        private DateTime _date;
        public Compromisso()
        {
            Data = DateTime.Now;
            HoraInicio = Data.TimeOfDay;
            HoraTermino = Data.TimeOfDay;
        }

        public Compromisso(string assunto, string local, string link, DateTime data,
             TimeSpan horaInicio, TimeSpan horaTermino, Contato contato)
        {
            Assunto = assunto;
            Local = local;
            Link = link;
            Data = data;
            HoraInicio = horaInicio;
            HoraTermino = horaTermino;
            Contato = contato;
        }

        public string Assunto { get; set; }
        
        public string Local { get; set; }

        public TipoLocalizacaoCompromissoEnum TipoLocal { get; set; }

        public string Link { get; set; }
        public DateTime Data { get { return _date.Date; } set { _date = value; } }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraTermino { get; set; }
        public Contato Contato { get; set; }



        public override void Atualizar(Compromisso registro)
        {
        }
    }
}
