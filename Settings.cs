using DreamPoeBot.Loki;
using DreamPoeBot.Loki.Common;

namespace DevReload;


public class Settings : JsonSettings
{
	private static Settings _instance;



	private string _targetPath = "dev.dll";

	public static Settings Instance => _instance ?? (_instance = new Settings());

	public string TargetPath
	{
		get
		{
			return _targetPath;
		}
		set
		{
			_targetPath = value;
		}
	}

	private Settings()
		: base(JsonSettings.GetSettingsFilePath(Configuration.Instance.Name, "DevReload.json"))
	{
	}

	

	static Settings()
	{
		
	}
}
