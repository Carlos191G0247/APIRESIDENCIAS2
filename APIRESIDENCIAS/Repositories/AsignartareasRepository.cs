using APIRESIDENCIAS.Models;

namespace APIRESIDENCIAS.Repositories
{
    public class AsignartareasRepository:Repository<Asignaciontareas>
    {
        public AsignartareasRepository(Sistem21ResidenciaswebcaContext context) : base(context)
        {
            
        }
        public override IEnumerable<Asignaciontareas> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Id);
        }
    }
}
