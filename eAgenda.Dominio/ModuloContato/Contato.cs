using eAgenda.Dominio.Compartilhado;

namespace eAgenda.Dominio.ModuloContato
{
    public class Contato : EntidadeBase<Contato>
    {
        public Contato()
        {
        }

        public Contato(string n, string e, string t, string emp, string c)
        {
            Nome = n;
            Email = e;
            Telefone = t;
            Empresa = emp;
            Cargo = c;
        }

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }


        public override void Atualizar(Contato registro)
        {
        }
    }
}
