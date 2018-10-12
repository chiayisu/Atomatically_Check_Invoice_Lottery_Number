using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;


class ReadWeb
{
    public static HttpWebResponse Request(string url)
    {
        HttpWebRequest request=null;
        HttpWebResponse response = null;
        try
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
        }
        catch(WebException ex)
        {
             
        }
        return response;
    }

    public static string getHTML(HttpWebResponse webResponse, Encoding encoding)
    {
        string html = "";
        bool isNull = IsNull(webResponse);
        if (!isNull)
        {
            if (checkStatus(webResponse))
            {
                html = ReadHTML(webResponse, encoding);
            }
        }
        return html;
    }

    public static bool IsNull(HttpWebResponse webResponse)
    {
        if (webResponse == null)
            return true;
        return false;
    }

    public static bool checkStatus(HttpWebResponse webResponse)
    {
        if (webResponse.StatusCode == HttpStatusCode.OK)
            return true;
        return false;
    }

    public static string ReadHTML(HttpWebResponse webResponse, Encoding encoding)
    {
        Stream receiveStream = webResponse.GetResponseStream();
        StreamReader readStream = null;
        string htmlData;
        readStream = new StreamReader(receiveStream, encoding);
        htmlData = readStream.ReadToEnd();
        receiveStream.Close();
        readStream.Close();
        return htmlData;
    }
}

class HTMLParsing
{
    public static HtmlDocument LoadData(string html)
    {
        var document = new HtmlAgilityPack.HtmlDocument();
        document.LoadHtml(html);
        return document;
    }

    public static HtmlNode getElement(HtmlDocument doc, string id)
    {
        var element = doc.GetElementbyId(id);
        return element;
    }

    public static List<HtmlNode> getNodeList(HtmlNode node)
    {
        return node.ChildNodes.ToList();
    }

    public static List<HtmlAttribute> getAttributeList(List<HtmlNode> nodeList, int index)
    {
        return nodeList[index].Attributes.ToList();
    }

    public static HtmlNode getOwnerNode(List<HtmlAttribute> attributeList, int index)
    {
        return attributeList[index].OwnerNode;
    }
}
