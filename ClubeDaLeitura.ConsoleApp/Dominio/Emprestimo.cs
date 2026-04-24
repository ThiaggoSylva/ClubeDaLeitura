using ClubeDaLeitura.ConsoleApp.Dominio.Base;
using ClubeDaLeitura.ConsoleApp.Dominio.Enums;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Emprestimo : EntidadeBase
{
    public Amigo Amigo { get; set; }
    public Revista Revista { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucao { get; set; }
    public DateTime? DataDevolucaoRealizada { get; set; }

    public StatusEmprestimo Status
    {
        get
        {
            if (DataDevolucaoRealizada.HasValue)
                return StatusEmprestimo.Concluido;

            if (DateTime.Now.Date > DataDevolucao.Date)
                return StatusEmprestimo.Atrasado;

            return StatusEmprestimo.Aberto;
        }
    }

    public Emprestimo(Amigo amigo, Revista revista)
    {
        Amigo = amigo;
        Revista = revista;
        DataEmprestimo = DateTime.Now.Date;
        DataDevolucao = DataEmprestimo.AddDays(revista.Caixa.DiasDeEmprestimo);
    }

    public void RegistrarDevolucao()
    {
        DataDevolucaoRealizada = DateTime.Now.Date;
        Revista.Status = Dominio.Enums.StatusRevista.Disponivel;
    }

    public override string[] Validar()
    {
        List<string> erros = new();

        if (Amigo == null)
            erros.Add("O amigo é obrigatório.");

        if (Revista == null)
            erros.Add("A revista é obrigatória.");

        return erros.ToArray();
    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Emprestimo e = (Emprestimo)entidadeAtualizada;

        Amigo = e.Amigo;
        Revista = e.Revista;
        DataEmprestimo = e.DataEmprestimo;
        DataDevolucao = e.DataDevolucao;
        DataDevolucaoRealizada = e.DataDevolucaoRealizada;
    }
}