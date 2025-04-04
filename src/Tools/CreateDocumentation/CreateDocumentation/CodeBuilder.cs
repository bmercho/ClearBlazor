﻿using System.Text;

namespace CreateDocumentation
{
    public class CodeBuilder
    {
        private readonly StringBuilder _code;

        public CodeBuilder()
        {
            _code = new StringBuilder();
            IndentLevel = 0;
        }

        public string Code { get => _code.ToString(); }

        public int IndentLevel { get; set; }

        public void Add(string codeString)
        {
            Add(codeString, IndentLevel);
        }

        public void Add(string codeString, int indentLevel)
        {
            _code.Append(codeString.PadLeft(codeString.Length + (indentLevel * 4), ' '));
        }

        public void AddLine()
        {
            _code.Append('\n');
        }

        public void AddLine(string codeLine)
        {
            Add(codeLine);
            AddLine();
        }

        public void AddHeader()
        {
            AddLine("//-----------------------------------------------------------------------");
            AddLine("// This file is autogenerated by MudBlazor.Docs.Compiler");
            AddLine("// Any changes to this file will be overwritten on build");
            AddLine("// <auto-generated />");
            AddLine("//-----------------------------------------------------------------------");
            AddLine();
        }

        public override string ToString()
        {
            return _code.ToString();
        }
    }
}
