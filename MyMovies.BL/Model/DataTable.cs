using MyMovies.BL.Model;
using System;
using System.Collections.Generic;

namespace MyMovies.BL
{
    public class DataTable
    {
        public DataTable(string[] columnsNames)
        {
            if (columnsNames is null)
            {
                throw new ArgumentNullException("Имена полей не могут быть пустыми", nameof(columnsNames));
            }

            ColumnsNames = columnsNames;  
            Rows = new List<Row>();
        }

        public string[] ColumnsNames { get; }
        public int ColumnCount => ColumnsNames.Length;
        public List<Row> Rows { get; set; }
    }
}
