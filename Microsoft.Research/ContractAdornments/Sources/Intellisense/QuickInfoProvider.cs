// CodeContracts
// 
// Copyright (c) Microsoft Corporation
// 
// All rights reserved. 
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.Diagnostics.Contracts;

namespace ContractAdornments {
  [Export(typeof(IQuickInfoSourceProvider))]
  [Name("Code Contracts QuickInfoSourceProvider")]
  [Order(After = "Default Quick Info Source")]
  [Order(After = "Shim Quick Info Source")]
  [ContentType("Code")]
  class QuickInfoSourceProvider : IQuickInfoSourceProvider {
    public IQuickInfoSource TryCreateQuickInfoSource(ITextBuffer textBuffer) {
      Contract.Assume(textBuffer != null);

      if (VSServiceProvider.Current == null || VSServiceProvider.Current.ExtensionHasFailed) {
        //If the VSServiceProvider is not initialize, we can't do anything.
        return null;
      }

      return VSServiceProvider.Current.Logger.PublicEntry<IQuickInfoSource>(() => {
        if (VSServiceProvider.Current.VSOptionsPage != null && !VSServiceProvider.Current.VSOptionsPage.QuickInfo)
          return null;

        TextViewTracker textViewTracker;
        if (TextViewTracker.TryGetTextViewTracker(textBuffer, out textViewTracker)) {
          return new QuickInfoSource(textBuffer, textViewTracker);
        } else
          return null;
      }, "TryCreateQuickInfoSession");
    }
  }
}
