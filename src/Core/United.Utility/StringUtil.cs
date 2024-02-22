using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Utility
{
    public static class StringUtil
    {
        public static List<string> CommaDelimitedtoStringList(string commaDelimitedString)
        {
            List<string> listOfStrings = new List<string>();
            if (!string.IsNullOrEmpty(commaDelimitedString))
            {
                listOfStrings = commaDelimitedString.Split(',').ToList<string>();
            }
            return listOfStrings;
        }

        public static List<string> AtDelimitedtoStringList(string atDelimitedString)
        {
            List<string> listOfStrings = new List<string>();
            if (!string.IsNullOrEmpty(atDelimitedString))
            {
                listOfStrings = atDelimitedString.Split('@').ToList<string>();
            }
            return listOfStrings;
        }

        public static List<Uri> CommaDelimitedToUriList(string commaDelimitedString)
        {
            List<Uri> listOfUris = new List<Uri>();
            if (!string.IsNullOrEmpty(commaDelimitedString))
            {
                listOfUris = CommaDelimitedtoStringList(commaDelimitedString).Select(x => new Uri(x)).ToList<Uri>();
            }
            return listOfUris;
        }

        public static string ToCommaDelimitedString(List<string> entries)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entry in entries)
            {
                sb.Append(entry);
                sb.Append(",");
            }
            string recordLocators = sb.ToString().TrimEnd(new char[] { ',' });
            return recordLocators;
        }

        public static List<string> PipeDelimitedtoStringList(string pipeDelimitedString)
        {
            List<string> listOfStrings = new List<string>();
            if (!string.IsNullOrEmpty(pipeDelimitedString))
            {
                listOfStrings = pipeDelimitedString.Split('|').ToList<string>();
            }
            return listOfStrings;
        }

        public static List<int> PipeDelimitedtoIntList(string pipeDelimitedString)
        {
            List<int> listOfInts = new List<int>();
            if (!string.IsNullOrEmpty(pipeDelimitedString))
            {
                var listOfString = pipeDelimitedString.Split('|').ToList<string>();

                listOfString.ForEach(s =>
                {
                    listOfInts.Add(Convert.ToInt32(s));
                });
            }
            return listOfInts;
        }

        public static string GetPipeDelimitedIdsString(List<string> values)
        {
            StringBuilder pipeDelimitedIds = new StringBuilder();
            object lockObj = new object();
            lock (lockObj)
            {
                values.ForEach(x =>
                {
                    if (pipeDelimitedIds.Length != 0)
                    {
                        pipeDelimitedIds.Append("|");
                    }
                    pipeDelimitedIds.Append(x.ToString());
                });
                return pipeDelimitedIds.ToString();
            }
        }

        public static bool PaxIdTryParse(string paxIdStr, out int surnameNumber, out int firstNameNumber)
        {
            surnameNumber = 0;
            firstNameNumber = 0;

            var paxIdentifier = paxIdStr?.Split('.');
            if (paxIdentifier == null || paxIdentifier.Length != 2) { return false; }

            var hasSNN = Int32.TryParse(paxIdentifier[0], out surnameNumber);
            var hasFNN = Int32.TryParse(paxIdentifier[1], out firstNameNumber);

            return hasFNN && hasSNN;
        }
    }
}
