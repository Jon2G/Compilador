// Copyright (c) 2009 Daniel Grunwald
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using Microsoft.Win32;
using My8086.Clases;
using My8086.Clases.Advertencias;
using My8086.Clases.AutoCompletar;
using My8086.Clases.Compilador;

namespace My8086
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private readonly AutoCompletado AutoCompletado;
        readonly FoldingManager FoldingManager;
        readonly BraceFoldingStrategy FoldingStrategy;
        private Compilador Compilador;
        public ResultadosCompilacion Errores { get; set; }
        public Window1()
        {
            this.FoldingStrategy = new BraceFoldingStrategy();
            // Load our custom highlighting definition
            IHighlightingDefinition customHighlighting;
            using (Stream s = typeof(Window1).Assembly.GetManifestResourceStream("My8086.CustomHighlighting.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            IHighlightingDefinition customHighlighting2;
            using (Stream s = typeof(Window1).Assembly.GetManifestResourceStream("My8086.CustomHighlightingAsm.xshd"))
            {
                if (s == null)
                    throw new InvalidOperationException("Could not find embedded resource");
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting2 = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("My8086", new string[] { ".my86" }, customHighlighting);

            HighlightingManager.Instance.RegisterHighlighting("ASM", new string[] { ".asm" }, customHighlighting2);
            this.Errores = new ResultadosCompilacion();

            InitializeComponent();
            this.CmbxEjemplos.ItemsSource = Ejemplos.Ejemplo.ListarEjemplos();
            //this.CmbxEjemplos.SelectedItem = this.CmbxEjemplos.ItemsSource.OfType<Ejemplos.Ejemplo>().Last();


            this.FoldingManager = FoldingManager.Install(TxtMy.TextArea);
            this.FoldingStrategy = new BraceFoldingStrategy();
            this.FoldingStrategy.UpdateFoldings(this.FoldingManager, TxtMy.Document);


            this.AutoCompletado = new AutoCompletado(this.TxtMy.TextArea, this.Errores);
            this.AutoCompletado.Analizar();
            this.ErroresList.ItemsSource = this.Errores.Resultados;
            this.ErroresList.Items.Refresh();
            //propertyGridComboBox.SelectedIndex = 2;

            TxtMy.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("My8086");
            TxtAsm.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("ASM");

            //this.currentFileName = $@"{AppData.Directorio}\..\..\Ejemplos\PruebaCompleta.my86";
            // this.currentFileName = $@"{AppData.Directorio}\..\..\Ejemplos\QueMesEs.my86";
            //this.currentFileName = $@"{AppData.Directorio}\..\..\Ejemplos\DimeTuNombre.my86";
            TxtMy.Load($@"{AppData.Directorio}\..\..\Ejemplos\SIoNo.my86");
            TxtArchivo.Text = TxtMy.Document.FileName;
            //this.currentFileName = $@"{AppData.Directorio}\..\..\Ejemplos\ImprimeCadenas.txt";
            //this.currentFileName = $@"{AppData.Directorio}\..\..\Ejemplos\UsoVariablesNumericas.txt";


            // initial highlighting now set by XAML
            TxtMy.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            TxtMy.TextArea.TextEntered += textEditor_TextArea_TextEntered;

            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
            foldingUpdateTimer.Start();

            this.Compilador = new Compilador(TxtMy.TextArea.Document, this.Errores);
        }

        private void VerLinea(object sender, EventArgs e)
        {
            int linea = (sender as DocumentLine)?.LineNumber ?? -1;
            if (linea > 0)
            {
                var l = this.TxtMy.TextArea.Document.GetLineByNumber(linea);
                SelectText(l.Offset, l.Length);
                this.TxtMy.ScrollToLine(linea);
            }
        }


        void openFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { CheckFileExists = true };
            if (dlg.ShowDialog() ?? false)
            {
                this.Compilador.Compilado = false;
                TxtArchivo.Text = dlg.FileName;
                TxtMy.Load(dlg.FileName);
                TxtMy.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(dlg.FileName));
            }
        }

        void saveFileClick(object sender, EventArgs e)
        {
            if (TxtMy.Document.FileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".txt";
                if (dlg.ShowDialog() ?? false)
                {
                    TxtMy.Document.FileName = dlg.FileName;
                }
                else
                {
                    return;
                }
            }

            try
            {
                TxtMy.Save(TxtMy.Document.FileName);

            }
            catch (Exception ex)
            {

            }
        }

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            this.Compilador.Compilado = false;
            if (e.Text == "(")
            {
                this.AutoCompletado.AutoCompletar();
            }
        }

        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && AutoCompletado.CompletionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    AutoCompletado.CompletionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // do not set e.Handled=true - we still want to insert the character that was typed
        }
        void foldingUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (TxtMy.Document != null)
            {
                this.ProgresoCompilacion.IsIndeterminate = true;
                FoldingStrategy.UpdateFoldings(FoldingManager, TxtMy.Document);
                if (!this.AutoCompletado.Analizando)
                {
                    //Dispatcher.BeginInvoke(new Action(() =>
                    //{
                    this.AutoCompletado.Analizar();
                    this.ErroresList.ItemsSource = this.Errores.Resultados;
                    this.ErroresList.Items.Refresh();
                    //}));
                    this.ProgresoCompilacion.IsIndeterminate = false;
                }
            }

        }

        private void Ejecutar(object sender, RoutedEventArgs e)
        {
            if (!this.Compilador.Compilado)
            {
                if (MessageBox.Show("Debe compilar el código antes de poder ejecutarlo.\n¿Desea compilarlo ahora?",
                        "Compilar código", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Compilar(sender, e);
                    if (Compilador.Compilado)
                    {
                        Ejecutar(sender, e);
                    }
                }
            }
            else
            {
                this.ProgresoCompilacion.IsIndeterminate = true;
                Salida.Text = Compilador.Ejecutar();
            }
        }

        private void Compilar(object sender, RoutedEventArgs e)
        {
            if (TxtMy.Document.FileName != null)
            {
                saveFileClick(sender, e);
            }

            this.Compilador = new Compilador(this.TxtMy.TextArea.Document, this.Errores);
            Compilador.OnProgreso += (o, i) =>
            {
                this.ProgresoCompilacion.SetPercent(Compilador.Progreso);
            };
            this.Salida.Text = Compilador.Compilar();
            TxtAsm.Text = Compilador.ASM?.ToString();

            if (!this.Errores.Resultados.Any())
            {
                ErroresList.Items.Refresh();
                TabErrores.SelectedIndex = 1;
            }
            else
            {
                TabErrores.SelectedIndex = 0;
            }
            this.Salida.Text += string.Join("\n", this.Errores.Resultados
                .Select(x => $"->[{x.Linea}] " + x.Texto));

        }
        private void SelectText(int offset, int length)
        {
            //Get the line number based off the offset.
            var line = TxtMy.Document.GetLineByOffset(offset);
            var lineNumber = line.LineNumber;

            //Select the text.
            TxtMy.SelectionStart = offset;
            TxtMy.SelectionLength = length;

            //Scroll the textEditor to the selected line.
            var visualTop = TxtMy.TextArea.TextView.GetVisualTopByDocumentLine(lineNumber);
            TxtMy.ScrollToVerticalOffset(visualTop);
        }

        private void CompilarYEjecutar(object sender, RoutedEventArgs e)
        {
            Compilar(sender, e);
            if (Compilador.Compilado)
            {
                Ejecutar(sender, e);
            }
        }

        private void CmbxEjemplos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ejemplos.Ejemplo ex = (CmbxEjemplos.SelectedItem as Ejemplos.Ejemplo);
            TxtMy.Document.Text = ex.GetDocumento();
            TxtMy.Document.FileName = null;
            TxtArchivo.Text = ex.Nombre;


        }
    }

}