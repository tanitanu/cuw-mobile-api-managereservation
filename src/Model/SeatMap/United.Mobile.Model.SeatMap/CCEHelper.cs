using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using United.Service.Presentation.PersonalizationModel;
using United.Utility;


namespace United.Mobile.Model.SeatMap
{
    public class CCEHelper
    {
        private readonly ILogger logger;
        public IConfiguration _configuration { get; }

        public CCEHelper(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger;
            _configuration = configuration;
        }

        public CCEHelper()
        { }

        public bool IsApplicationVersionGreaterorEqual(string msgKey, int applicationID, string appVersion, bool isGeneralCarousal)
        {
            bool ValidTFAVersion = false;
            string nonTFAVersion = string.Empty;
            if (applicationID == 1)
            {
                if (isGeneralCarousal)
                {
                    var dictionary_iOS = new Dictionary<string, string>();
                    if (dictionary_iOS.ContainsKey(msgKey))
                    {
                        nonTFAVersion = dictionary_iOS[msgKey];
                    }
                }
                else
                {
                    nonTFAVersion = _configuration["iOSMilePlayClear"].ToString() ?? "";
                }
            }
            else
            {
                if (isGeneralCarousal)
                {
                    var dictionary_andriod = new Dictionary<string, string>();
                    if (dictionary_andriod.ContainsKey(msgKey))
                    {
                        nonTFAVersion = dictionary_andriod[msgKey];
                    }
                }
                else
                {
                    nonTFAVersion = _configuration["androidMilePlayClear"].ToString() ?? "";
                }
            }

            if (!string.IsNullOrEmpty(appVersion))
            {
                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());
                if (appVersion != nonTFAVersion)
                {
                    ValidTFAVersion = isVersion1Greater(appVersion, nonTFAVersion);
                }
                else
                    ValidTFAVersion = true;
            }

            return ValidTFAVersion;
        }

        public static bool isVersion1Greater(string version1, string version2)
        {
            return SeperatedVersionCompareCommonCode(version1, version2);
        }

        public static bool SeperatedVersionCompareCommonCode(string version1, string version2)
        {
            try
            {
                string[] version1Arr = version1.Trim().Split('.');
                string[] version2Arr = version2.Trim().Split('.');

                if (Convert.ToInt32(version1Arr[0]) > Convert.ToInt32(version2Arr[0]))
                {
                    return true;
                }
                else if (Convert.ToInt32(version1Arr[0]) == Convert.ToInt32(version2Arr[0]))
                {
                    if (Convert.ToInt32(version1Arr[1]) > Convert.ToInt32(version2Arr[1]))
                    {
                        return true;
                    }
                    else if (Convert.ToInt32(version1Arr[1]) == Convert.ToInt32(version2Arr[1]))
                    {
                        if (Convert.ToInt32(version1Arr[2]) > Convert.ToInt32(version2Arr[2]))
                        {
                            return true;
                        }
                        else if (Convert.ToInt32(version1Arr[2]) == Convert.ToInt32(version2Arr[2]))
                        {
                            if (!string.IsNullOrEmpty(version1Arr[3]) && !string.IsNullOrEmpty(version2Arr[3]))
                            {
                                if (Convert.ToInt32(version1Arr[3]) > Convert.ToInt32(version2Arr[3]))
                                {
                                    return true;
                                }
                            }

                        }
                    }
                }

            }
            catch
            {
            }
            return false;
        }

        public static bool IsApplicationVersionGreaterorEqual(string applicationVersion, string configVersion)
        {
            bool isGreaterOrEqual = false;

            if (!string.IsNullOrEmpty(applicationVersion) && !string.IsNullOrEmpty(configVersion))
            {
                if (applicationVersion != configVersion)
                {
                    isGreaterOrEqual = isVersion1Greater(applicationVersion, configVersion);
                }
                else
                {
                    isGreaterOrEqual = true;
                }
            }

            return isGreaterOrEqual;
        }

        public CCERequest AddExtraComponentsForCCErequest(UIRequest<CCERequest> request)
        {
            string pipedComponents = _configuration["CCEExtraComponantsToLoad"] != null ? _configuration["CCEExtraComponantsToLoad"].ToString() : string.Empty;

            List<string> components = StringUtil.PipeDelimitedtoStringList(pipedComponents);
            List<string> componentsToAdd = new List<string>();

            foreach (var component in components)
            {
                if (!request.Data.ComponentsToLoad.Any(x => x.ToUpper() == component.ToUpper()))
                {
                    componentsToAdd.Add(component);
                }

                if (component.Equals("HomePageOverlay", StringComparison.OrdinalIgnoreCase))
                {
                    componentsToAdd.Remove(component);
                }
            }

            request.Data.ComponentsToLoad.AddRange(componentsToAdd);

            return request.Data;
        }

        public UIRequest<CCERequest> AddExtraComponentsForTrcRequest(UIRequest<CCERequest> request)
        {
            string pipedComponents = _configuration["TRC_ExtraComponentsToLoad"] != null ? _configuration["TRC_ExtraComponentsToLoad"].ToString() : string.Empty;

            List<string> components = StringUtil.PipeDelimitedtoStringList(pipedComponents);

            List<string> componentsToAdd = new List<string>();

            foreach (var component in components)
            {
                if (!request.Data.ComponentsToLoad.Any(x => x.ToUpper() == component.ToUpper()))
                {
                    componentsToAdd.Add(component);
                }
            }

            if (componentsToAdd.Any())
            {
                request.Data.ComponentsToLoad.Remove("TravelCenterRequirements");
            }

            request.Data.ComponentsToLoad.AddRange(componentsToAdd);

            return request;
        }

        public string GetPipeSaparatedLinks(Collection<ContextualLinkBase> links)
        {
            List<string> urls = new List<string>();

            if (links != null && links.Any())
            {
                foreach (var link in links)
                {
                    if (!string.IsNullOrEmpty(link.Link))
                    {
                        urls.Add(link.Link);
                    }
                }
            }

            return StringUtil.GetPipeDelimitedIdsString(urls);
        }

        public string GetPipeSaparatedLinkTexts(Collection<ContextualLinkBase> links)
        {
            List<string> urls = new List<string>();

            if (links != null && links.Any())
            {
                foreach (var link in links)
                {
                    if (!string.IsNullOrEmpty(link.LinkText))
                    {
                        urls.Add(link.LinkText);
                    }
                }
            }

            return GetPipeDelimitedIdsString(urls);
        }
        
        public static string GetPipeDelimitedIdsString(List<string> values)
        {
            StringBuilder pipeDelimitedIds = new StringBuilder();
            object lockObj = new object();
            values.ForEach(x =>
            {
                lock (lockObj)
                {
                    if (pipeDelimitedIds.Length != 0)
                    {
                        pipeDelimitedIds.Append("|");
                    }
                    pipeDelimitedIds.Append(x.ToString());
                }
            });

            return pipeDelimitedIds.ToString();
        }

        public string GetPipeSaparatedLinks(Collection<ContextualLinkType> links)
        {
            Collection<ContextualLinkBase> baseLinks = new Collection<ContextualLinkBase>();
            links.ToList().ForEach(x =>
            {
                baseLinks.Add(x as ContextualLinkBase);
            });

            return GetPipeSaparatedLinks(baseLinks);
        }

        public string GetPipeSaparatedLinkTexts(Collection<ContextualLinkType> links)
        {
            Collection<ContextualLinkBase> baseLinks = new Collection<ContextualLinkBase>();
            links.ToList().ForEach(x =>
            {
                baseLinks.Add(x as ContextualLinkBase);
            });

            return GetPipeSaparatedLinkTexts(baseLinks);
        }

        public string GetColors(string RGBHexValue, string alertType)
        {
            string resultColor = string.Empty;
            if (!string.IsNullOrEmpty(RGBHexValue))
            {
                string[] result = RGBHexValue.Split('|');
                for (int i = 0; i < result.Count(); i++)
                {
                    string[] UIColors = result[i].Split('~');

                    if (UIColors[0] == alertType)
                    {
                        resultColor = UIColors[1];
                    }
                }
            }
            return resultColor;
        }

        public string GetSharesLastName(string reqLastName)
        {
            //char[] charsToTrim = {' ', '-' };
            //string lastname = reqLastName.Trim(charsToTrim);

            string lastname = reqLastName.Replace(" ", "").Replace("-", "");

            return lastname;
        }

        public string GetValidDocExtension(string uploadedDoc)
        {
            var docInBytes = Convert.FromBase64String(uploadedDoc);
            var hex = BitConverter.ToString(docInBytes.Take(8).ToArray()).ToUpper();

            if (hex != null)
            {
                if (hex.StartsWith(Constants.JPG_JPEGDocumentSignature))
                {
                    return Constants.JPG_JPEGDocumentExtension;
                }
                else if (hex.Contains(Constants.PDFDocumentSignature))
                {
                    return Constants.PDFDocumentExtension;
                }
                else if (hex.StartsWith(Constants.PNGDocumentSignature))
                {
                    return Constants.PNGDocumentExtension;
                }
                else if (hex.StartsWith(Constants.HEICDocumentSignature))
                {
                    return Constants.HEICDocumentExtension;
                }
                else
                    logger.LogWarning("Hex does not match any accepted doc types. Document in {Bytes}/{Hex}", docInBytes.Take(8).ToArray(), hex);
                return null;
            }

            logger.LogWarning("Hex value is null for Document {Document}", uploadedDoc.Substring(0, 16));
            return string.Empty;

        }
    }
}
