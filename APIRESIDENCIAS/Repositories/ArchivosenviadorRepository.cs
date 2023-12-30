using APIRESIDENCIAS.Models;

namespace APIRESIDENCIAS.Repositories
{
    public class ArchivosenviadorRepository :Repository<Archivosenviados>
    {
        public ArchivosenviadorRepository(Sistem21ResidenciaswebcaContext context):base(context)
        {

        }
        public override IEnumerable<Archivosenviados> GetAll()
        {
            return base.GetAll().OrderBy(x => x.NombreArchivo);
        }

       
    }
}
