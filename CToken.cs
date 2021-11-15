using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompilerWPF
{
    class CToken
    {
        enum ETokenType
        {
            ttOper,
            ttIdent,
            ttValue
        }
        enum EOperator
        {

        }
        enum EValType
        {
            vtInt = 0x1,
            vtReal = 0x2,
            vtString = 0x4,
            vtChar = 0x8
        }
    }
}
