using APIRESIDENCIAS.Models;

namespace APIRESIDENCIAS.Repositories
{
    public class IniciarSesionRepository :Repository<Inciarsesion>
    {
        public IniciarSesionRepository(Sistem21ResidenciaswebcaContext context) : base(context)
        {

        }
        public override IEnumerable<Inciarsesion> GetAll()
        {
            return base.GetAll().OrderBy(x => x.Numcontrol);
        }
    }
}
