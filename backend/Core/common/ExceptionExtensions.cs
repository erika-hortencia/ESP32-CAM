/*
 * File: ExceptionExtensions.cs
 * paperless-core
 * Author: Jorge Felix (jfelix@vonbraunlabs.com)
 * Modified by: Mateus Penteado (mateus.penteado@vonbraunlabs.com.br)
 * Last Update: 12/05/2022 01:59pm
 * -----
 * Copyright 2022 - CPA Wernher von Braun
 */
using System;

namespace backend.Common
{
    /// <summary>
    /// Helper class to get inner original exception trace.
    /// </summary>
    public static class ExceptionExtensions
    {
        public static Exception GetOriginalException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetOriginalException();
        }
    }
}