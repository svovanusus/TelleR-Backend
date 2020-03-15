namespace TelleR.Logic.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateBasicUnitOfWork();

        IUnitOfWork CreateReadonlyUnitOfWork();
    }
}
