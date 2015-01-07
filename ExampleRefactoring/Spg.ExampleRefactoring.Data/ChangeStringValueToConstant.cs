using System;
using System.Collections.Generic;

namespace Spg.ExampleRefactoring.Data
{
    /// <summary>
    /// Change string value to constant
    /// </summary>
    public class ChangeStringValueToConstant : ExampleCommand
    {
        /// <summary>
        /// Return the train data set.
        /// </summary>
        /// <returns>List of examples</returns>
        public override List<Tuple<String, String>> Train()
        {
            List<Tuple<String, String>> tuples = new List<Tuple<string, string>>();

            String input01 =
@"internal static readonly DiagnosticDescriptor UseEmptyEnumerableRule = new DiagnosticDescriptor(
              ""RS0001"",
              RoslynDiagnosticsResources.UseEmptyEnumerableDescription,
              RoslynDiagnosticsResources.UseEmptyEnumerableMessage,
              ""Performance"",
              DiagnosticSeverity.Warning,
              isEnabledByDefault: true,
              customTags: WellKnownDiagnosticTags.Telemetry);
            ";


            String output01 =
@"internal static readonly DiagnosticDescriptor UseEmptyEnumerableRule = new DiagnosticDescriptor(
              RoslynDiagnosticIds.UseEmptyEnumerableRuleId,
              RoslynDiagnosticsResources.UseEmptyEnumerableDescription,
              RoslynDiagnosticsResources.UseEmptyEnumerableMessage,
              ""Performance"",
              DiagnosticSeverity.Warning,
              isEnabledByDefault: true,
              customTags: WellKnownDiagnosticTags.Telemetry);
            ";

            Tuple<String, String> tuple01 = Tuple.Create(input01, output01);
            Console.WriteLine(input01);
            Console.WriteLine(output01);
            tuples.Add(tuple01);

            String input02 =
@"internal static readonly DiagnosticDescriptor UseSingletonEnumerableRule = new DiagnosticDescriptor(
             ""RS0002"",
              RoslynDiagnosticsResources.UseSingletonEnumerableDescription,
              RoslynDiagnosticsResources.UseSingletonEnumerableMessage,
              ""Performance"",
              DiagnosticSeverity.Warning,
              isEnabledByDefault: true,
              customTags: WellKnownDiagnosticTags.Telemetry);
            ";


            String output02 =
@"internal static readonly DiagnosticDescriptor UseSingletonEnumerableRule = new DiagnosticDescriptor(
              RoslynDiagnosticIds.UseEmptyEnumerableRuleId,
              RoslynDiagnosticsResources.UseSingletonEnumerableDescription,
              RoslynDiagnosticsResources.UseSingletonEnumerableMessage,
              ""Performance"",
              DiagnosticSeverity.Warning,
              isEnabledByDefault: true,
              customTags: WellKnownDiagnosticTags.Telemetry);
            ";
            Tuple<String, String> tuple02 = Tuple.Create(input02, output02);
            Console.WriteLine(input02);
            Console.WriteLine(output02);
            tuples.Add(tuple02);
            return tuples;
        }

        /// <summary>
        /// Return the test data.
        /// </summary>
        /// <returns>Return a string to be tested.</returns>
        public override Tuple<String, String> Test()
        {
            String input01 =
@"internal static readonly DiagnosticDescriptor UseSingletonEnumerableRule = new DiagnosticDescriptor(
             ""RS0003"",
              RoslynDiagnosticsResources.UseSingletonEnumerableDescription,
              RoslynDiagnosticsResources.UseSingletonEnumerableMessage,
              ""Performance"",
              DiagnosticSeverity.Warning,
              isEnabledByDefault: true,
              customTags: WellKnownDiagnosticTags.Telemetry);
            ";

            String output01 =
@"internal static readonly DiagnosticDescriptor UseSingletonEnumerableRule = new DiagnosticDescriptor(
              RoslynDiagnosticIds.UseEmptyEnumerableRuleId,
              RoslynDiagnosticsResources.UseSingletonEnumerableDescription,
              RoslynDiagnosticsResources.UseSingletonEnumerableMessage,
              ""Performance"",
              DiagnosticSeverity.Warning,
              isEnabledByDefault: true,
              customTags: WellKnownDiagnosticTags.Telemetry);
            ";
            Tuple<String, String> test = Tuple.Create(input01, output01);
            return test;
        }
    }
}
