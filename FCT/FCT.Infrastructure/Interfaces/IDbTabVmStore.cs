using System.Collections.Generic;

namespace FCT.Infrastructure.Interfaces
{
    public interface IDbTabVmStore
    {
        void Add(IDbTabViewModel dbTabViewModel);

        IList<IDbTabViewModel> GetAll();
    }
}
