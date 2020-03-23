using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.AvalonEdit.Document;
using My8086.Clases.Funciones;

namespace My8086.Clases.Fases
{
    public class Programa:IBloque
    {
        public string Titulo;
        public SegmentoDeDatos SegmentoDeDatos;
        public DocumentLine Inicia;
        public readonly List<Accion> Acciones;
        public readonly ExpresionesRegulares.ExpresionesRegulares Expresiones;
        public Programa(string Titulo, ExpresionesRegulares.ExpresionesRegulares Expresiones)
        {
            this.Acciones=new List<Accion>();
            this.Expresiones = Expresiones;
        }
        public void AgregarAccion(Accion accion)
        {
            this.Acciones.Add(accion);
        }

        public DocumentLine FinBloque { get; set; }
        public DocumentLine InicioBloque { get; set; }
        public bool UsarSuma { get; internal set; }
        public bool UsarDivision { get; internal set; }
        public bool UsarResta { get; internal set; }
        public bool UsarMultiplicacion { get; internal set; }
        public bool OperadoresAritmeticos => UsarSuma || UsarDivision || UsarResta || UsarMultiplicacion|| OperacionesConNumeros;
        public bool OperacionesConCadenas { get; internal set; }
        public bool LeecturaNumeros { get; internal set; }
        public bool OperacionesConNumeros { get; internal set; }
        public bool OperacionesLogicas { get; internal set; }

        public string CerrarBloque()
        {
            throw new System.NotImplementedException();
        }

        StringBuilder IBloque.CerrarBloque()
        {
            throw new System.NotImplementedException();
        }
    }
}
