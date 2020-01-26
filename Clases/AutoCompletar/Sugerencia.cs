using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My8086.Clases.AutoCompletar
{
    internal class Sugerencia
    {
        private int Orden;
        public string Token { get; set; }
        public IList<MyCompletionData> Sugerencias;
        public enum TipoSugerencia
        {
            Metodo
        }
        public Sugerencia(string Token)
        {
            this.Token = Token;
            this.Sugerencias = new List<MyCompletionData>();
            this.Orden = 1;
        }

        public Sugerencia Agregar(string Token, string Descripcion, TipoSugerencia Tipo)
        {
            string imagen = null;
            switch (Tipo)
            {
                case TipoSugerencia.Metodo:
                    imagen = @"Imgs\Method_left_16x.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(TipoSugerencia), Tipo, "Cual tipo?");
            }
            this.Sugerencias.Add(new MyCompletionData(Token, Descripcion, this.Orden++, imagen));
            return this;
        }
    }
}
