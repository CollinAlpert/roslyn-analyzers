// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.UnitTests;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.ApiDesignGuidelines.Analyzers.UnitTests
{
    public class PropertiesShouldNotReturnArraysTests : DiagnosticAnalyzerTestBase
    {
        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            //return new PropertiesShouldNotReturnArraysAnalyzer();
            return new PropertiesShouldNotReturnArraysAnalyzer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new PropertiesShouldNotReturnArraysAnalyzer();
        }

        [Fact]
        public void TestCSharpPropertiesShouldNotReturnArraysWarning1()
        {
            //Verify return type is array, warning...
            VerifyCSharp(@"
    public class Book
    {
        private string[] _Pages;
        public string[] Pages
        {
            get { return _Pages; }
        }
    }
 ", CreateCSharpResult(5, 25));
        }

        [Fact]
        public void TestCSharpPropertiesShouldNotReturnArraysNoWarning1()
        {
            //Verify if property is override, then no warning...
            VerifyCSharp(@"
    public class Book
        public override string[] Pages
        {
            get { return _Pages; }
        }
    }
");
        }

        [Fact]
        public void TestCSharpPropertiesShouldNotReturnArraysNoWarning2()
        {
            //No warning if property definition has no outside visibility
            VerifyCSharp(@"
    private class Book
    {
        public string[] Pages
        {
            get { return _Pages; }
        }
    }
");
        }

        [Fact]
        public void TestCSharpPropertiesShouldNotReturnArraysNoWarning3()
        {
            //Attributes can contain properties that return arrays
            VerifyCSharp(@"
    public class Book : System.Attribute
    {
        public string[] Pages 
        {
            get { return _Pages; }
        }
    }
");
        }

        [Fact]
        public void TestBasicPropertiesShouldNotReturnArraysWarning1()
        {
            //Display warning for property return type is Array
            VerifyBasic(@"
    Public Class Book
        Private _Pages As String()
        Public ReadOnly Property Pages() As String()
            Get
                Return _Pages
            End Get
        End Property
    End Class", CreateBasicResult(4, 34));
        }

        [Fact]
        public void TestBasicPropertiesShouldNotReturnArraysNoWarning1()
        {
            //No warning if property definition is override
            VerifyBasic(@"
    Public Class Book
        Private _Pages As String()
        Public Override Property Pages() As String()
            Get
                Return _Pages
            End Get
        End Property
    End Class");
        }

        [Fact]
        public void TestBasicPropertiesShouldNotReturnArraysWarning2()
        {
            //No warning if property has no outside visibility
            VerifyBasic(@"
    Private Class Book
        Private _Pages As String()
        Public ReadOnly Property Pages() As String()
            Get
                Return _Pages
            End Get
        End Property
    End Class");
        }

        private static DiagnosticResult CreateCSharpResult(int line, int col)
        {
            return GetCSharpResultAt(line, col, PropertiesShouldNotReturnArraysAnalyzer.RuleId, MicrosoftApiDesignGuidelinesAnalyzersResources.PropertiesShouldNotReturnArraysMessage);
        }

        private static DiagnosticResult CreateBasicResult(int line, int col)
        {
            return GetBasicResultAt(line, col, PropertiesShouldNotReturnArraysAnalyzer.RuleId, MicrosoftApiDesignGuidelinesAnalyzersResources.PropertiesShouldNotReturnArraysMessage);
        }
    }

}