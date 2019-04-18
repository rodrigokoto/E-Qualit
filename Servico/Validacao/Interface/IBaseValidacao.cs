namespace Servico.Validacao.Interface
{
    public interface IBaseValidacao<TEntity> where TEntity : class
    {
        void ObjetoValido(TEntity obj);
    }
}
