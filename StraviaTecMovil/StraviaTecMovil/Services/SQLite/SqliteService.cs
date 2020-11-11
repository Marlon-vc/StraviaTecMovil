using SQLite;
using StraviaTecMovil.Models;
using System.Collections.Generic;
using SQLiteNetExtensions.Extensions;
using System.IO;
using System;
using System.Threading;
using Xamarin.Forms.PlatformConfiguration;
using System.Linq;

namespace StraviaTecMovil.Services.SQLite
{
    public class SqliteService : ISqliteService
    {
        private readonly SemaphoreSlim readLock = new SemaphoreSlim(1, 1);

        private SQLiteConnection _sqlCon;
        private readonly string foreignKey = "PRAGMA foreign_keys = ON";

        public SqliteService()
        {
            //var dbPath = DependencyService.Get<IPathService>().GetDatabasePath();
            //string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "StraviaTec.sqlite");
            string dbPath = Path.Combine("/storage/emulated/0/Android/data/com.companyname.straviatecmovil/files/", "StraviaTec.sqlite");

            //var connectionString = new SQLiteConnectionString($"{dbPath};foreign keys=true;", storeDateTimeAsTicks: false);

            _sqlCon = new SQLiteConnection(dbPath, false);

            SQLiteCommand com = new SQLiteCommand(_sqlCon);
            com.CommandText = foreignKey;
            com.ExecuteNonQuery();
            CreateDatabase();

        }

        private void CreateDatabase()
        {
            _sqlCon.CreateTable<Recorrido>();
            _sqlCon.CreateTable<TipoActividad>();
            _sqlCon.CreateTable<Actividad>();
            _sqlCon.CreateTable<Punto>();
        }

        public List<TipoActividad> GetTipoActividades() => _sqlCon.GetAllWithChildren<TipoActividad>();

        public TipoActividad GetTipoActividad(int id) => _sqlCon.Table<TipoActividad>().FirstOrDefault(t => t.Id == id);

        public void InsertTipoActividad(TipoActividad item)
        {
            readLock.Wait();
            try
            {
                var activity = _sqlCon.Table<TipoActividad>().FirstOrDefault(a => a.Id == item.Id);
                if (activity == null)
                {
                    _sqlCon.InsertWithChildren(item, recursive: true);
                }
                else
                {
                    item.Id = activity.Id;
                    _sqlCon.UpdateWithChildren(item);
                }
            }
            finally
            {
                readLock.Release();
            }
        }

        public void RemoveTipoActividad(TipoActividad item) => _sqlCon.Delete(item);

        public  List<Actividad> GetActividades() =>  _sqlCon.GetAllWithChildren<Actividad>();

        public Actividad GetActividad(int id) => _sqlCon.Table<Actividad>().FirstOrDefault(a => a.Id == id);

        public void InsertActividad(Actividad item)
        {
            List<TipoActividad> activities = _sqlCon.GetAllWithChildren<TipoActividad>();
            readLock.Wait();
            try
            {
                var activity =  _sqlCon.Table<Actividad>().FirstOrDefault(a => a.Id == item.Id);
                TipoActividad tipoActividad = _sqlCon.Table<TipoActividad>().FirstOrDefault(t => t.Id == item.Id_tipo_actividad);
                Recorrido recorrido = _sqlCon.Table<Recorrido>().FirstOrDefault(r => r.Id == item.Id_recorrido);
                if (tipoActividad == null || recorrido == null)
                {
                    return;
                }

                item.tipoActividad = tipoActividad;
                item.Recorrido = recorrido;
                
                if (activity == null)
                {
                    
                    _sqlCon.InsertWithChildren(item);
                }
                else
                {
                    item.Id = activity.Id;
                     _sqlCon.UpdateWithChildren(item);
                }
            }
            finally
            {
                readLock.Release();
            }
        }

        public  void RemoveActividad(Actividad item) =>  _sqlCon.Delete(item);

        public  List<Recorrido> GetRecorridos() =>  _sqlCon.GetAllWithChildren<Recorrido>();

        public Recorrido GetRecorrido(int id) => _sqlCon.Table<Recorrido>().FirstOrDefault(r => r.Id == id);

        public void InsertRecorrido(Recorrido item)
        {
             readLock.Wait();
            try
            {
                var recorrido =  _sqlCon.Table<Recorrido>().FirstOrDefault(a => a.Id == item.Id);

                if (recorrido == null)
                {
                     _sqlCon.InsertWithChildren(item, recursive: true);
                }
                else
                {
                    item.Id = recorrido.Id;
                     _sqlCon.UpdateWithChildren(item);
                }
            }
            finally
            {
                readLock.Release();
            }
        }

        public  void RemoveRecorrido(Recorrido item) =>  _sqlCon.Delete(item);

        public List<Punto> GetPuntos() =>  _sqlCon.GetAllWithChildren<Punto>();

        public Punto GetPunto(int id) => _sqlCon.Table<Punto>().FirstOrDefault(p => p.Id == id);
        public void InsertPunto(Punto item)
        {
             readLock.Wait();
            try
            {
                var punto =  _sqlCon.Table<Punto>().FirstOrDefault(a => a.Id == item.Id);
                var recorrido = _sqlCon.Table<Recorrido>().FirstOrDefault(r => r.Id == item.Id_recorrido);

                if (recorrido == null) return;

                if (punto == null)
                {
                     _sqlCon.InsertWithChildren(item, recursive: true);
                }
                else
                {
                    item.Id = punto.Id;
                     _sqlCon.UpdateWithChildren(item);
                }
            }
            finally
            {
                readLock.Release();
            }
        }

        public void RemovePunto(Punto item) =>  _sqlCon.Delete(item);
    }
}
