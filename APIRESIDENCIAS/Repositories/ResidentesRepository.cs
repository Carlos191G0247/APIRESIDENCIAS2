using APIRESIDENCIAS.Models;

namespace APIRESIDENCIAS.Repositories
{
    public class ResidentesRepository : Repository<Residentes>
    {
        public ResidentesRepository(Sistem21ResidenciaswebcaContext context) : base(context)
        {
            
        }
        public override IEnumerable<Residentes> GetAll()
        {
            return base.GetAll().OrderBy(x => x.NombreCompleto);
        }

    }
}
