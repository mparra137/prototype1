using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using DinkToPdf;
using DinkToPdf.Contracts;
using DinkToPdf.EventDefinitions;
using Proto1.Application.DTOs;


namespace Proto1.API.Helpers;

public class Util : IUtil
{
    private readonly IWebHostEnvironment hostEnvironment;
    private readonly IConverter converter;

    public Util(IWebHostEnvironment hostEnvironment, IConverter converter)
    {
        this.converter = converter;
        this.hostEnvironment = hostEnvironment;        
    }

    public async Task createTxtFile(string text){
        try
        {
            string fileName = "teste.txt";
            string fileAbsName = Path.Combine(hostEnvironment.ContentRootPath, @"Resources/Files", fileName);

            StreamWriter sw = new StreamWriter(fileAbsName, true, Encoding.UTF8);
            await sw.WriteLineAsync(text);
            
            sw.Close();
            

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Byte[]> createPdfFile(List<PessoaDto> lista) {           

        string html = await reportDatainHTML(lista);

        return GeneratePDF(html);        
    }


    private Byte[] GeneratePDF(string html){
        string fileName = $"ListaPessoas{DateTime.UtcNow.ToLocalTime().ToString("yyyyMMddHHmmssfff")}.pdf";//"teste.pdf";
        string fileAbsName = Path.Combine(hostEnvironment.ContentRootPath, @"Resources/Files", fileName);
        

        GlobalSettings globalSettings = new GlobalSettings();
        globalSettings.ColorMode = ColorMode.Color;
        globalSettings.Orientation = Orientation.Portrait;
        globalSettings.PaperSize = PaperKind.A4;
        globalSettings.Margins = new MarginSettings{Top = 25, Bottom = 25};
        ObjectSettings objectSettings = new ObjectSettings();
        objectSettings.PagesCount = true;
        objectSettings.HtmlContent = html;
        WebSettings webSettings = new WebSettings();
        webSettings.DefaultEncoding = "utf-8";
        HeaderSettings headerSettings = new HeaderSettings();
        headerSettings.FontSize = 9;
        headerSettings.FontName = "Arial";
        headerSettings.Right = "Page [page] of [toPage]";
        headerSettings.Line = true;        
        FooterSettings footerSettings = new FooterSettings();
        footerSettings.FontSize = 12;
        footerSettings.FontName = "Arial";
        footerSettings.Center = "This is for demonstration purposes only";
        footerSettings.Line = true;
        objectSettings.HeaderSettings = headerSettings;
        objectSettings.FooterSettings = footerSettings;
        objectSettings.WebSettings = webSettings;
        HtmlToPdfDocument htmlToPdfDocument = new HtmlToPdfDocument(){
            GlobalSettings = globalSettings,
            Objects = {objectSettings}
        };
        try{
            var pdfFile = converter.Convert(htmlToPdfDocument); 
            //using(var fileStream = new FileStream(fileAbsName, FileMode.CreateNew)){
            //    await fileStream.WriteAsync(pdfFile);
            //}
            return pdfFile;
        } catch (Exception ex){
            throw new Exception(ex.Message);
        }        
    }


    private async Task<string> reportDatainHTML(List<PessoaDto> lista){
        string template = Path.Combine(hostEnvironment.ContentRootPath, @"Resources/Template", "report.html");    
        string html = await File.ReadAllTextAsync(template);        

        StringBuilder headerLine = new StringBuilder();
        headerLine.Append("<tr>");
        headerLine.Append("<th>Nome</th>");
        headerLine.Append("<th>CPF</th>");
        headerLine.Append("<th>Endereço</th>");
        headerLine.Append("</tr>");

        html = html.Replace("{{tableHeader}}", headerLine.ToString());

        StringBuilder rows = new StringBuilder();

        foreach (var pessoa in lista)
        {
            rows.Append("<tr>");
            rows.Append($"<td>{pessoa.Nome}</td>");
            rows.Append($"<td>{pessoa.CPF}</td>");
            rows.Append($"<td>{pessoa.Endereco}, {pessoa.Numero}, {pessoa.Bairro}, {pessoa.Cidade} - {pessoa.UF}</td>");
            rows.Append("</tr>");  
            rows.AppendLine();    
        }   

        html = html.Replace("{{tableLines}}", rows.ToString());   

        html = html.Replace("{{reportTitle}}", "Lista de pessoas cadastradas");     

        //await createTxtFile(html);

        return html;
    }
}
