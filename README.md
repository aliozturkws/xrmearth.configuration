# :zap: Xrm Earth Configuration :floppy_disk:

* Allows storage and reading of settings on a custom entity or webresource on Dynamics CRM :+1:

## Description

* It provides flexible assembly developments that you can use in your Dynamics CRM processes. :running:
* Functions in this assembly aim to increase productivity by reducing development loads. :star:
* Can be used in plugin, web resource, workflow assembly, windows console app, windows service, web service. :sunglasses:

## Getting Started

### Dependencies

* Dynamics CRM V9 Recommended :heart:

### Installing

* Download solution [xrmearthconfiguration.zip](https://drive.google.com/file/d/1Du6HcDSkTQc234PdDdh78vL3MwNw5nOb/view?usp=sharing) and add it to crm. :floppy_disk:
* then customize it by reviewing the [example here](https://drive.google.com/drive/folders/1TqphqroZjoSX_5LiWJWiFe3lh3IZOUaE?usp=sharing) :point_left:

### Install Nuget Package
```
Install-Package XrmEarth.Configuration -Version 1.0.0
```

## Example Usage :monocle_face:

### CRM AppSettings For Entity :point_left:
```
public class AppSettings
{
  public CustomApi CustomApi { get; set; }
  public ReportServer ReportServer { get; set; }

  private static AppSettings _defaultSettings;
  private static DateTime _settingsValidUntil = DateTime.MinValue;
  public static AppSettings Default(IOrganizationService service, bool sandbox = true)
  {
    if (DateTime.UtcNow > _settingsValidUntil)
    {
      _defaultSettings = null;
    }

    if (_defaultSettings == null)
    {
      _defaultSettings = LoadSettings(service);
      _settingsValidUntil = DateTime.UtcNow.AddMinutes(5);
    }

     return _defaultSettings;
   }

   public static StartupConfiguration CreateConfig(IOrganizationService service)
   {
        return new StartupConfiguration
            {
                Targets = new TargetCollection
                {
                    new CrmStorageTarget(service)
                    {
                        Policy = new EntityStoragePolicy
                        {
                            Prefix = "new",
                            LogicalName = "configuration",
                            KeyAttributeLogicalName = "name",
                            ValueAttributeLogicalName = "value",
                        }
                    }
                }
            };
        }
   public static AppSettings LoadSettings(IOrganizationService service)
   {
      return ConfigurationManager.Load<AppSettings>(CreateConfig(service));
   }
}
```

### CRM AppSettings For WebResource :point_left:
```
public class AppSettings
{
   public CustomApi CustomApi { get; set; }
   public ReportServer ReportServer { get; set; }

   private static AppSettings _defaultSettings;
   private static DateTime _settingsValidUntil = DateTime.MinValue;
   public static AppSettings Default(IOrganizationService service, bool sandbox = true)
   {
      if (DateTime.UtcNow > _settingsValidUntil)
      {
            _defaultSettings = null;
      }

      if (_defaultSettings == null)
      {
              _defaultSettings = LoadSettings(service);
              _settingsValidUntil = DateTime.UtcNow.AddMinutes(5);
      }
      return _defaultSettings;
    }

    public static StartupConfiguration CreateConfig(IOrganizationService service)
    {
            return new StartupConfiguration
            {
                Targets = new TargetCollection
                {
                    new CrmStorageTarget(service)
                    {
                        Policy = new WebResourceStoragePolicy
                        {
                             Prefix = "new",
                             Name = "configuration",
                             Encoding = Encoding.UTF8
                        }
                    }
                }
            };
     }
     public static AppSettings LoadSettings(IOrganizationService service)
     {
         return ConfigurationManager.Load<AppSettings>(CreateConfig(service));
     }
}
```
### Sample Value Classes :footprints:
```
public class CustomApi
{
  public string BaseUrl { get; set; }
  public string UserName { get; set; }
  public string Password { get; set; }
}

public class ReportServer
{
  public string Url { get; set; }
  public string UserName { get; set; }
  public string Password { get; set; }
}
```
### Usage :unicorn:
```
CrmServiceClient adminClientService = XrmConnection.AdminCrmClient;
IOrganizationService orgService = adminClientService.GetOrganizationService();

var settings = AppSettings.Default(orgService);
string jsonFormatted = JValue.Parse(JsonConvert.SerializeObject(settings)).ToString(Formatting.Indented);

System.Console.WriteLine(jsonFormatted);
System.Console.ReadKey();
```

### Example Response :surfing_man:
```
{
  "CustomApi": {
    "BaseUrl": "http://api.xrmearth.com/",
    "UserName": "aliozturkws",
    "Password": "85*zjVK"
  },
  "ReportServer": {
    "Url": "https://backbonecrm.com/reports",
    "UserName": "reportadmin",
    "Password": "Op77Kvk"
  }
}
```
