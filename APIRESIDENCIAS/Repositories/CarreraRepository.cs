using APIRESIDENCIAS.Models;

namespace APIRESIDENCIAS.Repositories
{
    public class CarreraRepository : Repository<Carrera>
    {
        public CarreraRepository(Sistem21ResidenciaswebcaContext context):base(context) 
        {
        
        }

        public override IEnumerable<Carrera> GetAll()
        {
            return base.GetAll().OrderBy(x => x.NombreCarrera);
        }

    }
}
