using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBot.IO
{
    public class SaveFile : NodeContainer
    {
        protected StreamWriter _streamWriter;
        protected StreamReader _streamReader;
        protected string _path;

        public SaveFile(string path)
        {
            _path = path;
            if (!File.Exists(_path))
                File.Create(_path).Close();
        }

        protected void OpenWrite()
        {
            var fs = File.Open(_path, FileMode.Append, FileAccess.Write);
            _streamWriter = new StreamWriter(fs);
        }

        protected void OpenRead()
        {
            var fs = File.Open(_path, FileMode.Open, FileAccess.Read);
            _streamReader = new StreamReader(fs);
        }

        protected void Close()
        {
            if (_streamWriter != null)
                _streamWriter.Close();
            if (_streamReader != null)
                _streamReader.Close();
        }

        public void Load()
        {
            OpenRead();
            Node node = LoadNode(_streamReader, "");
            foreach (KeyValuePair<string, Node> temp in node.Nodes)
                nodes.Add(temp.Key, temp.Value);
            Close();
        }

        protected Node LoadNode(StreamReader sr, string nodeName)
        {
            Node current = new Node(nodeName);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Replace("\t", "");
                if (line.Length > 2 && line[0] == '<' && line[1] != '/')
                {
                    string currentNodeName = line.Substring(1, line.Length - 2);
                    Node node = LoadNode(sr, currentNodeName);
                    current.AddNode(currentNodeName, node);                }
                else if (line.Length > 2 && line.Substring(0, 2).Equals("</"))
                    return current;
                else
                {
                    //Actual data entries
                    string key = "";
                    string value = "";

                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i].Equals(':'))
                        {
                            key = line.Substring(0, i);
                            value = line.Substring(i+2);
                            break;
                        }
                    }
                    current.AddNode(key, new Node(value));
                }
            }
            if (sr.ReadLine() == null)
                return current;
            throw new Exception("File \"" + _path + "\" is in incorrect format.");
        }

        public void Save()
        {
            if (File.Exists(_path))
                File.Delete(_path);
            OpenWrite();
            foreach (KeyValuePair<string, Node> temp in nodes)
                SaveNode(temp.Key, temp.Value);
            Close();
        }

        protected void SaveNode(string key, Node node, int currentlevel = 0)
        {
            if (node.HasNodes || node.Value.Equals(key))
            {
                for (int i = 0; i < currentlevel; i++)
                    _streamWriter.Write("\t"); //add tabs so it looks better!

                _streamWriter.WriteLine("<" + key + ">");
                foreach (KeyValuePair<string, Node> temp in node.Nodes)
                {
                    //for (int i = 0; i < currentlevel; i++)
                    //  _streamWriter.Write("\t"); //add tabs so it looks better!

                    SaveNode(temp.Key, temp.Value, currentlevel + 1);
                }
                for (int i = 0; i < currentlevel; i++)
                    _streamWriter.Write("\t"); //add tabs so it looks better!

                _streamWriter.WriteLine("</" + key + ">");
            }
            else
            {
                for (int i = 0; i < currentlevel; i++)
                    _streamWriter.Write("\t"); //add tabs so it looks better!
                _streamWriter.WriteLine(key + ": " + node.Value);
            }

        }
    }
}
