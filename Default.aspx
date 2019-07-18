<%@ Page Debug="true" Title="ERREIUS - Búsqueda" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="resultados.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="Server">
    <div id="ContentBuscador" align="center">
        <asp:TextBox ID="TextBoxBuscar" runat="server" OnTextChanged="ButtonBuscar_Click" Width="290px" CssClass="BuscadorPalabra"></asp:TextBox>
        <asp:Button ID="ButtonBuscar" runat="server" OnClick="ButtonBuscar_Click" Text="Buscar" CssClass="botonBuscar" />
    </div>
    <p id="ListadoLinea" />
    <div id="ContentListado">
        <asp:GridView ID="GridViewResultados" runat="server">
        </asp:GridView>
        <asp:Repeater ID="RepeaterResultados" runat="server">
            <SeparatorTemplate>
                <p id="ListadoLinea" />
            </SeparatorTemplate>
            <ItemTemplate>
                <div id="divRegistro" runat="server">
                    <!-- http://erreius.errepar.com/sitios/Erreius/_layouts/DocumentoErreius.aspx?id=/sitios/erreius/docs/20100921040827389.docx&DefaultItemOpen=0&Source=javascript:this.close();&DefaultItemOpen=1&consulta=lucro&TipoDeContenido=Jurisprudencia -->
                    <a class="shpLink" target="_blank" href='http://www.errepar.com/nova/nova_modulos/suscriptoresnet/enlace.aspx?urlshp=http://erreius.errepar.com/sitios/erreius/docs/<%# DataBinder.Eval(Container, "DataItem.FILENAME") %>'>
                        <p class="Cerrado" id="ListadoTitulo">
                            <%# limpiarMetadatos(DataBinder.Eval(Container, "DataItem.Jurisdiccion").ToString(), ". ")%>
                            <%# limpiarMetadatos(DataBinder.Eval(Container, "DataItem.Seccion").ToString(), ". ")%>
                        </p>
                        <p id="ListadoSubtituloLink">
                            <%# limpiarMetadatos(DataBinder.Eval(Container, "DataItem.TipoDeNorma").ToString(), ". ")%>
                            <%# DataBinder.Eval(Container, "DataItem.NumeroDeNorma")%>
                            <%# DataBinder.Eval(Container, "DataItem.FechaBO", "BO: {0:d}")%>
                        </p>
                        <p class="LinkA">
                            <%# DataBinder.Eval(Container, "DataItem.owsiusTitulo").ToString().Replace("string;#", "").Replace("float;#0", "")%>
                        </p>
                        <p id="ListadoDescripcion">
                            <%# DataBinder.Eval(Container, "DataItem.Parte")%>
                            <%# limpiarMetadatos(DataBinder.Eval(Container, "DataItem.Tribunal").ToString(), ". ")%>
                            <%# limpiarMetadatos(DataBinder.Eval(Container, "DataItem.Sala", "Sala: {0}").ToString(), ". ")%>
                            <%# DataBinder.Eval(Container, "DataItem.Mes")%>
                            <%# DataBinder.Eval(Container, "DataItem.Anio")%>
                            <%# DataBinder.Eval(Container, "DataItem.Fecha", "{0:d}")%>
                        </p>
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblError" runat="server" CssClass="shpVacio" Visible="false">No se encontraron resultados en su búsqueda</asp:Label>
    </div>
    <div id="ContentListadoPie">
        <div id="divPaginacion" runat="server" align="right" visible="false">
            <asp:LinkButton CssClass="anterior" ID="LinkButtonAnterior" runat="server" OnClick="LinkButtonAnterior_Click" Visible="false">Anterior</asp:LinkButton>
            |<asp:Label ID="LabelStartAt" runat="server" Text="1" Visible="false"></asp:Label>|
            <asp:LinkButton CssClass="siguiente" ID="LinkButtonSiguiente" runat="server" OnClick="LinkButtonSiguiente_Click">Siguiente</asp:LinkButton>
        </div>
    </div>
</asp:Content>
