using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiscalProto
{
    /// <summary>
    /// Clase que describe un BATCH, que es un conjunto de comandos a ejecutar de una sola vez.
    /// </summary>
    public class Batch
    {
        string fileName;

        /// <summary>
        /// El nombre (corto) del comando.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// La descripcion (larga) del comando.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indica si el comando está habilitado para impresores EPSON.
        /// </summary>
        public bool EnabledEpson { get; set; }

        /// <summary>
        /// Indica si el comando está habilitado para impresores MORETTI.
        /// </summary>
        public bool EnabledMoretti { get; set; }

        /// <summary>
        /// Indica la lista de comandos a ejecutar.
        /// </summary>
        public List<string> Items { get; private set; }

        /// <summary>
        /// El nombre (completo) del archivo donde se graba el comando.
        /// </summary>
        public string FileName
        {
            get
            {
                if (fileName != null) return fileName;
                if (Name.IndexOf('<') >= 0) return "";
                return Name.Replace(' ', '_') + ".dat";
            }
            set { fileName = value; }
        }

        /// <summary>
        /// Nombre para los listbox
        /// </summary>
        /// 
        /// <returns></returns>
        public override string ToString()
        {
            var res = Name + $" [{ Items.Count }]";
            if (!EnabledMoretti || !EnabledEpson)
                res += EnabledMoretti? " (Moretti)" : " (Epson)";
            return res;
        }

        /// <summary>
        /// Helper method que se asegura que el nombre de archivo del comando esté correctamente formateado (lo
        /// que no asegura que éste exista...)
        /// </summary>
        /// 
        /// <param name="fn"></param>
        /// <returns></returns>
        string LocateBatch(string fn)
        {
            fn = fn.Replace('/', '_').Replace(' ', '_');
            if (!File.Exists(fn) && fn.IndexOf('\\') < 0)
            {
                fn = Path.Combine("batchs", fn);
                if (!Directory.Exists("batchs"))
                    Directory.CreateDirectory("batchs");
            }

            return fn;
        }

        /// <summary>
        /// Constructor de un nuevo batch (vacio)
        /// </summary>
        public Batch()
        {
            Items = new List<string>();
            Name = "<Nuevo>";
            Description = "";
            EnabledEpson = EnabledMoretti = true;
        }

        /// <summary>
        /// Construye un batch a partir de los datos del archivo.
        /// </summary>
        /// 
        /// <param name="fn">El archivo con el comando. Si contiene un PATH, no se lo ubica en otro lugar.</param>
        public Batch(string fn) 
            : this()
        {
            fn = LocateBatch(fn);

            var lines = File.ReadAllLines(fn);
            var parts = lines[0].Split(';');
            Name = parts[0].Trim();
            Description = parts[1].Trim();
            FileName = fn;
            EnabledEpson = parts[2].Trim().ToLower() == "s";
            EnabledMoretti = parts[3].Trim().ToLower() == "s";
            for (int i = 1; i < lines.Length; i++)
                Items.Add(lines[i]);
        }

        /// <summary>
        /// Graba la definición del batch.
        /// </summary>
        /// 
        /// <param name="fn">Nombre del archivo donde se grabará la definición. Si es <b>null</b>k, se usa el nombre ya definido.</param>
        public void Save(string fn = null)
        {
            if (fn != null) FileName = fn;
            FileName = LocateBatch(FileName);

            using (var sw = File.CreateText(FileName))
            {
                var bool1 = EnabledEpson ? "s" : "n";
                var bool2 = EnabledMoretti ? "s" : "n";
                sw.WriteLine($"{Name.Replace(';',',')};{Description.Replace(';',',')};{bool1};{bool2}");
                foreach (var line in Items)
                    sw.WriteLine(line.Trim());
            }
        }

        /// <summary>
        /// Clona el batch actual en uno nuevo (se usa para poder cancelar las ediciones)
        /// </summary>
        /// 
        /// <returns>El nuevo BATCH.</returns>
        public Batch Clone()
        {
            var nv = new Batch();

            nv.Name = this.Name;
            nv.Description = this.Description;
            nv.FileName = this.FileName;
            nv.EnabledEpson = this.EnabledEpson;
            nv.EnabledMoretti = this.EnabledMoretti;
            foreach (var item in this.Items)
                nv.Items.Add(item);
            return nv;
        }
    }
}
