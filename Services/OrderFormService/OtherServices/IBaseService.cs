namespace OrderFormService.OtherServices
{
    using System;

    public interface IBaseService<TEntity> // TODO: ignore covariant hints from R# - whole interface does in-out
    {
        TEntity Find(int id);

        TEntity Find(Guid id);
    }
}