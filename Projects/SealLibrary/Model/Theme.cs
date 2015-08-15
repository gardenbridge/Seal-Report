﻿//
// Copyright (c) Seal Report, Eric Pfirsch (sealreport@gmail.com), http://www.sealreport.org.
// This code is licensed under GNU General Public License version 3, http://www.gnu.org/licenses/gpl-3.0.en.html.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Seal.Helpers;
using System.Drawing;
using RazorEngine;
using RazorEngine.Templating;

namespace Seal.Model
{
    public class Theme
    {
        string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        bool _isDefault = false;
        public bool IsDefault
        {
            get { return _isDefault; }
            set { _isDefault = value; }
        }

        List<Parameter> _values = new List<Parameter>();
        public List<Parameter> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        public static List<Theme> LoadThemes(string folder)
        {
            List<Theme> result = new List<Theme>();
            //Templates
            foreach (var path in Directory.GetFiles(folder, "*.cshtml"))
            {
                Theme theme = new Theme() { Name = Path.GetFileNameWithoutExtension(path) };
                theme.FilePath = path;
                theme.Parse();
                result.Add(theme);
            }
            return result;
        }

        string _error = "";
        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }


        public void Clear()
        {
            _values.Clear();
        }

        public void Parse()
        {
            try
            {
                Clear();
                Razor.Parse(File.ReadAllText(FilePath), this);
            }
            catch (TemplateCompilationException ex)
            {
                _error = Helper.GetExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                _error = string.Format("Unexpected error got when parsing theme configuration.\r\n{0}", ex.Message);
                if (ex.InnerException != null) _error += "\r\n" + ex.InnerException.Message;
            }
        }

    }
}