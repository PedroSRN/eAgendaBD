using System;

namespace eAgenda.Dominio.ModuloTarefa
{
    [Serializable]
    public class ItemTarefa
    {
        public int Numero { get; set; }

        public string Titulo { get; set; }

        public bool Concluido { get; set; }

        public Tarefa Tarefa { get; set; }

        public override string ToString()
        {
            return Titulo;
        }

        public void Concluir()
        {
            Concluido = true;
        }

        internal void MarcarPendente()
        {
            Concluido = false;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemTarefa tarefa &&
                   Numero == tarefa.Numero &&
                   Titulo == tarefa.Titulo &&
                   Concluido == tarefa.Concluido;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Numero, Titulo, Concluido);
        }
    }
}
