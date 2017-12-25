using FCT.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace FCT.Control.Services
{
    public class DbTabVmStore : IDbTabVmStore
    {
        private List<IDbTabViewModel> _dbTabViewModels;

        public DbTabVmStore()
        {
            _dbTabViewModels = new List<IDbTabViewModel>();
        }

        public void Add(IDbTabViewModel dbTabViewModel)
        {
            _dbTabViewModels.Add(dbTabViewModel);
        }

        public IList<IDbTabViewModel> GetAll()
        {
            return _dbTabViewModels;
        }
    }
}
