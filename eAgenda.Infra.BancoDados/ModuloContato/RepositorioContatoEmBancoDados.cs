using eAgenda.Dominio.ModuloContato;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAgenda.Infra.BancoDados.ModuloContato
{
    public class RepositorioContatoEmBancoDados : IRepositorioContato
    {
        //private const string enderecoBanco = @"Data Source=(LocalDB)\MSSqlLocalDB;Initial Catalog = e - agendaDB; Integrated Security = True; Pooling=False";

        private const string enderecoBanco = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=e-agendaDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        
        #region Slq Queries
        private const string sqlInserir =
            @"INSERT INTO [TB_CONTATO]
                (
     
                     [NOME],
	                 [EMAIL],
	                 [TELEFONE],
	                 [EMPRESA],
	                 [CARGO]
                )
                VALUES
                     (
                    @NOME,
                    @EMAIL,
                    @TELEFONE,
                    @EMPRESA,
                    @CARGO
                );SELECT SCOPE_IDENTITY();";
        

        private const string sqlEditar =
             @"UPDATE[TB_CONTATO]
                SET
                    [NOME] = @NOME,

                    [EMAIL] = @EMAIL,

                    [TELEFONE] = @TELEFONE,

                    [EMPRESA] = @EMPRESA,

                    [CARGO] = @CARGO

                WHERE
                    [NUMERO] = @NUMERO";

        private const string sqlExcluir =
           @"DELETE FROM [TB_CONTATO]
		        WHERE
			        [NUMERO] = @NUMERO";

        private const string sqlSelecionarTodos =
            @"SELECT 
		            [NUMERO], 
		            [NOME], 
		            [EMAIL],
		            [TELEFONE],
		            [EMPRESA],
		            [CARGO]
	            FROM 
		            [TB_CONTATO]";

        private const string sqlSelecionarPorNumero =
            @"SELECT 
		            [NUMERO], 
		            [NOME], 
		            [EMAIL],
		            [TELEFONE],
		            [EMPRESA],
		            [CARGO]
	            FROM 
		            [TB_CONTATO]
		        WHERE
                    [NUMERO] = @NUMERO";

        #endregion

        public ValidationResult Inserir(Contato novoContato)
        {
            var validador = new ValidadorContato();

            var resultadoValidação = validador.Validate(novoContato);

            if (resultadoValidação.IsValid == false)
                return resultadoValidação;
         
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoInsercao = new SqlCommand(sqlInserir, conexaoComBanco);

            ConfigurarParametrosContato(novoContato, comandoInsercao);

            conexaoComBanco.Open();
            var id = comandoInsercao.ExecuteScalar();
            novoContato.Numero = Convert.ToInt32(id);

            conexaoComBanco.Close();

            return resultadoValidação;
        }
      
        public ValidationResult Editar(Contato contato)
        {
            var validador = new ValidadorContato();

            var resultadoValidacao = validador.Validate(contato);

            if (resultadoValidacao.IsValid == false)
                return resultadoValidacao;

            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoEdicao = new SqlCommand(sqlEditar, conexaoComBanco);

            ConfigurarParametrosContato(contato, comandoEdicao);

            conexaoComBanco.Open();
            comandoEdicao.ExecuteNonQuery();
            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public ValidationResult Excluir(Contato contato)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoExclusao = new SqlCommand(sqlExcluir, conexaoComBanco);

            comandoExclusao.Parameters.AddWithValue("NUMERO", contato.Numero);

            conexaoComBanco.Open();
            int numeroRegistrosExcluidos = comandoExclusao.ExecuteNonQuery();

            var resultadoValidacao = new ValidationResult();

            if (numeroRegistrosExcluidos == 0)
                resultadoValidacao.Errors.Add(new ValidationFailure("", "Não foi possível remover o registro"));

            conexaoComBanco.Close();

            return resultadoValidacao;
        }

        public List<Contato> SelecionarTodos()
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarTodos, conexaoComBanco);

            conexaoComBanco.Open();
            SqlDataReader leitorContato = comandoSelecao.ExecuteReader();

            List<Contato> contatos = new List<Contato>();

            while (leitorContato.Read())
            {
                Contato contato = ConverterParaContato(leitorContato);

                contatos.Add(contato);
            }

            conexaoComBanco.Close();

            return contatos;
        }
        public Contato SelecionarPorNumero(int numero)
        {
            SqlConnection conexaoComBanco = new SqlConnection(enderecoBanco);

            SqlCommand comandoSelecao = new SqlCommand(sqlSelecionarPorNumero, conexaoComBanco);

            comandoSelecao.Parameters.AddWithValue("NUMERO", numero);

            conexaoComBanco.Open();
            SqlDataReader leitorContato = comandoSelecao.ExecuteReader();

            Contato contato = null;
            if (leitorContato.Read())
                contato = ConverterParaContato(leitorContato);

            conexaoComBanco.Close();

            return contato;
        }

        private static Contato ConverterParaContato(SqlDataReader leitorContato)
        {
            int numero = Convert.ToInt32(leitorContato["NUMERO"]);
            string nome = Convert.ToString(leitorContato["NOME"]);
            string email = Convert.ToString(leitorContato["EMAIL"]);
            string telefone = Convert.ToString(leitorContato["TELEFONE"]);
            string empresa = Convert.ToString(leitorContato["EMPRESA"]);
            string cargo = Convert.ToString(leitorContato["CARGO"]);

            var contato = new Contato
            {
                Numero = numero,
                Nome = nome,
                Telefone = telefone,
                Email = email,
                Cargo = cargo,
                Empresa = empresa
            };

            return contato;
        }

        private void ConfigurarParametrosContato(Contato novoContato, SqlCommand comando)
        {
            comando.Parameters.AddWithValue("NUMERO", novoContato.Numero);
            comando.Parameters.AddWithValue("NOME", novoContato.Nome);
            comando.Parameters.AddWithValue("EMAIL", novoContato.Email);
            comando.Parameters.AddWithValue("TELEFONE", novoContato.Telefone);
            comando.Parameters.AddWithValue("EMPRESA", novoContato.Empresa);
            comando.Parameters.AddWithValue("CARGO", novoContato.Cargo);
        }
    }
}
