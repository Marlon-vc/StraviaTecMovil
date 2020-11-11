using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace StraviaTecMovil.Models
{
    [Table("ACTIVIDAD")]
    public class Actividad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public float Duracion { get; set; }
        public float Kilometros { get; set; }
        public bool Es_evento { get; set; }
        
        [ForeignKey(typeof(TipoActividad))]
        public int Id_tipo_actividad { get; set; }
        
        [ForeignKey(typeof(Recorrido))]
        public int Id_recorrido { get; set; }

        [OneToOne]
        public Recorrido Recorrido { get; set; }

        [ManyToOne]
        public TipoActividad tipoActividad { get; set; }
    }
}
