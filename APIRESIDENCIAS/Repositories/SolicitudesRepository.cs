using APIRESIDENCIAS.Models;

namespace APIRESIDENCIAS.Repositories
{
    public class SolicitudesRepository : Repository<Solicitudesalumnos>
    {
        public SolicitudesRepository(Sistem21ResidenciaswebcaContext context):base(context) 
        { 
           
        }
  

        public override IEnumerable<Solicitudesalumnos> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Carrera);
        }

        
    }
}
