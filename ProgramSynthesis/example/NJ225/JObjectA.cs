#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
#if HAVE_INOTIFY_COLLECTION_CHANGED
using System.Collections.ObjectModel;
using System.Collections.Specialized;
#endif
using System.ComponentModel;
#if HAVE_DYNAMIC
using System.Dynamic;
using System.Linq.Expressions;
#endif
using System.IO;
using Newtonsoft.Json.Utilities;
using System.Globalization;
#if !HAVE_LINQ
using Newtonsoft.Json.Utilities.LinqBridge;
#else
using System.Linq;
#endif

namespace Newtonsoft.Json.Linq
{
    public new static JObject Parse(string json, JsonLoadSettings settings)
    {
        using (JsonReader reader = new JsonTextReader(new StringReader(json)))
        {
            JObject o = Load(reader, settings);

            while (reader.Read())
            {
                throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");
            }

            return o;
        }
    }
}