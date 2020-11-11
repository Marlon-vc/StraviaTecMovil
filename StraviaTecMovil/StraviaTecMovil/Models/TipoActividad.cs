using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StraviaTecMovil.Models
{
    [Table("TIPO_ACTIVIDAD")]
    public class TipoActividad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Actividad> Actividades { get; set; }
    }
}
