using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Data;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace MyJournal.Export
{
    public class DataTableToWordExporter
    {
        public static void FileSave(string name, DataTable dataTable, string filePath)
        {
            Word.Application wordApp = new Word.Application();

            // Создание нового документа
            Word.Document doc = wordApp.Documents.Add();

            // Добавление заголовка и текущего времени
            Word.Paragraph header = doc.Content.Paragraphs.Add();
            header.Range.Text = "Таблица оценок "+name;
            header.Range.Font.Size = 14;
            header.Range.Font.Bold = 1;
            header.Format.SpaceAfter = 10;
            header.Range.InsertParagraphAfter();

            Word.Paragraph currentTime = doc.Content.Paragraphs.Add();
            currentTime.Range.Text = "Дата и время: " + DateTime.Now.ToString();
            currentTime.Range.Font.Size = 12;
            currentTime.Range.Font.Italic = 1;
            currentTime.Format.SpaceAfter = 10;
            currentTime.Range.InsertParagraphAfter();

            // Создание таблицы Word и настройка стилей
            Word.Table table = doc.Tables.Add(header.Range, dataTable.Rows.Count + 1, dataTable.Columns.Count);
            table.Borders.Enable = 1;
            table.Range.Font.Size = 12;

            // Заполнение заголовков таблицы
            for (int col = 0; col < dataTable.Columns.Count; col++)
            {
                table.Cell(1, col + 1).Range.Text = dataTable.Columns[col].ColumnName;
            }

            // Заполнение данных в таблицу
            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    table.Cell(row + 2, col + 1).Range.Text = dataTable.Rows[row][col].ToString();
                }
            }

            // Сохранение документа
            doc.SaveAs(filePath);

            // Закрытие документа и завершение приложения Word
            doc.Close();
            wordApp.Quit();
        }
        public static void FileSave(string name,List<string> students, List<DataTable> dataTables, string filePath)
        {
            Word.Application wordApp = new Word.Application();

            // Создание нового документа
            Word.Document doc = wordApp.Documents.Add();

            // Добавление заголовка и текущего времени
            Word.Paragraph header = doc.Content.Paragraphs.Add();
            header.Range.Text = "Таблицы оценок группы" + name;
            header.Range.Font.Size = 14;
            header.Range.Font.Bold = 1;
            header.Format.SpaceAfter = 10;
            header.Range.InsertParagraphAfter();

            Word.Paragraph currentTime = doc.Content.Paragraphs.Add();
            currentTime.Range.Text = "Дата и время: " + DateTime.Now.ToString();
            currentTime.Range.Font.Size = 12;
            currentTime.Range.Font.Italic = 1;
            currentTime.Format.SpaceAfter = 10;
            currentTime.Range.InsertParagraphAfter();

            header = doc.Content.Paragraphs.Add();
            header.Range.Text = "Таблицы оценок " + students[0];
            header.Range.Font.Size = 14;
            header.Range.Font.Bold = 1;
            header.Format.SpaceAfter = 10;
            header.Range.InsertParagraphAfter();
            // Создание таблицы Word и настройка стилей
            int i = 1;
            foreach (DataTable dataTable in dataTables)
            {
                
                if (dataTables.IndexOf(dataTable) > 0)
                {
                    Word.Paragraph separator = doc.Content.Paragraphs.Add();
                    separator.Range.Text = "=========================";
                    separator.Format.SpaceAfter = 10;
                    separator.Range.InsertParagraphAfter();
                    try
                    {
                        header = doc.Content.Paragraphs.Add();
                        header.Range.Text = "Таблицы оценок " + students[i];
                        header.Range.Font.Size = 14;
                        header.Range.Font.Bold = 1;
                        header.Format.SpaceAfter = 10;
                        header.Range.InsertParagraphAfter();
                    }
                    catch { return; }
                }

                // Создание таблицы для текущего DataTable
                Word.Table table = doc.Tables.Add(header.Range, dataTable.Rows.Count + 1, dataTable.Columns.Count);
                table.Borders.Enable = 1;
                table.Range.Font.Size = 12;

                // Заполнение заголовков таблицы
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    table.Cell(1, col + 1).Range.Text = dataTable.Columns[col].ColumnName;
                }

                // Заполнение данных в таблицу
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        table.Cell(row + 2, col + 1).Range.Text = dataTable.Rows[row][col].ToString();
                    }
                }
                i++;
            }

            // Сохранение документа
            doc.SaveAs(filePath);

            // Закрытие документа и завершение приложения Word
            doc.Close();
            wordApp.Quit();
        }
    }
}
