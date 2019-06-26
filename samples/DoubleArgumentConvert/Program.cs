using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Globalization;
using System.Threading.Tasks;

namespace DoubleArgumentConvert
{
    public static class Program
    {
        public static Task<int> Main(string[] args)
        {
            var parser = new CommandLineBuilder(
                new RootCommand
                {
                    Handler = CommandHandler.Create<int>(Run1)
                })
                .AddOption(new Option(new[] { "-v", "--value" })
                {
                    Argument = new Argument<int>(TryConvertHexArgument)
                })
                .UseDefaults()
                .Build();

            return parser.InvokeAsync(args);

            static bool TryConvertHexArgument(SymbolResult result, out int value)
            {
                Console.WriteLine("Invoked conversion method: " + nameof(TryConvertHexArgument));

                const string hexPrefix = "0x";

                var arg = result.Token.Value ?? string.Empty;
                if (arg.StartsWith(hexPrefix, StringComparison.OrdinalIgnoreCase))
                    arg = arg.Substring(hexPrefix.Length);
                return int.TryParse(arg, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
            }
        }

        internal static void Run1(int value) => Console.WriteLine($"{nameof(Run1)}: {value}");

        internal static int Run2(ParseResult parseResult)
        {
            int value;

            var optionResult = parseResult.CommandResult.OptionResult(nameof(value));
            value = optionResult.GetValueOrDefault<int>();

            Console.WriteLine($"{nameof(Run2)}: {value}");

            return value;
        }
    }
}
