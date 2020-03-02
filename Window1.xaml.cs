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

            HighlightingManager.Instance.RegisterHighlighting("My8086", new string[] { ".txt", ".asm" }, customHighlighting);

            InitializeComponent();
            this.FoldingManager = new FoldingManager(textEditor.Document);
            this.AutoCompletado = new AutoCompletado(this.textEditor.TextArea);
            //propertyGridComboBox.SelectedIndex = 2;

            textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("My8086");
            this.currentFileName = $@"{AppData.Directorio}\..\..\Ejemplos\Variables.My8086.txt";
            textEditor.Load(currentFileName);
            //textEditor.SyntaxHighlighting = customHighlighting;
            // initial highlighting now set by XAML

            textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

            DispatcherTimer foldingUpdateTimer = new DispatcherTimer();
            foldingUpdateTimer.Interval = TimeSpan.FromSeconds(2);
            foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
            foldingUpdateTimer.Start();

            this.Compilador = new Compilador(textEditor.TextArea.Document);
        }

        string currentFileName;

        void openFileClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog { CheckFileExists = true };
            if (dlg.ShowDialog() ?? false)
            {
                this.Compilador.Compilado = false;
                currentFileName = dlg.FileName;
                textEditor.Load(currentFileName);
                textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(currentFileName));
            }
        }

        void saveFileClick(object sender, EventArgs e)
        {
            if (currentFileName == null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".txt";
                if (dlg.ShowDialog() ?? false)
                {
                    currentFileName = dlg.FileName;
                }
                else
                {
                    return;
                }
            }
            textEditor.Save(currentFileName);
        }

        //void propertyGridComboBoxSelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    if (propertyGrid == null)
        //        return;
        //    switch (propertyGridComboBox.SelectedIndex)
        //    {
        //        case 0:
        //            propertyGrid.SelectedObject = textEditor;
        //            break;
        //        case 1:
        //            propertyGrid.SelectedObject = textEditor.TextArea;
        //            break;
        //        case 2:
        //            propertyGrid.SelectedObject = textEditor.Options;
        //            break;
        //    }
        //}

        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            this.Compilador.Compilado = false;
            if (e.Text == ".")
            {
                this.AutoCompletado.AutoCompletar();
            }

            if (e.Text == "(")
            {
                //this.AutoCompletado.SugerirVariable();
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
            FoldingStrategy.UpdateFoldings(FoldingManager, textEditor.Document);

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
                Salida.Text = Compilador.Ejecutar();
            }
        }

        private void Compilar(object sender, RoutedEventArgs e)
        {
            saveFileClick(sender, e);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.ProgresoCompilacion.Visibility = Visibility.Visible;
                this.ProgresoCompilacion.IsIndeterminate = true;
            }));

            this.Compilador = new Compilador(this.textEditor.TextArea.Document);
            Compilador.OnProgreso += (o, i) =>
            {
                this.ProgresoCompilacion.SetPercent(Compilador.Progreso);
            };
            Compilador.VerLinea += (o, i) =>
            {
                int linea = (o as DocumentLine).LineNumber;
                if (linea > 0)
                {
                    var l = this.textEditor.TextArea.Document.GetLineByNumber(linea);
                    SelectText(l.Offset, l.Length);
                    this.textEditor.ScrollToLine(linea);
                }
            };
            this.Salida.Text = Compilador.Compilar();
            ErroresList.ItemsSource = Compilador.Resultados;
            ErroresList.Items.Refresh();
            Dispatcher.Invoke(() => { this.ProgresoCompilacion.Visibility = Visibility.Collapsed; },
                DispatcherPriority.ApplicationIdle);
            this.Salida.Text += string.Join("\n", Compilador.Resultados
                .Select(x => $"->[{x.Excepcion.Linea}] " + x.Excepcion.Texto));

            this.ProgresoCompilacion.Visibility = Visibility.Collapsed;
            this.ProgresoCompilacion.IsIndeterminate = false;



        }
        private void SelectText(int offset, int length)
        {
            //Get the line number based off the offset.
            var line = textEditor.Document.GetLineByOffset(offset);
            var lineNumber = line.LineNumber;

            //Select the text.
            textEditor.SelectionStart = offset;
            textEditor.SelectionLength = length;

            //Scroll the textEditor to the selected line.
            var visualTop = textEditor.TextArea.TextView.GetVisualTopByDocumentLine(lineNumber);
            textEditor.ScrollToVerticalOffset(visualTop);
        }

        private void CompilarYEjecutar(object sender, RoutedEventArgs e)
        {
            Compilar(sender, e);
            if (Compilador.Compilado)
            {
                Ejecutar(sender, e);
            }
        }
    }

}