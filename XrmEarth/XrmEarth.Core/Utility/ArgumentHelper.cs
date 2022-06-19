using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XrmEarth.Core.Arguments;
using XrmEarth.Core.Attributes;
using XrmEarth.Core.Common;
using XrmEarth.Core.Data.Converters;

namespace XrmEarth.Core.Utility
{
    public static class ArgumentHelper
    {
        public static List<ArgumentContainer> LoadArguments()
        {
            var parameters = GetParameters();
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;

            var arguments = new List<ArgumentContainer>();

            foreach (var parameter in parameters)
            {
                if (parameter.Any())
                {
                    foreach (var commandLineParameter in parameter)
                    {
                        arguments.Add(new ArgumentContainer
                        {
                            Key = parameter.Key,
                            Name = commandLineParameter.Name,
                            Value = commandLineParameter.Value,
                            Source = ArgumentSourceType.StartupArgs
                        });
                    }
                }
                else
                {
                    arguments.Add(new ArgumentContainer
                    {
                        Key = parameter.Key,
                        Name = null,
                        Value = null,
                        Source = ArgumentSourceType.StartupArgs
                    });
                }
            }

            foreach (var key in appSettings.AllKeys)
            {
                arguments.Add(new ArgumentContainer
                {
                    Key = key,
                    Value = appSettings[key],
                    Source = ArgumentSourceType.ConfigFile
                });
            }
            return arguments;
        }

        private static List<ArgumentContainer> _arguments;
        private static List<ArgumentContainer> Arguments
        {
            get
            {
                if (_arguments == null)
                    _arguments = LoadArguments();

                return _arguments;
            }
        }

        public static List<CommandLineParameterGroup> GetParameters()
        {
            return GetParameters(Environment.GetCommandLineArgs().Skip(1));
        }

        public static List<CommandLineParameterGroup> GetParameters(IEnumerable<string> args)
        {
            var cmdParametersList = new List<CommandLineParameterGroup>();

            var argsLine = String.Join(" ", args.Select(p => "\"" + p + "\""));
            var argsParts = argsLine.Split(new[] { "\"-" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var argPart in argsParts)
            {
                var cmdParameters = new CommandLineParameterGroup();
                var paramParts = argPart.Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);

                cmdParameters.Key = paramParts.First();
                foreach (var param in paramParts.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(param))
                        continue;

                    var cmdParameter = new CommandLineParameter();
                    var singleParamParts = param.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    cmdParameter.Name = singleParamParts.First();
                    if (singleParamParts.Length > 1)
                    {
                        cmdParameter.Value = string.Join(" ", singleParamParts.Skip(1));
                        cmdParameter.HasValue = true;
                    }
                    else
                    {
                        cmdParameter.HasValue = false;
                    }
                    cmdParameters.Add(cmdParameter);
                }
                cmdParametersList.Add(cmdParameters);
            }
            return cmdParametersList;
        }

        public static ArgumentContainer[] GetArguments()
        {
            return Arguments.ToArray();
        }

        public static void Initialize(object instance)
        {
            if (instance == null)
                return;

            var ownType = instance.GetType();

            var groupedContainers = ownType
                .GetProperties()
                .Where(p => p.GetCustomAttributes<ArgumentAttribute>().Any() && p.CanWrite)
                .Select(p => new BindArgumentContainer { Property = p, Attribute = p.GetCustomAttribute<ArgumentAttribute>() })
                .GroupBy(ap => new { ap.Attribute.Source, ap.Attribute.Key })
                .ToList();

            foreach (var enmField in Enum.GetValues(typeof(ArgumentSourceType)))
            {
                var enmValue = (int)enmField;
                foreach (var sourceContainers in groupedContainers.Where(c => (int)c.Key.Source == enmValue))
                {
                    foreach (var container in sourceContainers)
                    {
                        var keyVal =
                            string.IsNullOrEmpty(container.Attribute.Name)
                            ? Arguments.FirstOrDefault(aa => aa.Key == container.Attribute.Key && (int)aa.Source == enmValue)
                            : Arguments.FirstOrDefault(aa => aa.Key == container.Attribute.Key && aa.Name == container.Attribute.Name && (int)aa.Source == enmValue);
                        if (keyVal == null)
                        {
                            if (container.Attribute.Required)
                                throw new ArgumentException(string.Format("Uygulama '{0}' kaynağından, Anahtar : '{1}' İsim : '{2}' değerlerine erişemedi. Ayarlarınızı kontrol ediniz.", enmField, container.Attribute.Key, container.Attribute.Name));

                            if (container.Attribute.DefaultValue != null)
                                container.Property.SetValue(instance, container.Attribute.DefaultValue);

                            continue;
                        }

                        var val = keyVal.ActualValue;
                        if (container.Attribute.Converter != null)
                        {
                            var conv = Activator.CreateInstance(container.Attribute.Converter) as IValueConverter;
                            if (conv != null)
                                val = conv.Convert(val);
                        }
                        else
                        {
                            val = Convert.ChangeType(val, container.Property.PropertyType);
                        }

                        container.Property.SetValue(instance, val);
                    }
                }
            }
        }
    }
}
