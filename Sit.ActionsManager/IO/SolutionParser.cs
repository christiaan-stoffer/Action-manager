using System;

namespace Sit.ActionsManager.IO
{
    public class SolutionParser
    {
        private string _solutionFilePath;

        private const string XsdResourcePath = "Sit.ActionsManager.Xsd.solution.xsd";

        public SolutionParser(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException();
            }

            _solutionFilePath = path;
        }
    }
}