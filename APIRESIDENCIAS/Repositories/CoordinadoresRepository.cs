using APIRESIDENCIAS.Models;

namespace APIRESIDENCIAS.Repositories
{
    public class CoordinadoresRepository : Repository<Coordinadores>
    {
        public CoordinadoresRepository(Sistem21ResidenciaswebcaContext context) : base(context) 
        {

        
        }
        
        public override IEnumerable<Coordinadores>GetAll()
        {
            return base.GetAll().OrderBy(x => x.NombreCompleto);
        }
    }
}
