namespace ClubeDaLeitura.ConsoleApp.Dominio.Base;

public abstract class EntidadeBase
{
    public string Id { get; set; }

    protected EntidadeBase()
    {
        Id = Guid.NewGuid().ToString("N")[..6].ToUpper();
    }

    public abstract string[] Validar();
    public abstract void AtualizarRegistro(EntidadeBase entidadeAtualizada);
}