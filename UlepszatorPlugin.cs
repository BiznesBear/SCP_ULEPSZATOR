using CommandSystem;
using LabApi.Features;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using System.Reflection;
using System.Text.Json;

namespace SCP_ULEPSZATOR;
public class UlepszatorPlugin : Plugin
{
    public override string Name { get; } = "Ulepszator";

    public override string Description { get; } = "This ulepszatorPlugin does magic!";


    public override string Author { get; } = "Me!";


    public override Version Version { get; } = new Version(1, 0, 0, 0);


    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);

    public override void Disable()
    {
        LabApi.Loader.CommandLoader.UnregisterCommands(this);
    }

    public override void Enable()
    {
        LabApi.Loader.CommandLoader.RegisterCommands(this);
    }


    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class UlepszatorCommand : ICommand
    {
        public string Command { get; } = "ulepszator"; // The command used in the console.
        public string[] Aliases { get; } = Array.Empty<string>(); // The desired aliases.
        public string Description { get; } = "SCP ULEPSZSATOR"; // A small description.

        public bool Execute(ArraySegment<string> args, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.AdminChat))
            {
                response = "You don't have permissions to manage ulepszator fool";
                return false;
            }

            if (args.Count < 1)
            {
                response = "ulepszator is at your service, sir...";
                return true;
            }

            switch (args[0])
            {
                case "reload":
                    Core.Instance.LoadConfig();
                    response = "Reloaded config.";
                    return true;
                case "rebuilt":
                    Core.Instance.RebuiltConfig();
                    response = "Config totaly rebuilt.";
                    return true;
                case "reset":
                    Core.Instance.ResetConfig();
                    response = "Reseted config to defalut. Remeber to rebuilt it to overrite config file with changes.";
                    return true;
                case "set":

                    if(args.Count > 2)
                    {
                        var configType = typeof(UlepszatorConfig);
                        var parameterName = args[1];
                        var value = args[2];

                        PropertyInfo pi = configType.GetProperty(parameterName);

                        if(pi is null)
                        {
                            response = "Invalid property argument";
                            return false;
                        }

                        var convertedValue = Convert.ChangeType(value, pi.PropertyType);

                        if(convertedValue is null)
                        {
                            response = "Invalid value argument";
                            return false;
                        }
                        
                        try
                        {
                            pi.SetValue(Core.Instance.Config, convertedValue, null);
                        }
                        catch 
                        {
                            response = $"Prompted invalid type of parameter {convertedValue.GetType().Name}. Should be ${pi.PropertyType.Name}";
                            return false;
                        }

                        response = $"Sucessfuly changed value of {pi.Name} to {value}";
                        return true;

                    }


                    response = "Invalid args";
                    return false;
                case "settings":
                    response = $"Current SCP_ULEPSZATOR settings: \n {JsonSerializer.Serialize(Core.Instance.Config, Core.jsonSerializerOptions)}";
                    return true;
                case "test":
                    if(args.Count > 1)
                    {
                        switch(args[1])
                        {
                            case "engage":
                                if(args.Count > 2)
                                {
                                    switch(args[2])
                                    {
                                        case "1":
                                            Generator.List.ElementAt(0).Engaged = true;
                                            response = "Engaged generator 1";
                                            return true;
                                        case "2":
                                            Generator.List.ElementAt(1).Engaged = true;
                                            response = "Engaged generator 2";
                                            return true;
                                        case "3":
                                            Generator.List.ElementAt(2).Engaged = true;
                                            response = "Engaged generator 3";
                                            return true;
                                        case "all":
                                            foreach (var g in Generator.List)
                                            {
                                                g.Engaged = true;
                                            }
                                            response = "Engaged all";
                                            return true;
                                    }
                                    
                                }
                                response = "No specified generator/s";
                                return false;
                            case "fastGenerators":
                                foreach (var g in Generator.List)
                                {
                                    g.TotalActivationTime = 5;
                                }
                                response = "Generators are now creazy fast";
                                return true;
                            case "warheadMessage":
                                Announcer.Clear();
                                Announcer.GlitchyMessage(Core.WarheadDetonationMessage, 0.3f, 0.4f);

                                response = "message test";
                                return Announcer.IsValid(Core.WarheadDetonationMessage);
                            default:
                                response = "Invalid argument: " + args[1];
                                return false;
                        }
                    }

                    response = "No arguments";
                    return false;
            }

            response = "invalid ulepszator args";
            return true;
        }
    }
}