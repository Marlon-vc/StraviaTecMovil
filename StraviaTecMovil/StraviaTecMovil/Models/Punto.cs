using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StraviaTecMovil.Models
{
    [Table("PUNTO")]
    public class Punto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Segmento { get; set; }
        public int Orden { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public DateTime Tiempo { get; set; }
        public float Elevacion { get; set; }
        
        [ForeignKey(typeof(Recorrido))]
        public int Id_recorrido { get; set; }

        [ManyToOne]
        public Recorrido Recorrido { get; set; }

    }
}
