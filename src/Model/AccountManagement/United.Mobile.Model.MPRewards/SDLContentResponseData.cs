using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class SDLContentResponseData
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public SDLBody[] Body { get; set; }
        public SDLResult[] Results { get; set; }
        public object[] ErrorList { get; set; }
    }

    [Serializable()]
    public class SDLBody
    {
        public SDLPresentation presentation { get; set; }
        public SDLContent[] content { get; set; }
        public SDLMetadata metadata { get; set; }
        public SDLExtendedproperties extendedProperties { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Body { get; set; }
        public object Results { get; set; }
        public object Page { get; set; }
        public object PageLayout { get; set; }
        public object PageMetadata { get; set; }
        public object PageData { get; set; }
        public object Navigation { get; set; }
        public object ComponentPresentations { get; set; }
        public SDLErrorlist[] ErrorList { get; set; }
        public string LastCallDateTime { get; set; }
        public string CallTime { get; set; }
        public object SessionId { get; set; }
    }

    [Serializable()]
    public class SDLPresentation
    {
        public string view { get; set; }
        public string designName { get; set; }
        public string designId { get; set; }
    }

    [Serializable()]
    public class SDLMetadata
    {
        public string nav_title { get; set; }
        public string show_sitemap { get; set; }
        public string show_in_breadcrumb { get; set; }
        public string include_in_atw { get; set; }
    }

    [Serializable()]
    public class SDLExtendedproperties
    {
        public string fileName { get; set; }
    }

    [Serializable()]
    public class SDLContent
    {
        public SDLPresentation1 presentation { get; set; }
        public SDLContent1 content { get; set; }
        public SDLMetadata1 metadata { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    [Serializable()]
    public class SDLPresentation1
    {
        public string view { get; set; }
        public string header_tag { get; set; }
        public string header_tag_increment { get; set; }
        public string designId { get; set; }
        public string designName { get; set; }
    }

    [Serializable()]
    public class SDLContent1
    {
        public string key { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string subtitle { get; set; }
        public SDLSection[] sections { get; set; }
        public string src { get; set; }
        public SDLImageSource image_source { get; set; }
        public List<SDLImageAsset> image_assets { get; set; }
    }
    public class SDLImageAsset
    {
        public SDLContent1 content { get; set; }

    }

    public class SDLImageSource
    {
        public SDLContent1 content { get; set; }
    }

    [Serializable()]
    public class SDLSection
    {
        public SDLContent2 content { get; set; }
        public SDLMetadata1 metadata { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

    [Serializable()]

    public class SDLContent2
    {
        public string key { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }

    [Serializable()]
    public class SDLMetadata1
    {
        public string contentModel { get; set; }
    }

    [Serializable()]
    public class SDLErrorlist
    {
        public int Code { get; set; }
        public object Message { get; set; }
        public object FieldName { get; set; }
    }

}
