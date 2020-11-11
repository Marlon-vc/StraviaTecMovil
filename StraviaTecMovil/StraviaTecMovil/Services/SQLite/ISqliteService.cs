using StraviaTecMovil.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StraviaTecMovil.Services.SQLite
{
    public interface ISqliteService
    {
        List<TipoActividad> GetTipoActividades();
        void InsertTipoActividad(TipoActividad item);
        void RemoveTipoActividad(TipoActividad item);
    }
}
