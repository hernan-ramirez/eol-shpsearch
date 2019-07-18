using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Text;
using com.errepar.erreiusgestion;
using System.Net;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["k"]))
        {
            TextBoxBuscar.Text = Request.QueryString["k"];
            Buscar();
            divPaginacion.Visible = true;
        }
    }


    /// <summary>
    /// Metodos de botones
    /// Busqueda y paginacion
    /// </summary>
    #region Metodos de botones, Busqueda y paginacion
    protected void ButtonBuscar_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(TextBoxBuscar.Text))
        {
            LabelStartAt.Text = "1";
            Buscar();
            divPaginacion.Visible = true;
            LinkButtonAnterior.Visible = false;
        }
    }
    protected void LinkButtonSiguiente_Click(object sender, EventArgs e)
    {
        LabelStartAt.Text = (int.Parse(LabelStartAt.Text) + 10).ToString();
        Buscar();
        LinkButtonAnterior.Visible = true;
    }
    protected void LinkButtonAnterior_Click(object sender, EventArgs e)
    {
        if (LabelStartAt.Text != "1")
        {
            LabelStartAt.Text = (int.Parse(LabelStartAt.Text) - 10).ToString();
            Buscar();
            if (LabelStartAt.Text == "1")
            {
                LinkButtonAnterior.Visible = false;
            }
        }
    }
    #endregion


    /// <summary>
    /// Armo la consulta CAML
    /// </summary>
    protected String ArmarConsulta(string Consulta, string StartAt)
    {
        StringBuilder queryXml = new StringBuilder();

        queryXml.Append(@"<QueryPacket xmlns='urn:Microsoft.Search.Query' Revision='1000'>
            <Query>
                <Range>
                    <StartAt>" + StartAt + @"</StartAt>
                </Range>
                <Context>");

        //queryXml.Append("<QueryText language='es-AR' type='MSSQLFT'>");
        //queryXml.Append("Select Size, Rank, Path, FileName, owseoltitulo, Obra, Seccion, TipodeNorma, NumerodeNorma, Jurisdicción, Fecha, Organismo, FechaBO, parte, tribunal, sala, tipocontenido, Analisis FROM Scope() WHERE \"scope\"='EOL' ");
        //queryXml.Append("and CONTAINS('" + Consulta + "') ");
        queryXml.Append(@"<QueryText language='es-AR' type='STRING'>" + Consulta + @" AND scope:Erreius</QueryText>
                </Context>");

        queryXml.Append(@"<Properties>
                    <Property name='TipoContenido'></Property>
                    <Property name='TITLE'></Property>
                    <Property name='owsiusTitulo'></Property>
                    <Property name='FILENAME'></Property>
                    <Property name='Obra'></Property>
                    <Property name='Seccion'></Property>
                    <Property name='AreaDelDerecho'></Property>

                    <Property name='TipoDeNorma'></Property>
                    <Property name='NumeroDeNorma'></Property>
                    <Property name='Jurisdiccion'></Property>
                    <Property name='Organismo'></Property>

                    <Property name='Dictamen'></Property>

                    <Property name='Parte'></Property>
                    <Property name='Tribunal'></Property>
                    <Property name='Sala'></Property>

                    <Property name='Autores'></Property>
                    <Property name='Mes'></Property>
                    <Property name='Anio'></Property>

                    <Property name='Fecha'></Property>
                    <Property name='FechaBO'></Property>
                </Properties>");

        queryXml.Append(@"</Query>
        </QueryPacket>");

        return queryXml.ToString();
    }


    /// <summary>
    /// Ejecuta la busqueda y calcula la paginacion
    /// </summary>
    protected void Buscar()
    {
        String queryXml = ArmarConsulta(TextBoxBuscar.Text, LabelStartAt.Text);

        try
        {
            QueryService qs = new QueryService();
            qs.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["shpUsuario"], ConfigurationManager.AppSettings["shpPassword"], ConfigurationManager.AppSettings["shpDominio"]);
            //NetworkCredential networkCredential = CredentialCache.DefaultCredentials.GetCredential(new Uri(qs.Url), "NTLM");
            //qs.Credentials = networkCredential;

            qs.Url = "http://erreiusgestion.errepar.com/sitios/suscriptores/_vti_bin/Search.asmx";
            System.Data.DataSet queryResults = qs.QueryEx(queryXml);

            //GridViewResultados.DataSource = queryResults;
            //GridViewResultados.DataBind();

            RepeaterResultados.DataSource = queryResults;
            RepeaterResultados.DataBind();
        }
        catch (System.Web.Services.Protocols.SoapException ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.Message;
        }
    }


    /// <summary>
    /// Ejemplo de los valores que se esperan:
    /// 67769;#Impuestos|6287f92a-f372-4bf4-8136-1ba95e0c177c;#99379;#Buenos Aires|e6ecfe50-710d-4eec-b12c-de1722ad46ac
    /// </summary>
    #region Formateadores de datos
    public static string limpiarMetadatos(string texto, string separador)
    {
        if (!string.IsNullOrEmpty(texto) && texto.IndexOf(";#") != -1)
        {
            StringBuilder resultado = new StringBuilder();
            string[] sp = texto.Split(';');

            foreach (string i in sp)
            {
                if (i.IndexOf('|') != -1)
                {
                    resultado.Append(i.Substring(0, i.IndexOf('|')).Replace("#", "") + separador);
                }
            }
            return resultado.Remove(resultado.Length - separador.Length, separador.Length).ToString();
        }
        else { return texto; }
    }
    #endregion
}
