
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ThisAssembly {
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Cecil.Rocks;
    using System.Collections.Generic;

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
                    var fileName_Modified = Path.Combine(dirName, fileNameShort + @".tmp");
                    var fileName_Backup = Path.Combine(dirName, fileNameShort + @".bak");
                    var fullpath = Path.GetFullPath(fileName_Original);
                    var rm = new ReaderParameters() { ThrowIfSymbolsAreNotMatching = true, ReadSymbols = true };
                    rm.InMemory = true;
                    var assembly = AssemblyDefinition.ReadAssembly(fileName_Original, rm);


                    var module = assembly.MainModule;
                    // ProcessCoreLibByReference(module);
                    
                    var modified = 0;
                    {
                        var OpCodes_fields = typeof(OpCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        var asmmap = OpCodes_fields.ToDictionary(x => x.Name.ToUpperInvariant());
                        asmmap.Add("Ldelem".ToUpperInvariant(), typeof(OpCodes).GetField(nameof(OpCodes.Ldelem_Any), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)!);
                        asmmap.Add("Stelem".ToUpperInvariant(), typeof(OpCodes).GetField(nameof(OpCodes.Stelem_Any), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)!);
                        asmmap.Add("Unaligned_".ToUpperInvariant(), typeof(OpCodes).GetField(nameof(OpCodes.Unaligned), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)!);
                        var int32Converter = new System.ComponentModel.Int32Converter();

                        var ilAttrName = "ILMethodBodyAttribute";

                        foreach (var typeref in module.GetTypes()) {
                            if (typeref is TypeDefinition type) {
                                foreach (var method in type.Methods) {
                                    if (!method.HasCustomAttributes) {
                                        continue;
                                    }

                                    CustomAttribute? ilAttr = GetCustomAttributeByName(method, ilAttrName);
                                    if (null == ilAttr) {
                                        continue;
                                    }
                                    Console.Out.Write($@"Assembling {method.FullName}... ");
                                    var ilSource = ilAttr.ConstructorArguments[0].Value as string;

                                    using var ilSourceReader = new StringReader(ilSource!);
                                    var tts = type.GenericParameters;
                                    var ts = method.GenericParameters;
                                    if (!method.HasBody) {
                                        Console.Out.WriteLine($@"No stub body found. Skipped.");
                                        continue;
                                    }
                                    {
                                        var bd = method.Body;
                                        var ins = bd.Instructions;
                                        var ilg = (ILProcessor?)null;
                                        ilg = bd.GetILProcessor();
                                        ilg.Clear();
                                        /*
                                        var insa = ins.ToArray();
                                        for (var i = 0; insa.Length > i; ++i) {
                                            var inn = insa[i];
                                            if (null != inn) {
                                                ilg.Remove(inn);
                                            }
                                        }
                                        */
                                        {
                                            string? ilasmLine;
                                            for (; null != (ilasmLine = ilSourceReader.ReadLine());) {
                                                var sdfasd = ilasmLine.Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries);
                                                var sdfas = sdfasd.FirstOrDefault();
                                                if (null == sdfas) {
                                                    continue;
                                                }
                                                var sdfa = sdfas.Replace('.', '_').ToUpperInvariant();
                                                var safsad = asmmap[sdfa];
                                                var sdfsaf = (safsad.GetValue(null) as OpCode?);
                                                if (null == sdfsaf) {
                                                    throw new Exception("gfghgh");
                                                }
                                                var sdfdsa = sdfsaf.Value;
                                                switch (sdfdsa.OperandType) {
                                                case OperandType.InlineBrTarget:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineField:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineI:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineI8:
                                                    ilg.Append(ilg.Create(sdfdsa, int.Parse(sdfasd[1])));
                                                    break;
                                                case OperandType.InlineMethod:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineNone:
                                                    ilg.Append(ilg.Create(sdfdsa));
                                                    break;
                                                case OperandType.InlinePhi:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineR:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineSig:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineString:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineSwitch:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineTok:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineType:
                                                    if (sdfasd[1].StartsWith("!!")) {
                                                        ilg.Append(ilg.Create(sdfdsa, ts[int.Parse(sdfasd[1][2..])]));
                                                    } else {
                                                        ilg.Append(ilg.Create(sdfdsa, tts[int.Parse(sdfasd[1][1..])]));
                                                    }
                                                    break;
                                                case OperandType.InlineVar:
                                                    throw new NotImplementedException();
                                                case OperandType.InlineArg:
                                                    throw new NotImplementedException();
                                                case OperandType.ShortInlineBrTarget:
                                                    throw new NotImplementedException();
                                                case OperandType.ShortInlineI:
                                                    var a = (int)int32Converter.ConvertFromString(sdfasd[1])!;
                                                    ilg.Append(ilg.Create(sdfdsa, checked((byte)a)));
                                                    break;
                                                case OperandType.ShortInlineR:
                                                    throw new NotImplementedException();
                                                case OperandType.ShortInlineVar:
                                                    throw new NotImplementedException();
                                                case OperandType.ShortInlineArg:
                                                    throw new NotImplementedException();
                                                default:
                                                    throw new Exception("il opcode arg");
                                                }



                                            }
                                        }
                                        method.CustomAttributes.Remove(ilAttr);

                                        ++modified;
                                        Console.Out.WriteLine($@"Done.");

                                        if (null != ilg) {
                                            var ignored = 0;
                                        }
                                    }

                                }
                            }
                        }

                    }
                    {
                        var ilAttrName = "ForceDiscardAttribute";
                        var pending = new List<TypeDefinition>();
                        foreach (var typeref in module.GetTypes()) {
                            if (typeref is TypeDefinition type) {
                                if (type.HasCustomAttributes) {
                                    CustomAttribute? ilAttr = GetCustomAttributeByName(type, ilAttrName);
                                    if (null != ilAttr) {
                                        pending.Add(type);
                                        continue;
                                    }
                                }
                                if (type.Name.EndsWith("_ProcessedByFody")) {
                                    pending.Add(type);
                                    continue;
                                }
                            }
                        }
                        foreach (var typeref in module.GetTypes().ToList()) {
                            if (typeref is TypeDefinition type) {
                                if (pending.Contains(type)) {
                                    continue;
                                }
                                foreach (var m in type.Fields.ToList()) {
                                    if (!m.HasCustomAttributes) {
                                        continue;
                                    }
                                    CustomAttribute? ilAttr = GetCustomAttributeByName(m, ilAttrName);
                                    if (null == ilAttr) {
                                        continue;
                                    }
                                    Console.Out.Write($@"Removing {m.FullName}... ");
                                    type.Fields.Remove(m);
                                    ++modified;
                                    Console.Out.WriteLine($@"Done.");
                                }

                                foreach (var m in type.Methods) {
                                    if (!m.HasCustomAttributes) {
                                        continue;
                                    }
                                    CustomAttribute? ilAttr = GetCustomAttributeByName(m, ilAttrName);
                                    if (null == ilAttr) {
                                        continue;
                                    }
                                    Console.Out.Write($@"Removing {m.FullName}... ");
                                    type.Methods.Remove(m);
                                    ++modified;
                                    Console.Out.WriteLine($@"Done.");
                                }
                                foreach (var m in type.Properties) {
                                    if (!m.HasCustomAttributes) {
                                        continue;
                                    }
                                    CustomAttribute? ilAttr = GetCustomAttributeByName(m, ilAttrName);
                                    if (null == ilAttr) {
                                        continue;
                                    }
                                    Console.Out.Write($@"Removing {m.FullName}... ");
                                    type.Properties.Remove(m);
                                    ++modified;
                                    Console.Out.WriteLine($@"Done.");
                                }

                                foreach (var m in type.Events) {
                                    if (!m.HasCustomAttributes) {
                                        continue;
                                    }
                                    CustomAttribute? ilAttr = GetCustomAttributeByName(m, ilAttrName);
                                    if (null == ilAttr) {
                                        continue;
                                    }
                                    Console.Out.Write($@"Removing {m.FullName}... ");
                                    type.Events.Remove(m);
                                    ++modified;
                                    Console.Out.WriteLine($@"Done.");
                                }

                            }
                        }
                        foreach (var type in pending) {
                            Console.Out.Write($@"Removing {type.FullName}... ");
                            module.Types.Remove(type);
                            ++modified;
                            Console.Out.WriteLine($@"Done.");
                        }
                    }
                    if (modified <= 0) {
                        return (int)ExitStatus.RequestFailed;
                    }
                    Console.Out.WriteLine($@"Modified: {modified}.");
                    var sss = (System.Reflection.StrongNameKeyPair?)null;
                    try {
                        var keyfile = args?[1];
                        if (null != keyfile) {
                            sss = new System.Reflection.StrongNameKeyPair(File.ReadAllBytes(keyfile));
                        }
                    } catch (Exception) {
                    }

                    Console.Out.Write($@"Writing file: {fileName_Modified}... ");
                    var wp = new WriterParameters() { WriteSymbols = true };
                    assembly.Write(fileName_Modified, wp);
                    Console.Out.WriteLine($@"Done.");
                    File.Replace(fileName_Modified, fileName_Original, fileName_Backup);
                    Console.Out.WriteLine($@"""{fileName_Original}"" -> ""{fileName_Backup}""");
                    Console.Out.WriteLine($@"""{fileName_Modified}"" -> ""{fileName_Original}""");
                    Console.Out.WriteLine(@"Done.");
                    return (int)ExitStatus.OK;
                } catch (Exception ex) {
                    Console.Error.WriteLine(ex);
                    return (int)ExitStatus.RequestFailed;
                }
            }
            return (int)ExitStatus.UserCanceled;
        }

        private static void ProcessCoreLibByReference(ModuleDefinition module) {
            TypeReference MockByReferenceTypeReference = module.GetType("System", "ByReference`1");
            var assemblyNameReference = module.AssemblyReferences
                .Where(x => x.Name == "System.Runtime").Single();
            var CoreLibAssemblyNameReference = AssemblyNameReference.Parse("System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e");
            CoreLibAssemblyNameReference.Version = (Version)assemblyNameReference.Version.Clone();

            var ByReferenceTypeReference = new TypeReference("System", "ByReference`1", module, CoreLibAssemblyNameReference);

            var tByReference = Type.GetType("System.ByReference`1");

            var ByReferenceTypeReferenceA = module.ImportReference(tByReference);
            var sdfddda = ByReferenceTypeReferenceA.Scope as AssemblyNameReference;
            module.AssemblyReferences.Remove(sdfddda);

            module.AssemblyReferences.Add(CoreLibAssemblyNameReference);
            ByReferenceTypeReferenceA.Scope = CoreLibAssemblyNameReference;

            var WarppedByReferenceTypeDefinition = module.GetType("UltimateOrb", "ByReference`1");
            var WarppedReadOnlyByReferenceTypeDefinition = module.GetType("UltimateOrb", "ReadOnlyByReference`1");

            {
                var type = WarppedByReferenceTypeDefinition;
                var tFrom = MockByReferenceTypeReference;

                var tTo = ByReferenceTypeReferenceA;

                var tT = type.GenericParameters[0];
                var tFromR = tFrom.MakeGenericInstanceType(tT);

                var tToR = tTo.MakeGenericInstanceType(tT);

                foreach (var m in type.Fields) {
                    if (m.FieldType.FullName == tFromR.FullName) {
                        m.FieldType = tTo;
                    }
                }

                foreach (var m in type.Methods) {
                    var a = 0;
                    foreach (var item in
                        from v in m.Body.Variables
                        where v.VariableType.FullName == tFromR.FullName
                        select (v.VariableType = tTo) into b
                        select 1) {
                        a += item;
                    }
                    var sdfa = m.Body.GetILProcessor();
                    foreach (var item in m.Body.Instructions.ToArray()) {
                        var sdaf = item;
                        var sdfs = item.Operand;
                        var sdfssda = sdfs?.GetType()?.FullName;
                        if (sdfs is MethodReference aa) {

                            if (aa.DeclaringType.FullName == tFromR.FullName) {
                                aa.DeclaringType = tToR;
                            }
                        }
                    }
                }
                foreach (var m in type.Properties) {

                }

                foreach (var m in type.Events) {

                }

                foreach (var m in type.CustomAttributes) {

                }
                foreach (var m in type.GenericParameters) {

                }
            }
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
