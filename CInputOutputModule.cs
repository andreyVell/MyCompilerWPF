﻿using System;
using System.Collections.Generic;

namespace MyCompilerWPF
{
    class СInputOutputModule
    {
        private List<CError> errorList;
        private string buffer = string.Empty;
        private ushort curLinePos;
        private ushort curCharPos;
        private string[] parsedInput;
        public СInputOutputModule(string code)
        {
            parsedInput = code.Split('\n');
            curLinePos = 0;
            curCharPos = 0;
            errorList = new List<CError>();
    }
        public char getNextLetter()
        {
            if (parsedInput.Length > curLinePos || buffer.Length > curCharPos)
            {
                if (string.IsNullOrEmpty(buffer) || (curCharPos >= buffer.Length))
                    updateTheBuffer();
                if (buffer != "")
                    return buffer[curCharPos++];
                else return ' ';
            }
            else
            {
                if (errorList.Count > 0)
                    throw new Exception("Find some errors!");
                else
                    throw new Exception("Done");
            }
        }
        public void error(string name)//add new error to our errorList
        {
            CError newError = new CError(name, (ushort)(curLinePos), (ushort)(curCharPos));
            errorList.Add(newError);
        }
        public string errorOutput()//get our code with marked errors
        {
            string errorsOut=string.Empty;
            for (int i = 0; i < parsedInput.Length; i++)
            {
                errorsOut += parsedInput[i];
                foreach (CError curError in errorList)
                    if (curError.lineContainError(i))
                        errorsOut += curError.getErrorInfo();
            }
            return errorsOut;
        }
        private void updateTheBuffer() //start to analyse new line
        {
            buffer = parsedInput[curLinePos++];
            curCharPos = 0;
        }
    }
}
