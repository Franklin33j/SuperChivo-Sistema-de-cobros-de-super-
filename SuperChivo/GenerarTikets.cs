using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//agregados
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data.SqlClient;

namespace SuperChivo
{
    class GenerarTikets
    {
        Random aleatorio = new Random();
        ListView nProductos;
        Document ticket = new Document(PageSize.A6,5,5,7,7);
        string iva, totalNeto, total, cajero;
        public GenerarTikets(ListView nProductos, string iva, string totalNeto, string total, string cajero)
        {
            this.nProductos = nProductos;
            this.iva = iva;
            this.totalNeto = totalNeto;
            this.total = total;
            this.cajero = cajero;
            Proceso();
        }

        public void Proceso()
        {
            //generar aleatorio para que los tikets no coicidan
            byte contador = 1;
            string anexo = "";
            while(contador < 6)
            {
                anexo += aleatorio.Next(0, 9);
                contador++;
            }
            string nombreArchivo = string.Format(@"Tickets\ticket{0}.pdf", anexo);
            PdfWriter.GetInstance(ticket, new FileStream(nombreArchivo, FileMode.Create));
            ticket.Open();
            //creacion de tipos de letras
            Font titulo = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK);
            Font titulo2 = new Font(Font.FontFamily.HELVETICA,8, Font.BOLD, BaseColor.BLACK);
            Font letra = new Font(Font.FontFamily.HELVETICA,8, Font.NORMAL, BaseColor.BLACK);
            ticket.Open();
            ticket.Add(new Paragraph("                      SuperChivo S.A de C.V",titulo));
            ticket.Add(new Paragraph("                                 Calle a San Ramon, Colonia La Flor", titulo2));
            ticket.Add(new Paragraph("                                 Autor:Franklin Guevara", letra));
            ticket.Add(new Paragraph($"                                 Cajero: {cajero}", letra));
            ticket.Add(new Paragraph($"                                       Impreso:{DateTime.Today.ToString()}", letra));
            ticket.Add(new Paragraph($"                                           Tiket n#:{anexo}", letra));

            ticket.Add(Chunk.NEWLINE);
            //creacion de tablas

            ticket.Add(CrearFilas("Producto","Cant.","Uni.","Desc.","Total",titulo2));

            for(byte a=0; a<nProductos.Items.Count; a++)
            {
               string produc= nProductos.Items[a].SubItems[1].Text; 
               string unita = nProductos.Items[a].SubItems[2].Text;
               string cant =  nProductos.Items[a].SubItems[3].Text;
               string desc= nProductos.Items[a].SubItems[4].Text;
            string tota = nProductos.Items[a].SubItems[5].Text;
                ticket.Add(CrearFilas(produc,cant,unita,desc,tota, letra));
            }

            ticket.Add(CrearFilas("", "", "", "Total:", total, letra));
            ticket.Add(CrearFilas("", "", "", "Iva:", iva, letra));
            ticket.Add(CrearFilas("", "", "", "Total N:", totalNeto, titulo2));
            ticket.Close();

        }
        public PdfPTable CrearFilas(string produc, string cant, string uni, string desc, string tota ,Font titulo)
        {
            PdfPTable tabla = new PdfPTable(5);
            tabla.WidthPercentage = 90;
            /*largo para las celdas, solo se puede modificando la tabla, genera error intentar desde las 
             * celdas. solo utiliza float, El metodo SetWidths genera el formatos(por porcentaje los valores
             * y deben coincidir con el parametro de tabla)
            */
            float [] largoFila = new float[] { 60f, 10f, 10f, 10f, 10f };
            tabla.SetWidths(largoFila);
            PdfPCell producto = new PdfPCell(new Phrase(produc, titulo));
            producto.BorderWidth = 0;
            producto.BorderWidthBottom = 0.75f;

            PdfPCell cantidad = new PdfPCell(new Phrase(cant, titulo));
            cantidad.BorderWidth = 0;
            cantidad.BorderWidthBottom = 0.75f;

            PdfPCell unitario = new PdfPCell(new Phrase(uni, titulo));
            unitario.BorderWidth = 0;
            unitario.BorderWidthBottom = 0.75f;

            PdfPCell total = new PdfPCell(new Phrase(tota, titulo));
            total.BorderWidth = 0;
            total.BorderWidthBottom = 0.75f;

            PdfPCell descuento = new PdfPCell(new Phrase(desc, titulo));
            descuento.BorderWidth = 0;
            descuento.BorderWidthBottom = 0.75f;
            tabla.AddCell(producto);
            tabla.AddCell(unitario);
            tabla.AddCell(cantidad);
            tabla.AddCell(descuento);
            tabla.AddCell(total);
            return tabla;
        }
    }
}
