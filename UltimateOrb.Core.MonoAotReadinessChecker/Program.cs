using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection;

namespace ThisAssembly {
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;

    public static class Program {

        public enum ExitStatus {
            OK,
            UserCanceled,
            RequestFailed
        }

        public static int Main(string[] args) {
            for (; null != args;) {
                try {
                    var fileName = args?[0];
                    if (null == fileName) {
                        return (int)ExitStatus.UserCanceled;
                    }
                    var dirName = Path.GetDirectoryName(fileName)!;
                    var fileNameShort = Path.GetFileNameWithoutExtension(fileName);
                    var fileNameExt = Path.GetExtension(fileName);

                    var fileName_Original = Path.Combine(dirName, fileNameShort + fileNameExt);
                    var fullpath = Path.GetFullPath(fileName_Original);
                    var rm = new ReaderParameters() { ThrowIfSymbolsAreNotMatching = true, ReadSymbols = true };
                    rm.InMemory = true;
                    var assembly = AssemblyDefinition.ReadAssembly(fileName_Original, rm);


                    var modulus = assembly.MainModule;
                    // ProcessCoreLibByReference(modulus);
                    var found = 0;
                    {

                        foreach (var typeref in modulus.GetTypes()) {
                            if (typeref.IsNested) {
                                if (!(typeref.GenericParameters.Count < 16)) {
                                    ++found;
                                    Console.Out.WriteLine($@"E: Type: {typeref.FullName}");
                                    continue;
                                }
                            }
                            
                            if (typeref.HasMethods) {
                                foreach (var method in typeref.Methods) {
                                    if (method.GenericParameters.Count < 16) {
                                        continue;
                                    }
                                    ++found;
                                    Console.Out.WriteLine($@"E: Method: {method.FullName}");
                                }
                            }
                        }
                    }
                    Console.Out.WriteLine($@"Found: {found}.");

                    Console.Out.WriteLine(@"Done.");
                    return (int)ExitStatus.OK;
                } catch (Exception ex) {
                    Console.Error.WriteLine(ex);
                    return (int)ExitStatus.RequestFailed;
                }
            }
            return (int)ExitStatus.UserCanceled;
        }

        private static CustomAttribute? GetCustomAttributeByName(ICustomAttributeProvider member, string name) {
            var result = default(CustomAttribute);
            var attrs = member.CustomAttributes;
            if (null != attrs) {
                try {
                    foreach (var customAttribute in attrs) {
                        try {
                            if (name == customAttribute?.AttributeType.Name) {
                                result = customAttribute;
                                break;
                            }
                        } catch (Exception) {
                        }
                    }
                } catch (Exception) {
                }
            }
            return result;
        }
    }
}
