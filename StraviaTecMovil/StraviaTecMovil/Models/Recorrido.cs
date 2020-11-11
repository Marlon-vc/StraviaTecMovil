using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StraviaTecMovil.Models
{
    [Table("RECORRIDO")]
    public class Recorrido
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha_hora { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Punto> Puntos { get; set; }



    }
}
