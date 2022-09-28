using System.Text.Json;

namespace KMS_Helper
{
    public sealed class JsonHandler
    {
        public event MyEventHandler OnSaved;
        public string fileName { get; set; } = "config.json";
        public string folderName { get; set; } = "KMS-Helper";
        private string folderPath;
        private string filePath;

        public JsonHandler()
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/" + folderName;
            filePath = folderPath + "/" + fileName;
        }

        public async Task WriteSettings(Setting setting)
        {
            try
            {
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                using FileStream createStream = File.Create(filePath);
                await JsonSerializer.SerializeAsync(createStream, setting, jsonOptions);
                await createStream.DisposeAsync();
                if (OnSaved != null)
                    OnSaved(this, new MyEventArgs("Saved"));
            }
            catch
            {
                if (OnSaved != null)
                    OnSaved(this, new MyEventArgs("Failed"));
            }
        }

        public Setting GetSettings()
        {
            if (File.Exists(filePath))
            {
                using FileStream openStream = File.OpenRead(filePath);
                Setting setting = JsonSerializer.Deserialize<Setting>(openStream);
                openStream.Dispose();
                return setting;
            }
            return null;
        }

        public async Task DeleteSettings()
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath);
        }

        public bool SettingsExist()
        {
            return File.Exists(filePath);
        }
    }

    public delegate void MyEventHandler(object source, MyEventArgs e);
    public class MyEventArgs : EventArgs
    {
        private string EventInfo;
        public MyEventArgs(string Text)
        {
            EventInfo = Text;
        }
        public string GetInfo()
        {
            return EventInfo;
        }
    }
}
